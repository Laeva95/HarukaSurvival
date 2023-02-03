using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum STATUS
{
    HARUKA,
    GUN,
    MUTSUKI,
    ARU,
    KAYOKO,
    SHIELD,
    HINA,

}
public class StatusUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private STATUS m_Status;
    private RectTransform m_Rect;
    private void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.ClickStatusPanel(m_Status, m_Rect);
    }
}
