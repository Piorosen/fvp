using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

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


    async Task<Packet.GetRoomListAck> GetRoomList()
    {
        return await NetworkManage.GetRoomList();
    }


    public async void Login(){
        var data = await NetworkManage.Login(PlayerName.text);
        if (data != null)
        {
            NetworkManager.ClientNetworkId = data.NetworkId;
            NetworkManager.ClientName = data.Name;

            Debug.Log($"{data.Name} {data.NetworkId}");
            PlayerPrefs.SetString("Name", data.Name);
            PlayerPrefs.SetString("NetworkId", data.NetworkId.ToString());
        }else{
            Debug.Log("로그인 실패, 서버 꺼져있음");
        }
    }

    public async void CreateRoom(){
        Debug.Log(RoomName.text);
        var data = await NetworkManage.MakeRoom(RoomName.text, 8);
        Debug.Log($"{data.Room.MasterUserNetworkId} {data.Room.Id} {data.Room.Name} {data.Room.RoomUsers.Count}");
        PlayerPrefs.SetString("UserList", data.Room.ToString());

        SceneManager.LoadScene("InGame");
    }

    public async void ExitRoom()
    {
        var getlist = await NetworkManage.GetRoomList();
        foreach (var t in getlist.Rooms)
        {
            Debug.Log(t.Id + " " + t.Name + " ");
        }

    }

    public async void JoinGame() {
        try
        {
            var t = await GetRoomList();
            foreach (var data in t.Rooms)
            {
                Debug.Log(data.Id + " " + data.MaxUserCount + " " + data.RoomUsers.Count + " " + data.Name);
            }

            long RoomId = long.Parse(Join.text);
            Debug.Log(RoomId);
            /*
             * 
             *  서버쪽 값이 넘어오지 않아서 처리가 안됨.
             * 
             */
            var e = await NetworkManage.EnterRoom(RoomId);    
            PlayerPrefs.SetString("UserList", e.Room.ToString());
        }
        catch (Exception)
        {

        }
        finally
        {
            SceneManager.LoadScene("InGame", LoadSceneMode.Single);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
