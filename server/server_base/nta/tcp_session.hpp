#pragma once

#include <boost/asio/buffer.hpp>
#include <boost/asio/write.hpp>
#include "event_handler.hpp"
#include "socket_resource.hpp"

namespace nta
{
  class tcp_session : public nta::event_handler, public std::enable_shared_from_this<tcp_session>
  {
  public:

    explicit tcp_session()
    {
    }

    virtual ~tcp_session()
    {
	  if (resource)
	  {
		resource->pool.dealloc(resource);
	  }
    }

    void run(socket_resource* rsc)
    {
      this->resource = rsc;

      post([self = shared_from_this()]
	  {
		self->on_connected();
		if (self->resource->recvBuf.empty())
		{
		  self->resource->recvBuf.resize(1024 * 4);
		}
		self->do_receive(std::move(self));
      });
    }

    void send(const void* data, std::size_t size)
    {
	  // @todo 내부 버퍼 풀을 이용하도록 
      std::vector<char> buf;
      buf.resize(size);
      memcpy(buf.data(), data, size);

      post([self = shared_from_this(), buf = std::move(buf)]
	  {
        const bool stopped = self->resource->sendBuffers.empty();
		self->resource->sendQueue.push(std::move(buf));
        if (stopped)
        {
		  self->do_send(std::move(self));
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
      dispatch([this, self = shared_from_this()]
	  {
        resource->socket.shutdown(resource->socket.shutdown_send);
      });
    }

    void shutdown()
    {
      dispatch([this, self = shared_from_this()]
	  {
        resource->socket.shutdown(resource->socket.shutdown_both);
      });
    }

    template < typename Func >
    inline void post(Func&& func)
    {
      resource->strand.post(std::forward<Func>(func));
    }

    template < typename Func >
    inline void dispatch(Func&& func)
    {
      resource->strand.dispatch(std::forward<Func>(func));
    }

  protected:

    boost::asio::ip::tcp::socket& get_socket()
    {
      return resource->socket;
    }

  private:

    void do_send(std::shared_ptr<tcp_session> self)
    {
	  const auto sendBufferSize = resource->sendQueue.size();
	  if (resource->sendBuffers.size() < sendBufferSize)
	  {
		resource->sendBuffers.reserve(sendBufferSize);
		resource->sendBuffers2.reserve(sendBufferSize);
	  }
      
	  while (!resource->sendQueue.empty())
      {
		auto& front = resource->sendQueue.front();
        resource->sendBuffers.emplace_back(std::move(front));
		resource->sendBuffers2.push_back(boost::asio::buffer(resource->sendBuffers.back()));
        resource->sendQueue.pop();
      }

      boost::asio::async_write(
        resource->socket,
		resource->sendBuffers2,
        resource->strand.wrap([this, self = std::move(self)](const auto& err, auto bytes)
		{
		  resource->sendBuffers2.clear();
		  resource->sendBuffers.clear();

		  on_sent(err);
		  
		  if (!err || (err == boost::asio::error::would_block))
		  {
			if (!resource->sendQueue.empty())
			{
			  do_send(std::move(self));
			}
		  }
      }));
    }

    void do_receive(std::shared_ptr<tcp_session> self)
    {
      resource->socket.async_receive(
        boost::asio::buffer(resource->recvBuf),
        resource->strand.wrap([this, self = std::move(self)](const auto& err, auto bytes)
		{
		  // @todo 수신을 알리기 전에 버퍼의 남은 부분으로 receive를 받도록
		  on_received(err, resource->recvBuf.data(), bytes);
		  if (!err || (err == boost::asio::error::would_block))
		  {
			if (resource->recvBuf.size() == bytes)
			{
			  const auto sizeOld = resource->recvBuf.size();
			  resource->recvBuf.resize(sizeOld + (sizeOld / 2));
			}
			do_receive(std::move(self));
		  }
		  else
		  {
			on_disconnected();
		  }
		}));
    }

    socket_resource* resource;
  };
}