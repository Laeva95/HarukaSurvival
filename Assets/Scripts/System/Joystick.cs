using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform m_Lever;
    private RectTransform m_RectTransform;
    [SerializeField, Range(10f, 100f)]
    private float m_LeverRange;
    [SerializeField]
    private Haruka m_player;

    private Vector2 m_InputVec;
    private bool m_IsInput;

    void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        // 현재 드래그 상태가 아니라면 플레이어의 방향을 초기화함
        if (!m_IsInput)
        {
            m_player.SetDirection2(Vector3.zero);
        }
    }
    // 드래그 시작
    public void OnPointerDown(PointerEventData eventData)
    {
        // 레버 포지션 입력, 플레이어 방향 벡터 입력
        ControlJoystick(eventData);
        m_player.SetDirection2(m_InputVec);
        // 입력 플래그 true
        m_IsInput = true;
    }

    // 드래그 도중
    public void OnDrag(PointerEventData eventData)
    {
        // 레버 포지션 입력, 플레이어 방향 벡터 입력
        ControlJoystick(eventData);
        m_player.SetDirection2(m_InputVec);
    }

    // 드래그 끝
    public void OnPointerUp(PointerEventData eventData)
    {
        // 레버 포지션 초기화, 플레이어 방향 벡터 초기화
        m_Lever.anchoredPosition = Vector2.zero;
        m_player.SetDirection2(Vector3.zero);

        // 입력 플래그 false
        m_IsInput = false;
    }

    // 조이스틱 이동 계산
    private void ControlJoystick(PointerEventData eventData)
    {
        Vector3 _dir = eventData.position - m_RectTransform.anchoredPosition;
        Vector3 _dir2 = _dir.magnitude < m_LeverRange ? _dir : _dir.normalized * m_LeverRange;
        m_Lever.anchoredPosition = _dir2;
        m_InputVec = _dir2 / m_LeverRange;
    }
}
