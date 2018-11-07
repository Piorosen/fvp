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
        HealthPoint.GetComponent<Slider>().value = now / maxValue;
    }
    public void ChangeMP(float now, float maxValue){
        ManaPoint.GetComponent<Slider>().value = now / maxValue;
    }
}
