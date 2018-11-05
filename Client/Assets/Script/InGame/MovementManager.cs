using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MovementManager : MonoBehaviour
{
    List<GameObject> Pool = new List<GameObject>(8);

    public List<GameObject> Prefab;
    public List<Vector2> SpawnLocation;

    public int? PlayerUID = null;
     
    void Awake()
    {
        for (int i = 0; i < Pool.Capacity; i++)
        {
            Pool.Add(null);
        }

        int? UID = AddPlayer(Prefab[0], SpawnLocation[0]);
        UID = AddPlayer(Prefab[0], SpawnLocation[0]);
        UID = AddPlayer(Prefab[0], SpawnLocation[0]);
        UID = AddPlayer(Prefab[0], SpawnLocation[0]);
        UID = AddPlayer(Prefab[0], SpawnLocation[0]);



        if (UID != null)
        {
            PlayerUID = UID.Value;
        }
        else
        {
            Debug.Log("PlayerPool : Start : UID : null");
        }
    }

    public GameObject ClientPlayer {
        get {
            return Pool.First((item) => item.GetComponent<Character>().UID == PlayerUID);
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

    public Vector2 LocationMax
    {
        get 
        {
            Vector2 Result = new Vector2(float.MinValue, float.MinValue);

            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i] != null)
                {
                    if (Result.x < Pool[i].transform.position.x)
                    {
                        Result.x = Pool[i].transform.position.x;
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

    public Vector2 LocationMin
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

        Vector4 data = new Vector4(x,y);
        ClientPlayer.GetComponent<Character>().Movement(data);
    }

    public void ServerMove(){

    }


    int? AddPlayer(GameObject @object, Vector2 Location)
    {
        for (int i = 0; i < Pool.Count; i++)
            if (Pool[i] == null)
            {
                Pool[i] = Instantiate(@object, Location, Quaternion.identity);
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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
