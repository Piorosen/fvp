using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class Lobby : MonoBehaviour {

    public Text PlayerName;
    public Text Join;
    public Text RoomName;
    public Text Exit;

    /// <summary>
    /// 임시적으로 로비에서 네트워크 매니저를 초기화를 진행함.
    /// 만약 Lobby 클래스를 통하지 않으면은 자동으로 offline으로 처리가 됨
    /// </summary>
    private void Awake()
    {
        if (NetworkManager.Instance == null)
        {
            NetworkManager.Instance = new NetworkManager();
        }
        // NetworkManager.Instance = NetworkManager.Instance ?? new NetworkManager();
    }

    /// <summary>
    /// 방의 목록을 가져옵니다. 이때 로그인이 되어 있지않으면은 null을 리턴합니다.
    /// </summary>
    /// <returns>방의 목록을 가져옵니다.</returns>
    async Task<Packet.GetRoomListAck> GetRoomList()
    {
        return await NetworkManager.Instance.GetRoomList();
    }

    /// <summary>
    /// 로그인 처리 영역입니다. 지금 현재 ID를 통하여 로그인만 진행이 되며 리턴값을 NetworkId, 고유 번호가 됩니다.
    /// 이 부분은 자동으로 NetworkManager.ClientNetworkId에 값이 됩니다.
    /// </summary>
    public async void Login(){
        // 비동기 작업으로 로그인을 처리를 합니다.
        // 유니티에서 함수를 호출 할때 Login()으로 호출 하므로 비동기 작업이 됩니다.
        var data = await NetworkManager.Instance.Login(PlayerName.text);

        // LoginAck의 값이 null일 경우 로그인 실패합니다.
        // null이 아닌 경우에는 로그인이 성공한 경우 이므로 설정합니.
        if (data != null)
        {
            NetworkManager.ClientNetworkId = data.NetworkId;
            NetworkManager.ClientName = data.Name;

            Debug.Log($"{data.Name} {data.NetworkId}");
            PlayerPrefs.SetString("Name", data.Name);
            PlayerPrefs.SetString("NetworkId", data.NetworkId.ToString());
        }else
        {
            Debug.Log("로그인 실패, 서버 꺼져있음");
        }
    }

    public async void CreateRoom()
    {
        // 방이름을 출력을 합니다.
        Debug.Log(RoomName.text);
        // Net
        var data = await NetworkManager.Instance.MakeRoom(RoomName.text, 8);
        Debug.Log($"{data.Room.MasterUserNetworkId} {data.Room.Id} {data.Room.Name} {data.Room.RoomUsers.Count}");
        PlayerPrefs.SetString("UserList", data.Room.ToString());

        SceneManager.LoadScene("InGame");
    }

    public async void ExitRoom()
    {
        var getlist = await NetworkManager.Instance.GetRoomList();
        foreach (var t in getlist.Rooms)
        {
            Debug.Log(t.Id + " " + t.Name + " ");
        }

    }

    public async void JoinGame()
    {
        if (NetworkManager.ClientNetworkId == null)
        {
            return;
        }
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
        var e = await NetworkManager.Instance.EnterRoom(RoomId);
        PlayerPrefs.SetString("UserList", e.Room.ToString());
        SceneManager.LoadScene("InGame", LoadSceneMode.Single);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
