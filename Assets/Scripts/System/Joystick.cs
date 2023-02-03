using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ�� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ����

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
        // ���� �巡�� ���°� �ƴ϶�� �÷��̾��� ������ �ʱ�ȭ��
        if (!m_IsInput)
        {
            m_player.SetDirection2(Vector3.zero);
        }
    }
    // �巡�� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        // ���� ������ �Է�, �÷��̾� ���� ���� �Է�
        ControlJoystick(eventData);
        m_player.SetDirection2(m_InputVec);
        // �Է� �÷��� true
        m_IsInput = true;
    }

    // �巡�� ����
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ������ �Է�, �÷��̾� ���� ���� �Է�
        ControlJoystick(eventData);
        m_player.SetDirection2(m_InputVec);
    }

    // �巡�� ��
    public void OnPointerUp(PointerEventData eventData)
    {
        // ���� ������ �ʱ�ȭ, �÷��̾� ���� ���� �ʱ�ȭ
        m_Lever.anchoredPosition = Vector2.zero;
        m_player.SetDirection2(Vector3.zero);

        // �Է� �÷��� false
        m_IsInput = false;
    }

    // ���̽�ƽ �̵� ���
    private void ControlJoystick(PointerEventData eventData)
    {
        Vector3 _dir = eventData.position - m_RectTransform.anchoredPosition;
        Vector3 _dir2 = _dir.magnitude < m_LeverRange ? _dir : _dir.normalized * m_LeverRange;
        m_Lever.anchoredPosition = _dir2;
        m_InputVec = _dir2 / m_LeverRange;
    }
}
