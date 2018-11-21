using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // 어디서나 읽을수 있는 정적인 데이터입니다.
    public static Vector3 InputVector;
    
    // 방향키의 이미지
    Image BgImage;
    // 조이스틱 가운데 흰색 버튼입니다.
    Image JoyStickImg;

    void Start()
    {
        BgImage = GetComponent<Image>();
        JoyStickImg = transform.GetChild(0).GetComponent<Image>();
        Debug.Log("123");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BgImage.rectTransform
                                                                    , eventData.position
                                                                    , eventData.pressEventCamera
                                                                    , out pos))
        {
            pos.x = (pos.x / BgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / BgImage.rectTransform.sizeDelta.y);

            InputVector = new Vector3(pos.x * 2 + 1, pos.y * 2 - 1, 0);

            InputVector = (InputVector.magnitude > 1.0f)
                            ? InputVector.normalized
                            : InputVector;

            JoyStickImg.rectTransform.anchoredPosition = new Vector3(
                InputVector.x * BgImage.rectTransform.sizeDelta.x / 2
                , InputVector.y * BgImage.rectTransform.sizeDelta.y / 2
                , 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector3.zero;
        JoyStickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
}
