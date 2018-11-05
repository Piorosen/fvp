#pragma once

#include <thread>
#include <iostream>
#include <boost/asio/io_context.hpp>
#include "Singleton.h"

class ThreadPool : public Singleton<ThreadPool>
{
public:

  friend class Singleton<ThreadPool>;

  ~ThreadPool();

  template < typename Func >
  inline void Post(Func&& func)
  {
    context.post(std::forward<Func>(func));
  }

  void Stop();

  void Run(int numThreads);

private:

  ThreadPool();

  boost::asio::io_context context;
  std::vector<std::thread> threads;
};