using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCamera : MonoBehaviour {

    public Vector3 Target;
    public float Speed = 1;
    public Vector2 MaxSize;
    public Vector2 MinSize;

    public Vector2 NeedSize;

	// Use this for initialization
	void Start () {
        Target = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
        Target.z = transform.position.z;

        if (transform.position != Target){
            transform.position = Vector3.Lerp(this.transform.position, Target, Speed * Time.deltaTime);
        }

        if ( )

	}
}
