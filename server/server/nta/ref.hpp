#pragma once

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

    ref(const ref<Type>& rhs) noexcept : ptr(rhs.ptr)
    {
      if (ptr)
      {
        ptr->ref();
      }
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
      if (ptr)
      {
        ptr->unref();
      }
      ptr = rhs;
      if (ptr)
      {
        ptr->ref();
      }
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
      if (ptr)
      {
        ptr->unref();
      }
      ptr = rhs.ptr;
      if (ptr)
      {
        ptr->ref();
      }
      return *this;
    }

  private:

    Type* ptr;
  };
}