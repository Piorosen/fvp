#pragma once

#include <boost/intrusive_ptr.hpp>

namespace nta
{
  template < typename Type >
  class ref
  {
  public:

	

    ref() noexcept : ptr(nullptr)
    {
    }

    ref(Type* rhs) noexcept : ptr(rhs)
    {
      if (ptr)
      {
        ptr->ref();
      }
    }

    ref(ref<Type>&& rhs) noexcept : ptr(rhs.ptr)
    {
      rhs.ptr = nullptr;
    }

    ref(ref<Type> const & rhs) noexcept : ptr(rhs.ptr)
    {
      if (ptr)
      {
        ptr->ref();
      }
    }

	template<class Other> friend class ref;

	template < typename Other >
	ref(ref<Other>&& rhs) noexcept : ptr(rhs.ptr)
	{
	  rhs.ptr = nullptr;
	}

    ~ref() noexcept
    {
      if (ptr)
      {
        ptr->unref();
      }
    }

    inline Type* get() const
    {
      return ptr;
    }

    inline Type* operator->() noexcept
    {
      return ptr;
    }

    inline const Type* const operator->() const noexcept
    {
      return ptr;
    }

    inline Type& operator*() const noexcept
    {
      return *ptr;
    }

    inline ref<Type>& operator=(Type* rhs) noexcept
    {
	  if (rhs)
	  {
		rhs->ref();
	  }
      if (ptr)
      {
        ptr->unref();
      }
      ptr = rhs;
      return *this;
    }

    inline ref<Type>& operator=(ref<Type>&& rhs) noexcept
    {
      if (ptr)
      {
        ptr->unref();
      }
      ptr = rhs.ptr;
      rhs.ptr = nullptr;
      return *this;
    }

    inline ref<Type>& operator=(const ref<Type>& rhs) noexcept
    {
	  rhs->ref();
      if (ptr)
      {
        ptr->unref();
      }
      ptr = rhs.ptr;
      return *this;
    }

	template < typename Other >
	ref<Type>& operator=(ref<Other> const & rhs) noexcept
	{
	  rhs->ref();
	  if (ptr)
	  {
		ptr->unref();
	  }
	  ptr = rhs.ptr;
	  return *this;
	}

	template<class Other>
	ref<Type>& operator=(ref<Other>&& rhs) noexcept
	{
	  ptr = rhs.ptr;
	  rhs->ptr = nullptr;
	  return *this;
	}

	explicit operator bool() const noexcept
	{
	  return ptr != nullptr;
	}

  private:

    Type* ptr;
  };
}