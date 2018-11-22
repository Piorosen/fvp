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

    Image[] image = new Image[4];

    void Start()
    {
        BgImage = GetComponent<Image>();
        for (int i = 0; i < 4; i++){
            image[i] = transform.GetChild(i).GetComponent<Image>();
        }
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

            if (InputVector.y >= 0.5)
            {
                image[0].color = new Color(130.0f/255.0f, 0, 0);
                image[3].color = new Color(1, 1, 1);
            }if (InputVector.x > 0)
            {
                image[1].color = new Color(1, 1, 1);
                image[2].color = new Color(130.0f / 255.0f, 0, 0);
            }if (InputVector.x < 0)
            {
                image[1].color = new Color(130.0f / 255.0f, 0, 0);
                image[2].color = new Color(1, 1, 1);
            }if (InputVector.y <= -0.5)
            {
                image[0].color = new Color(1, 1, 1);
                image[3].color = new Color(130.0f / 255.0f, 0, 0);
            }

            if (-0.5 <= InputVector.y && InputVector.y < 0.5){
                image[0].color = new Color(1, 1, 1);
                image[3].color = new Color(1, 1, 1);
            }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image[0].color = new Color(1, 1, 1);
        image[1].color = new Color(1, 1, 1);
        image[2].color = new Color(1, 1, 1);
        image[3].color = new Color(1, 1, 1);
        InputVector = Vector3.zero;
    }
}
