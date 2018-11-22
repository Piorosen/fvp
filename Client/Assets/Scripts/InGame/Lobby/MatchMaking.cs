using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchMaking : MonoBehaviour {
    NetworkManager NetworkManage;

    public Text PlayerName;
    public Text Join;
    public Text RoomName;

    private void Awake()
    {
        NetworkManage = NetworkManager.Instance ?? new NetworkManager();
    }


    Packet.GetRoomListAck GetRoomList(){
        return NetworkManage.GetRoomList();
    }
    public void Login(){
        Debug.Log("로그인 시도");
        var data = NetworkManage.Login(PlayerName.text);
        Debug.Log("로그인 시도 종료");
        if (data != null)
        {
            PlayerPrefs.SetString("Name", data.Name);
            PlayerPrefs.SetString("NetworkId", data.NetworkId.ToString());
        }else{
            Debug.Log("로그인 실패, 서버 꺼져있음");
        }
    }

    public void CreateRoom(){
        var data = NetworkManage.MakeRoom(RoomName.text, 8);
        
    }

    public void JoinGame(){
        var t = GetRoomList();
        foreach (var data in t.Rooms){
            Debug.Log(data.Id + " " + data.MaxUserCount + " " + data.RoomUsers.Count);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
