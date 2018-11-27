using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCamera : MonoBehaviour {

    // 모든 플레이어의 중심점.
    public Vector3 Target;

    // 카메라가 움직이는데 걸리는 시간. 이동, 카메라 크기 변환에 따른 속도.
    public float Speed;
    // 카메라의 최대 크기
    public float MaxSize;
    // 카메라의 최소 크기.
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
	void Start ()
    {
        Target = new Vector3();
        CameraInfo = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Target.z = transform.position.z;

        // 현재 카메라의 위치와 캐릭터 2명 이상의 위치의 중심점과 비교를 하고 이동을 합니다.
        if (transform.position != Target){
            transform.position = Vector3.Lerp(this.transform.position, Target, Speed * Time.deltaTime);
        }
        // 카메라 크기를 변화 합니다.
        // 플레이어가 2명일 경우 카메라가 동적으로 시야각이 넓어집니다.
        CameraInfo.orthographicSize = Lerp();
	}
}
