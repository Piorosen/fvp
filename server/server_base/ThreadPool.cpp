#include "ThreadPool.h"

ThreadPool::ThreadPool()
{
}

ThreadPool::~ThreadPool()
{
}

void ThreadPool::Stop()
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

void ThreadPool::Run(int numThreads)
{
	threads.resize(numThreads);
	for (auto& t : threads)
	{
		t = std::thread([this] {
			boost::asio::io_context::work work(context);
			context.run();
		});
	}
}