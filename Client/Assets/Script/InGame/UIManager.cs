using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public GameObject HealthPoint;
    public GameObject ManaPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeHP(float now, float maxValue){
        float t = Vector2.Lerp(new Vector2(HealthPoint.GetComponent<Slider>().value, 0)
                             , new Vector2(now / maxValue, 0)
                             , 50 * Time.deltaTime).x;
        HealthPoint.GetComponent<Slider>().value = t;
    }
    public void ChangeMP(float now, float maxValue){
        float t = Vector2.Lerp(new Vector2(ManaPoint.GetComponent<Slider>().value, 0)
                             , new Vector2(now / maxValue, 0)
                             , 50 * Time.deltaTime).x;
        ManaPoint.GetComponent<Slider>().value = now / maxValue;
    }
}
