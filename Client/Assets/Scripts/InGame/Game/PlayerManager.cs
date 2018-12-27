using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// 플레이어의 네트워크 이동, 클라이언트의 이동, 플레이어의 스킬, 플레이어와 관련된 모든 처리를 진행합니다.
/// 그 외 캐릭터의 HP, MP 와 같은 UI 역시 건들입니다.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    List<BaseCharacter> Pool;
    public List<GameObject> Prefab;
    public List<Vector2> SpawnLocation;
    public UIManager UserInterface;
    public Queue<Vector4> MovementQueue = new Queue<Vector4>();

    public void OnChangePing(int ping)
    {
        UserInterface.ChangePing(ping);
    }


    void Awake()
    {
        Pool = new List<BaseCharacter>();
        for (int i = 0; i < 8; i++)
        {
            Pool.Add(null);
        }
    }

    public void Initialize()
    {
        UserInterface.PlayerName.text = NetworkManager.ClientName;
       
        Pool[ClientPlayerIndex].ChangeHP += UserInterface.ChangeHP;
        Pool[ClientPlayerIndex].ChangeMP += UserInterface.ChangeMP;
        Pool[ClientPlayerIndex].OnUpdateUiInfo();
    }

    #region Property
    /// <summary>
    /// 클라이언트의 플레이어 정보를 가져옵니다.
    /// </summary>
    public BaseCharacter ClientPlayer
    {
        get
        {
            return Pool.First((item) => item.NetworkId == NetworkManager.ClientNetworkId);
        }
    }
    /// <summary>
    /// 클라이언트 플레이어의 Pool의 인덱스를 가져옵니다.
    /// </summary>
    public int ClientPlayerIndex
    {
        get
        {
            return Pool.FindIndex((item) => item.NetworkId == NetworkManager.ClientNetworkId);
        }
    }
    /// <summary>
    /// 특정 네트워크 아이디를 가진 플레이어를 리턴합니다.
    /// </summary>
    /// <param name="NetworkId">네트워크 아이디를 입력받습니다.</param>
    /// <returns></returns>
    public BaseCharacter FindPlayer(long? NetworkId)
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
    /// <summary>
    /// 현재 진행중인 플레이어의 수를 리턴 합니다.
    /// </summary>
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
    #endregion

    /// <summary>
    /// 서버의 움직임을 처리합니다.
    /// </summary>
    public void ServerMove()
    {
        while (MovementQueue.Count > 0)
        {
            var data = MovementQueue.Dequeue();
            if (data.w != NetworkManager.ClientNetworkId)
            {
                var character = FindPlayer(Convert.ToInt64(data.w));
                character.ServerData(data);
            }

        }
        for (int i = 0; i < Pool.Count; i++)
        {
            if (Pool[i] != null)
            {
                Pool[i].Movement();
            }
        }
    }

    public void CastSkill(ActiveSkill Skill)
    {
        Debug.Log("스킬 사용 함");
    }


    // 클라이언트에서 물리적인 동작이 있으므로 Fixed에 처리합니다.
    private void FixedUpdate()
    {
        ServerMove();
    }

    public long? AddPlayer(int Object, Vector2 Location, string PlayerName, long NetworkId)
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (Pool[i] == null)
            {
                Vector3 location = Location;
                location.z = -1;

                Pool[i] = Instantiate(Prefab[Object], location, Quaternion.identity).GetComponent<BaseCharacter>();
                Pool[i].Initialize(NetworkId, PlayerName);
                return Pool[i].NetworkId;
            }
        }
        return null;
    }

    public long? AddPlayer(int Object, int Location, string PlayerName, long? NetworkId)
    {
        Vector2 locations = SpawnLocation[Location];
        return AddPlayer(Object, locations, PlayerName, NetworkId.Value);
    }

    
    public bool DelPlayer(BaseCharacter @object)
    {
        int index = Pool.IndexOf(Pool.First((item) =>
                                  item.NetworkId == @object.NetworkId));
        Destroy(Pool[index]);
        return (Pool[index] = null);
    }
    public bool DelPlayer(long? UID)
    {
        //var player = FindPlayer(UID);
        //Destroy(player);
        return (true);
    }

}
