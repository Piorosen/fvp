using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> where T : class, new()
{
    private static T _instance;

    public static T Insatence
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}
