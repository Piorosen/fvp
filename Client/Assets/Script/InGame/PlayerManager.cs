using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    List<Character> Pool = new List<Character>(8);
    public List<GameObject> Prefab;
    public List<Vector2> SpawnLocation;
    public UIManager UserInterface;

    public int? PlayerUID;
    public float IsDebug;


    void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            Pool.Add(null);
        }
        string PlayerName = PlayerPrefs.GetString("PlayerName");
        int? UID = AddPlayer(Prefab[0], SpawnLocation[0], PlayerName);
        Debug.Log(UID);
        if (UID != null)
        {
            PlayerUID = UID.Value;

            Pool[ClientPlayerIndex].ChangeHP += (float now, float max) => UserInterface.ChangeHP(now, max);
            Pool[ClientPlayerIndex].ChangeMP += (float now, float max) => UserInterface.ChangeMP(now, max);
        }
        else
        {
            Debug.Log("PlayerPool : Start : UID : null");
        }
    }

    /// <summary>
    /// 클라이언트의 플레이어 정보를 가져옵니다.
    /// </summary>
    public Character ClientPlayer
    {
        get
        {

            return Pool.First((item) => item.UID == PlayerUID);
        }
    }
    /// <summary>
    /// 클라이언트 플레이어의 Pool의 인덱스를 가져옵니다.
    /// </summary>
    public int ClientPlayerIndex
    {
        get
        {
            int index = Pool.FindIndex((item) => item.UID == PlayerUID);

            return index;
        }
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
        Pool[ClientPlayerIndex].Movement(data);
    }

    /// <summary>
    /// 서버의 움직임을 처리합니다.
    /// </summary>
    public void ServerMove()
    {

    }

    // 클라이언트에서 물리적인 동작이 있으므로 Fixed에 처리합니다.
    private void FixedUpdate()
    {
        ServerMove();
        ClientMove();
    }

    int? AddPlayer(GameObject @object, Vector2 Location, string PlayerName)
    {
        for (int i = 0; i < Pool.Count; i++)
            if (Pool[i] == null)
            {
                Pool[i] = Instantiate(@object, new Vector3(Location.x, Location.y, -1), Quaternion.identity).GetComponent<Character>();
                Pool[i].PlayerName = PlayerName;
                return Pool[i].UID;
            }
        return null;
    }

    bool DelPlayer(Character @object)
    {
        int index = Pool.IndexOf(Pool.First((item) =>
                                  item.UID == @object.UID));
        return (Pool[index] = null);
    }

    bool DelPlayer(int UID)
    {
        int index = Pool.IndexOf(Pool.First((item) =>
                                  item.UID == UID));
        return (Pool[index] = null);
    }

}
