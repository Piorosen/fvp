using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoadAttribute]
public class EditorQuit : MonoBehaviour
{
    static bool WantsToQuit()
    {
        Debug.Log("Player  quitting. NetworkManager.Instance.Close()");
        NetworkManager.Instance.Close();
        return true;
    }

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.wantsToQuit += WantsToQuit;
    }
}
