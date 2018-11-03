using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class PlayerPool : UnityEngine.MonoBehaviour
{
    public static PlayerPool SinglePlayerPool = null;
    public List<GameObject> Prefab = new List<GameObject>(1);
    public List<Vector2> SpawnLocation = new List<Vector2>(8);

    private List<GameObject> Pool = new List<GameObject>(8);

    void Awake(){
        if (SinglePlayerPool == null){
            SinglePlayerPool = new PlayerPool();
            for (int i = 0; i < Pool.Count; i++){
                Pool[i] = null;
            }
            SinglePlayerPool.AddPlayer(Prefab[0], SpawnLocation[0]);
        }
    }

	// Use this for initialization
	void Start () {

    }


    bool AddPlayer(GameObject @object, Vector2 Location){
        for (int i = 0; i < Pool.Count; i++)
            if (Pool[i] == null){
                Pool[i] = Instantiate(@object, Location, Quaternion.identity);
                return true;
            }
        return false;
    }

    void DelPlayer(GameObject @object){
        Pool.Remove(Pool.Where((item) =>
                               item.GetComponent<Character>().UID == @object.GetComponent<Character>().UID)
                    .FirstOrDefault());
    }

    void DelPlayer(int UID){
        Pool.Remove(Pool.Where((item) =>
                               item.GetComponent<Character>().UID == UID)
                    .First());
    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
