using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{

    public static float DivX = 0.0f;
    public static float DivY = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    public void Vertical(float y)
    {
        DivY = y;
    }

    public void Horizontal(float x)
    {
        DivX = x;
    }
}
