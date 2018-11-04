using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerPool : UnityEngine.MonoBehaviour
{
    public static PlayerPool Instance = null;

    public List<GameObject> Prefab = new List<GameObject>(1);
    public List<Vector2> SpawnLocation = new List<Vector2>(8);
    private List<GameObject> Pool = new List<GameObject>(8);
    public GameObject Camera;

    NetClient Client;

    int PlayerUID = -1;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;

            Client = new NetClient(Global.Network.IPAdress, Global.Network.Port);
            for (int i = 0; i < Pool.Capacity; i++)
            {
                Pool.Add(null);
            }
            int? UID = AddPlayer(Prefab[0], SpawnLocation[0]);
            UID = AddPlayer(Prefab[0], SpawnLocation[0]);
            if (UID != null)
            {
                PlayerUID = UID.Value;
            }
            else
            {
                Debug.Log("PlayerPool : Start : UID : null");
            }

            try
            {
                Client.Connect();
                
                Packet.LoginReq login = new Packet.LoginReq
                {
                    NetworkId = UID.Value,
                    Name = System.IO.Path.GetRandomFileName()
                };

                Client.Send(Packet.Type.LoginReq, login);
                Client.Update();
                PacketInfo info = new PacketInfo();
                if (Client.TryGetPacket(out info))
                {
                    Debug.Log("Login Successs  " + login.Name + " : " + login.NetworkId);
                }
                else
                {
                    Debug.Log("Login Fail");
                }
                
            }
            catch (Exception)
            {
                Debug.Log("Character : Start : Server Error! : " + UID.Value);
                Client.Close();
                Client = null;
            }

            
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        StartCoroutine("ServerRequest");
    }

    /// <summary>
    /// 이것을 이제 개인 클라이언트에게 값을 던져줘야함.
    /// </summary>
    /// 

    IEnumerator ServerRequest()
    {
        Debug.Log("코루틴 시작");
        if (Client != null)
        {
            Debug.Log("널아님");
            while (true)
            {
                Vector3 location = new Vector3();

                location = Pool
                    .First((item) => item.GetComponent<Character>().UID == PlayerUID)
                    .GetComponent<Character>().transform.position;

                Packet.MoveReq move = new Packet.MoveReq
                {
                    Position = new Packet.Vector3()
                    {
                        X = location.x,
                        Y = location.y,
                        Z = location.z
                    }
                };

                Client.Send(Packet.Type.MoveReq, move);
                Client.Update();

                PacketInfo info = new PacketInfo();
                if (Client.TryGetPacket(out info))
                {
                    
                    if (info.Type == Packet.Type.MoveAck)
                    {
                        Packet.MoveAck moveAck = Packet.MoveAck.Parser.ParseFrom(info.Payload);
                        Debug.Log("Network : " + moveAck.NetworkId + " Pos : " + moveAck.Position.ToString());
                    }
                    
                }

                yield return new WaitForSeconds(0.1f);

            }
        }
        Debug.Log("코루틴 종료");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        var User = Pool.First(item => item.GetComponent<Character>().UID == PlayerUID);
        User.GetComponent<Character>().Movement(new Vector4(x, y));

        Vector3 AvgLocation = new Vector3();
        int UserCount = 0;

        Vector2 Max = new Vector2(float.MinValue, float.MinValue);
        Vector2 Min = new Vector2(float.MaxValue, float.MaxValue);

        for (int i = 0; i < Pool.Count; i++){
            if (Pool[i] != null)
            {
                AvgLocation += Pool[i].transform.position;

                if (Max.x < Pool[i].transform.position.x){
                    Max.x = Pool[i].transform.position.x;
                }
                if (Max.y < Pool[i].transform.position.y){
                    Max.y = Pool[i].transform.position.y;
                }
                if (Min.x > Pool[i].transform.position.x){
                    Min.x = Pool[i].transform.position.x;
                }
                if (Min.y > Pool[i].transform.position.y){
                    Min.y = Pool[i].transform.position.y;
                }

                UserCount++;
            }
           // Debug.Log(AvgLocation / UserCount);
        }
        var Character = Camera.GetComponent<InGameCamera>();

        Character.Target = AvgLocation / UserCount;
        Character.NeedSize = new Vector2(Mathf.Abs(Max.x - Min.x), Mathf.Abs(Max.y - Min.y));
    }


    int? AddPlayer(GameObject @object, Vector2 Location){
        for (int i = 0; i < Pool.Count; i++)
            if (Pool[i] == null){
                Pool[i] = Instantiate(@object, Location, Quaternion.identity);
                return Pool[i].GetComponent<Character>().UID;
            }
        return null;
    }

    bool DelPlayer(GameObject @object){
        return Pool.Remove(Pool.First((item) =>
                               item.GetComponent<Character>().UID == @object.GetComponent<Character>().UID));
    }

    bool DelPlayer(int UID)
    {
        return Pool.Remove(Pool.First((item) =>
                               item.GetComponent<Character>().UID == UID));
    }



    // Update is called once per frame
    void Update()
    {
		
	}
}
