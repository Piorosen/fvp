#pragma once

#include <thread>
#include <iostream>
#include <boost/asio.hpp>
#include "Singleton.h"

class ThreadPool : public Singleton<ThreadPool>
{
public:

  template < typename Func >
  void Post(Func&& func)
  {
    context.post(std::forward<Func>(func));
  }

  void Stop()
  {
    context.stop();
    for (auto& t : threads)
    {
      if (t.joinable())
      {
        t.join();
      }
    }
    context.reset();
  }

  void Run(int numThreads)
  {
    //auto work = boost::asio::make_work_guard(context);
    threads.resize(numThreads);
    for (auto& t : threads)
    {
      t = std::thread([this] {
        boost::asio::io_context::work work(context);
        context.run();
        std::cout << "thread closed" << std::endl;
      });
    }
  }

private:

  boost::asio::io_context context;
  std::vector<std::thread> threads;
};