#pragma once

#include <vector>
#include <atomic>
#include <boost/asio.hpp>
#include "ref.hpp"
#include "event_handler.hpp"
#include "socket_resource.hpp"

namespace nta
{
  class tcp_session : public nta::event_handler
  {
  public:

    explicit tcp_session()
    {
    }

    virtual ~tcp_session()
    {
    }

    void run(socket_resource* rsc)
    {
      this->resource = rsc;

      post([this, self = self()]{
        on_connected();
        if (resource->recvBuf.empty())
        {
          resource->recvBuf.resize(1024 * 4);
        }
        do_receive();
        });
    }

    void send(const void* data, std::size_t size)
    {
      std::vector<char> buf;
      buf.resize(size);
      memcpy(buf.data(), data, size);

      post([this, self = self(), buf = std::move(buf)]{
        const bool stopped = resource->sendQueue.empty();
        resource->sendQueue.push(std::move(buf));
        if (stopped)
        {
          do_send();
        }
      });
    }

    boost::asio::ip::tcp::endpoint get_local_endpoint() const
    {
      return resource->socket.local_endpoint();
    }

    boost::asio::ip::tcp::endpoint get_remote_endpoint() const
    {
      return resource->socket.remote_endpoint();
    }

    void shutdown_send()
    {
      dispatch([this, self = self()]{
        resource->socket.shutdown(resource->socket.shutdown_send);
      });
    }

    void shutdown()
    {
      dispatch([this, self = self()]{
        resource->socket.shutdown(resource->socket.shutdown_both);
      });
    }

    template < typename Func >
    void post(Func&& func)
    {
      resource->strand.post(std::forward<Func>(func));
    }

    template < typename Func >
    void dispatch(Func&& func)
    {
      resource->strand.dispatch(std::forward<Func>(func));
    }

    socket_resource* get_socket_resource()
    {
      return resource;
    }

    void ref()
    {
      ++refCounter;
    }

    void unref()
    {
      const auto count = --refCounter;
      if (count == 0 && resource)
      {
        resource->pool.dealloc(this);
        resource = nullptr;
      }
    }

    nta::ref<tcp_session> self()
    {
      return this;
    }

  protected:

    boost::asio::ip::tcp::socket& get_socket()
    {
      return resource->socket;
    }

  private:

    void do_send()
    {
      resource->sendBuffers.reserve(resource->sendQueue.size());

      while (!resource->sendQueue.empty())
      {
        resource->sendBuffers.emplace_back(std::move(resource->sendQueue.front()));
        resource->sendQueue.pop();
      }

      boost::asio::async_write(
        resource->socket,
        boost::asio::buffer(resource->sendBuffers),
        resource->strand.wrap([this, self = self()](const auto& err, auto){
        on_sent(err);
        if (!err || (err == boost::asio::error::would_block))
        {
          resource->sendBuffers.clear();
          if (!resource->sendQueue.empty())
          {
            do_send();
          }
        }
      }));
    }

    void do_receive()
    {
      resource->socket.async_receive(
        boost::asio::buffer(resource->recvBuf),
        resource->strand.wrap([this, self = self()](const auto& err, auto bytes){
        on_received(err, resource->recvBuf.data(), bytes);
        if (!err || (err == boost::asio::error::would_block))
        {
          if (resource->recvBuf.size() == bytes)
          {
            const auto sizeOld = resource->recvBuf.size();
            resource->recvBuf.resize(sizeOld + (sizeOld / 2));
          }
          do_receive();
        }
        else
        {
          on_disconnected();
        }
      }));
    }

    socket_resource* resource;
    std::int32_t refCounter = 0;
  };
}