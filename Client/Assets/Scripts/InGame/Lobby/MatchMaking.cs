using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMaking : MonoBehaviour {
    NetworkManager NetworkManage = new NetworkManager();


    void GetRoomList(){
        var list = NetworkManage.GetRoomList();


    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
