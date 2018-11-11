using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public Slider HealthPoint;
    public Slider ManaPoint;
    public Text PlayerName;

    // 체력바, 기력바의 변화량 속도
    public float Speed;

	// Use this for initialization
	void Start () {
        
	}

    float NowHP = 1;
    float MaxHP = 1;

    float NowMP = 1;
    float MaxMP = 1;

    // 값을 1 의 기준으로 변경을 한다.
    float ConvertPercent(float a, float percent)
    {
        return a * percent;
    }

    // Percent, 0~ 100 기준을 0 ~ 1의 기준으로 변경을 합니다.
    float ConvertPercentToNumber(float Now, float Max)
    {
        return Now / Max;
    }

    // 위의 값을 현재값, 최대값을 기준으로 0 ~ 1로 바꿔 줍니다.
    float Convert(float Now, float Max, float Percent = 1)
    {
        return ConvertPercentToNumber(ConvertPercent(Now, Percent), Max);
    }

    // 실시간으로 처리합니다.
    void Update()
    {
        // 값이 0.99 <= value 일 경우 value의 값을 1 로 바꿔줍니다.
        if (Convert(NowHP, MaxHP, 0.99f) <= HealthPoint.value
            && HealthPoint.value <= Convert(NowHP, MaxHP, 1.01f))
        {
            HealthPoint.value = Convert(NowHP, MaxHP);
        }
        else
        {
            HealthPoint.value = Vector2.Lerp(new Vector2(HealthPoint.value, 0)
                                           , new Vector2(Convert(NowHP, MaxHP), 0)
                                           , Speed * Time.deltaTime).x;
        }

        // 값이 0.99 <= value 일 경우 value의 값을 1 로 바꿔줍니다.
        if (Convert(NowMP, MaxMP, 0.99f) <= ManaPoint.value
            && ManaPoint.value <= Convert(NowMP, MaxMP, 1.01f))
        {
            ManaPoint.value = Convert(NowMP, MaxMP);
        }
        else
        {
            ManaPoint.value = Vector2.Lerp(new Vector2(ManaPoint.value, 0)
                                           , new Vector2(Convert(NowMP, MaxMP), 0)
                                           , Speed * Time.deltaTime).x;
        }

    }

    // Character의 이벤트를 받은 PlayerManager에서 연결된 함수입니다.
    public void ChangeHP(float now, float maxValue){
        NowHP = now;
        MaxHP = maxValue;
    }
    public void ChangeMP(float now, float maxValue){
        
        NowMP = now;
        MaxMP = maxValue;
    }
}
