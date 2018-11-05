#pragma once

#include <thread>
#include <boost/asio/io_context.hpp>
#include <boost/asio/ip/tcp.hpp>
#include "socket_resource.hpp"
#include "object_pool.hpp"

namespace nta
{
  namespace details
  {
  }

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
	  if (!context.stopped())
	  {
		context.stop();
	  }
    }

    void run()
    {
      acceptor.open(endpoint.protocol());
      acceptor.bind(endpoint);
      acceptor.listen();

	  resource_pool pool(this, maxSessionCount);
	  pool.accept_all();

	  std::vector<std::thread> subThreads;
	  if (1 < threadCount)
	  {
		subThreads.reserve(threadCount - 1);
		for (unsigned int i = 1; i != threadCount; ++i)
		{
		  subThreads.emplace_back([this] { context.run(); });
		}
	  }
      context.run();

      for (std::thread& subThread : subThreads)
      {
        subThread.join();
      }
    }

    void stop()
    {
      context.stop();
	  acceptor.close();
    }

    template < typename Func >
    inline void post(Func&& func)
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
		for (unsigned int i = 0; i != size; ++i)
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
	  }

	  void accept_all()
	  {
		for (auto i = size; i != 0; --i)
		{
		  server->accept(resources + i);
		}
	  }

	  void* alloc() override
	  {
		return nullptr;
	  }

	  void dealloc(void* obj) override
	  {
		socket_resource* resource = static_cast<socket_resource*>(obj);
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
		server->accept(resource);
	  }

    private:

      tcp_server* server;
      SessionType* sessions;
      socket_resource* resources;
      const unsigned int size;
    };

    void accept(socket_resource* resource)
    {
      acceptor.async_accept(resource->socket, [this, resource](const auto& err)
      {
        if (!err)
        {
		  std::make_shared<SessionType>()->run(resource);
        }
        else
        {
          if (acceptor.is_open())
          {
            accept(resource);
          }
        }
      });
    }

    const boost::asio::ip::tcp::endpoint endpoint;
    boost::asio::io_context context;
    boost::asio::ip::tcp::acceptor acceptor;
    const unsigned int threadCount;
    const unsigned int maxSessionCount;
	
  };
}