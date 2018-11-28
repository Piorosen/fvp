using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchMaking : MonoBehaviour {
    NetworkManager NetworkManage;

    public Text PlayerName;
    public Text Join;
    public Text RoomName;
    public Text Exit;

    private void Awake()
    {
        NetworkManage = NetworkManager.Instance ?? new NetworkManager();
    }


    Packet.GetRoomListAck GetRoomList(){
        return NetworkManage.GetRoomList();
    }
    public void Login(){
        var data = NetworkManage.Login(PlayerName.text);
        if (data != null)
        {
            NetworkManager.ClientNetworkId = data.NetworkId;
            Debug.Log($"{data.Name} {data.NetworkId}");
            PlayerPrefs.SetString("Name", data.Name);
            PlayerPrefs.SetString("NetworkId", data.NetworkId.ToString());
        }else{
            Debug.Log("로그인 실패, 서버 꺼져있음");
        }
    }

    public void CreateRoom(){
        Debug.Log(RoomName.text);
        var data = NetworkManage.MakeRoom(RoomName.text, 8);
        Debug.Log($"{data.Room.MasterUserNetworkId} {data.Room.Id} {data.Room.Name} {data.Room.RoomUsers.Count}");
        PlayerPrefs.SetString("UserList", data.Room.ToString());

        SceneManager.LoadScene("InGame");
    }

    public void ExitRoom()
    {
        Debug.Log("Exit Room");
        NetworkManage.ExitRoom();

    }

    public void JoinGame(){
        var t = GetRoomList();
        foreach (var data in t.Rooms){
            Debug.Log(data.Id + " " + data.MaxUserCount + " " + data.RoomUsers.Count +" " + data.Name);
        }

        var e = NetworkManage.EnterRoom(long.Parse(Join.text));
        PlayerPrefs.SetString("UserList", e.Room.ToString());

        SceneManager.LoadScene("InGame");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
