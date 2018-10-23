#pragma once

#include <boost/asio.hpp>
#include "socket_resource.hpp"
#include "object_pool.hpp"
#include "ref.hpp"

namespace nta
{
  template < typename SessionType >
  class tcp_server
  {
  public:

    explicit tcp_server(const std::string& addr, unsigned short port) :
      tcp_server(addr, port, 1)
    {
    }

    explicit tcp_server(const std::string& addr, unsigned short port, unsigned int threadCount, unsigned int maxSessionCount = 1024) :
      endpoint(boost::asio::ip::tcp::endpoint(boost::asio::ip::address::from_string(addr), port)),
      acceptor(context),
      threadCount(threadCount),
      maxSessionCount(maxSessionCount)
    {
    }

    ~tcp_server()
    {
    }

    void run()
    {
      acceptor.open(endpoint.protocol());
      acceptor.bind(endpoint);
      acceptor.listen();

      std::vector<std::thread> subThreads;
      subThreads.reserve(threadCount - 1);
      for (auto& subThread : subThreads)
      {
        subThread = std::thread([this] { context.run(); });
      }

      resource_pool pool(this, maxSessionCount);
      pool.accept_all();

      context.run();

      for (auto& subThread : subThreads)
      {
        subThread.join();
      }

      acceptor.close();
      context.reset();
    }

    void stop()
    {
      context.stop();
    }

    template < typename Func >
    void post(Func&& func)
    {
      context.post(std::forward<Func>(func));
    }

  private:

    class resource_pool : public object_pool
    {
    public:

      resource_pool(tcp_server* server, unsigned int size) : 
        size(size), 
        server(server)
      {
        resources = static_cast<socket_resource*>(::operator new(sizeof(socket_resource) * size));
        sessions = static_cast<SessionType*>(::operator new(sizeof(SessionType) * size));

        for (auto i = size; i != 0; --i)
        {
          new (&resources[i]) socket_resource(server->context, *this);
        }
      }

      ~resource_pool()
      {
        for (auto i = size; i != 0; --i)
        {
          resources[i].~socket_resource();
        }
        ::operator delete(resources);

        for (auto i = size; i != 0; --i)
        {
          sessions[i].~SessionType();
        }
        ::operator delete(sessions);
      }

      void accept_all()
      {
        for (auto i = size; i != 0; --i)
        {
          server->accept(sessions + i, resources + i);
        }
      }

      void* alloc() override
      {
        return nullptr;
      }

      void dealloc(void* obj) override
      {
        SessionType* session = static_cast<SessionType*>(obj);
        socket_resource* resource = session->get_socket_resource();
        {
          if (resource->socket.is_open())
          {
            resource->socket.close();
          }
          resource->sendBuffers.clear();
          while (!resource->sendQueue.empty())
          {
            resource->sendQueue.pop();
          }
          resource->recvBuf.clear();
        }
        session->~SessionType();
        server->accept(session, resource);
      }

    private:

      tcp_server* server;
      SessionType* sessions;
      socket_resource* resources;
      const unsigned int size;
    };

    void accept(SessionType* session, socket_resource* resource)
    {
      acceptor.async_accept(resource->socket, [this, resource, session](const auto& err)
      {
        if (!err)
        {
          new (session)SessionType();
          resource->handler = static_cast<nta::event_handler*>(session);
          nta::ref<SessionType> s(session);
          s->run(resource);
        }
        else
        {
          if (acceptor.is_open())
          {
            accept(session, resource);
          }
        }
      });
    }

    boost::asio::ip::tcp::endpoint endpoint;
    boost::asio::io_context context;
    boost::asio::ip::tcp::acceptor acceptor;
    unsigned int threadCount;
    unsigned int maxSessionCount;
  };
}