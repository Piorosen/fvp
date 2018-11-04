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
                Packet.Login login = new Packet.Login();
                login.NetworkId = UID.Value;
                login.Name = System.IO.Path.GetRandomFileName();
                Client.Send(Packet.Type.Login, login);
                Debug.Log("Login Successs  " + login.Name + " : " + login.NetworkId);
            }
            catch (Exception)
            {
                Debug.Log("Character : Start : Client Error! : " + UID.Value);
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
        if (Client != null)
        {
            while (true)
            {
                Packet.Move move = new Packet.Move();
                
                move.Pos = new Packet.Vector3()
                {
                    X = transform.position.x,
                    Y = transform.position.y,
                    Z = transform.position.z
                };

                Client.Send(Packet.Type.Move, move);
                PacketInfo info = new PacketInfo();
                if (Client.TryGetPacket(out info))
                {
                    if (info.Type == Packet.Type.MoveAck)
                    {
                        Packet.MoveAck moveAck = Packet.MoveAck.Parser.ParseFrom(info.Payload);
                        Debug.Log("Network : " + moveAck.NetworkId + " Pos : " + moveAck.Pos.ToString());
                    }
                    Debug.Log("Type : " + info.Type);
                }
                
                yield return new WaitForSeconds(0.100f);
            }

        }
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
