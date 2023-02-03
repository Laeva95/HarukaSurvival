using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    public BossClearSlot m_Boss;
    public LevelUpSlot m_LevelUp;
    public bool m_IsLevelUp;
    public Text m_Text;

    // Ȯ�� ��ư Ŭ�� �� ���� �Լ�
    public void SelectBtn()
    {
        // ���°� ����������, ����Ŭ�������� Ȯ��
        if (m_IsLevelUp)
        {
            // ������ ���� ���� �Լ� ����
            m_LevelUp.SelectSlot();
            m_LevelUp.m_Player.SetIsLevelUp(false);
        }
        else
        {
            // ����Ŭ���� ���� ���� �Լ� ����
            m_Boss.SelectSlot();
            GameManager.Instance.SetBossClear(false);
        }
        GameManager.Instance.CloseStatus();
        // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }

    // ��� ��ư Ŭ�� �� ���� �Լ�
    public void CancelBtn()
    {
        // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }
}
