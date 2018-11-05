using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCamera : MonoBehaviour {

    public Vector3 Target;
    public float Speed = 1;
    public float MaxSize;
    public float MinSize;

    public Vector2 NeedSize;

    Camera CameraInfo;

    float Lerp(){
        float max = NeedSize.x > NeedSize.y ? NeedSize.x : NeedSize.y;

        return Mathf.Lerp(CameraInfo.orthographicSize, 
                          Mathf.Clamp(max, MinSize, MaxSize), 
                          Speed * Time.deltaTime);
    }

	// Use this for initialization
	void Start () {
        Target = new Vector3();
        CameraInfo = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Target.z = transform.position.z;

        if (transform.position != Target){
            transform.position = Vector3.Lerp(this.transform.position, Target, Speed * Time.deltaTime);
        }
        CameraInfo.orthographicSize = Lerp();
	}
}
