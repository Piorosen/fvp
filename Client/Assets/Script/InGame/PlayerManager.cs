using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerManager : MonoBehaviour
{
    List<Character> Pool = new List<Character>(8);
    public List<GameObject> Prefab;
    public List<Vector2> SpawnLocation;
    public UIManager UserInterface;
    public Queue<Vector4> MovementQueue = new Queue<Vector4>();
    public long? ClientNetworkId;
    public string ClientName;
    public float IsDebug;


    void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            Pool.Add(null);
        }

        string PlayerName = PlayerPrefs.GetString("PlayerName");

        // Pool[ClientPlayerIndex].ChangeHP += (float now, float max) => UserInterface.ChangeHP(now, max);
        // Pool[ClientPlayerIndex].ChangeMP += (float now, float max) => UserInterface.ChangeMP(now, max);

    }

    /// <summary>
    /// 클라이언트의 플레이어 정보를 가져옵니다.
    /// </summary>
    public Character ClientPlayer
    {
        get
        {
            return Pool.First((item) => item.NetworkId == ClientNetworkId);
        }
    }
    /// <summary>
    /// 클라이언트 플레이어의 Pool의 인덱스를 가져옵니다.
    /// </summary>
    public int ClientPlayerIndex
    {
        get
        {
            return Pool.FindIndex((item) => item.NetworkId == ClientNetworkId);
        }
    }
    public Character FindPlayer(long? NetworkId)
    {
        return Pool.First((item) => item.NetworkId == NetworkId);
    }

    /// <summary>
    ///  Pool의 갯수를 가져옵니다.
    /// </summary>
    public int PoolCount
    {
        get
        {
            return Pool.Count;
        }
    }

    public int PlayerCount
    {
        get
        {
            int Result = 0;
            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i] != null)
                    Result++;
            }
            return Result;
        }
    }
    /// <summary>
    /// 네트워크 플레이어 포함 평균 위치 자표를 가져옵니다.
    /// </summary>
    public Vector3 LocationAverage
    {
        get
        {
            Vector3 Result = new Vector3();
            int user = 0;
            for (int i = 0; i < Pool.Count; i++)
                if (Pool[i] != null)
                {
                    Result += Pool[i].transform.position;
                    user++;
                }
            return Result / user;
        }
    }
    /// <summary>
    /// 네트워크 플레이어의 x,y좌표 최대값을 가져옵니다.
    /// </summary>
    Vector2 LocationMax
    {
        get 
        {
            Vector2 Result = new Vector2(float.MinValue, float.MinValue);

            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i] != null)
                {
                    if (Result.x < Pool[i].transform.position.x)
                        Result.x = Pool[i].transform.position.x;
                    if (Result.y < Pool[i].transform.position.y)
                        Result.y = Pool[i].transform.position.y;
                }
            }
            return Result;
        }
    }
    /// <summary>
    /// 네트워크 플레이어의 x,y좌표 최소값을 가져옵니다.
    /// </summary>
    Vector2 LocationMin
    {
        get
        {
            Vector2 Result = new Vector2(float.MaxValue, float.MaxValue);
            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i] != null)
                {
                    if (Result.x > Pool[i].transform.position.x)
                        Result.x = Pool[i].transform.position.x;
                    if (Result.y > Pool[i].transform.position.y)
                        Result.y = Pool[i].transform.position.y;
                }
            }
            return Result;
        }
    }
    /// <summary>
    /// 현재 카메라의 위치를 나타냅니다.
    /// </summary>
    public Vector2 LocationCamera
    {
        get
        {
            var x = LocationMax - LocationMin;
            return new Vector2(Mathf.Abs(x.x), Mathf.Abs(x.y));
        }
    }
    
    /// <summary>
    /// 클라이언트의 움직임을 처리합니다.
    /// </summary>
    public void ClientMove(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) == true){
            x = -1;
        }else if (Input.GetKey(KeyCode.D) == true){
            x = 1;
        }
        if (Input.GetKey(KeyCode.S) == true){
            y = -1;
        }else if (Input.GetKey(KeyCode.W) == true){
            y = 1;
        }

        Vector4 data = new Vector4(x, y, IsDebug);
        if (ClientNetworkId != null)
        {
            ClientPlayer.Movement(data);
        }
        
    }

    /// <summary>
    /// 서버의 움직임을 처리합니다.
    /// </summary>
    public IEnumerator ServerMove()
    {
        while (true)
        {
            if (MovementQueue.Count == 0)
            {
                yield return new WaitForSeconds(0.001f);
                continue;
            }

            var data = MovementQueue.Dequeue();
            if (data.w != ClientNetworkId)
            {
                var character = FindPlayer(Convert.ToInt64(data.w));
                character.transform.position = data;
            }
        }
    }
    
    void Start()
    {
        StartCoroutine(ServerMove());
    }


    // 클라이언트에서 물리적인 동작이 있으므로 Fixed에 처리합니다.
    private void FixedUpdate()
    {
        ClientMove();
    }

    public long? AddPlayer(int Object, int Location, string PlayerName, long? NetworkId)
    {
        for (int i = 0; i < Pool.Count; i++)
            if (Pool[i] == null)
            {
                Pool[i] = Instantiate(Prefab[Object], new Vector3(SpawnLocation[Location].x
                                                                , SpawnLocation[Location].y, -1)
                                                                , Quaternion.identity).GetComponent<Character>();
                Pool[i].PlayerName = PlayerName;
                Pool[i].NetworkId = NetworkId;
                if (ClientNetworkId != NetworkId)
                {
                    // StartCoroutine(Pool[i].SM());
                }
                return Pool[i].NetworkId;
            }
        return null;
    }

    bool DelPlayer(Character @object)
    {
        int index = Pool.IndexOf(Pool.First((item) =>
                                  item.NetworkId == @object.NetworkId));
        return (Pool[index] = null);
    }

    bool DelPlayer(long? UID)
    {
        int index = Pool.IndexOf(Pool.First((item) =>
                                  item.NetworkId == UID));
        return (Pool[index] = null);
    }

}
