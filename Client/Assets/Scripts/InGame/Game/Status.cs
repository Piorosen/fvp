using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Status : MonoBehaviour
{
    public UnityEngine.UI.Image Statuses;
    
    public void EventHandler()
    {
        Statuses.enabled = !Statuses.enabled;
    }
    


}
