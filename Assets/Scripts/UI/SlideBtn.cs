using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideBtn : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Haruka m_Player;

    public void OnPointerDown(PointerEventData eventData)
    {
        m_Player.SlideBtn();
    }
}
