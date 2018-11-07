using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    List<GameObject> Pool = new List<GameObject>(8);
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

        int? UID = AddPlayer(Prefab[0], SpawnLocation[0]);
        Debug.Log(UID);
        if (UID != null)
        {
            PlayerUID = UID.Value;

            Pool[ClientPlayerIndex].GetComponent<Character>().ChangeHP += (float now, float max) => UserInterface.ChangeHP(now, max);
            Pool[ClientPlayerIndex].GetComponent<Character>().ChangeMP += (float now, float max) => UserInterface.ChangeMP(now, max);
        }
        else
        {
            Debug.Log("PlayerPool : Start : UID : null");
        }
    }

    public GameObject ClientPlayer
    {
        get
        {
            return Pool.First((item) => item.GetComponent<Character>().UID == PlayerUID);
        }
    }
    public int ClientPlayerIndex
    {
        get
        {
            int index = Pool.FindIndex((item) => item.GetComponent<Character>().UID == PlayerUID);

            return index;
        }
    }
    public int PoolCount
    {
        get
        {
            return Pool.Count;
        }
    }
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
                    {
                    }
                    if (Result.y < Pool[i].transform.position.y)
                    {
                        Result.y = Pool[i].transform.position.y;
                    }
                }
            }
            return Result;
        }
    }
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
                    {
                        Result.x = Pool[i].transform.position.x;
                    }
                    if (Result.y > Pool[i].transform.position.y)
                    {
                        Result.y = Pool[i].transform.position.y;
                    }
                }
            }
            return Result;
        }
    }
    public Vector2 LocationCamera
    {
        get
        {
            var x = LocationMax - LocationMin;
            return new Vector2(Mathf.Abs(x.x), Mathf.Abs(x.y));
        }
    }

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
        Pool[ClientPlayerIndex].GetComponent<Character>().Movement(data);
    }

    public void ServerMove(){

    }

    private void FixedUpdate()
    {
        ClientMove();
    }

    int? AddPlayer(GameObject @object, Vector2 Location)
    {
        for (int i = 0; i < Pool.Count; i++)
            if (Pool[i] == null)
            {
                Pool[i] = Instantiate(@object, new Vector3(Location.x, Location.y, -1), Quaternion.identity);
                return Pool[i].GetComponent<Character>().UID;
            }
        return null;
    }

    bool DelPlayer(GameObject @object)
    {
        return Pool.Remove(Pool.First((item) =>
                               item.GetComponent<Character>().UID == @object.GetComponent<Character>().UID));
    }

    bool DelPlayer(int UID)
    {
        return Pool.Remove(Pool.First((item) =>
                               item.GetComponent<Character>().UID == UID));
    }

}
