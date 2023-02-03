using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossClearSlot : MonoBehaviour
{
    public Text m_NameText;
    public Text m_DescriptionText;
    public Image m_Image;
    public int m_SkillIndex;
    public BossClearUI m_Boss;
    [SerializeField]
    private GameObject m_BossClearUI;
    [SerializeField]
    private GameObject m_ConfirmUI;
    [SerializeField]
    private Haruka m_Player;

    private void Awake()
    {
        m_Boss = m_BossClearUI.GetComponent<BossClearUI>();
    }
    // ���� �� ������ �����Ұ��� Ȯ��â�� ���� �Լ�
    public void ConfirmSelect()
    {
        string str = "";
        if (m_SkillIndex == 0 || m_SkillIndex == 2 || m_SkillIndex == 6 || m_SkillIndex == 7)
        {
            str = "��";
        }
        else
        {
            str = "��";
        }
        // ������ ���¸� false�� �Ѱ���
        ConfirmUI confirm = m_ConfirmUI.GetComponent<ConfirmUI>();
        confirm.m_Boss = this;
        confirm.m_IsLevelUp = false;
        confirm.m_Text.text = $"{m_NameText.text}{str} �����Ͻðڽ��ϱ�?";

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
        m_ConfirmUI.SetActive(true);
    }

    public void SelectSlot()
    {
        // �ش� ������ Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
        // �� ������ �ε����� �´� ȿ���� �ߵ���
        // �ߺ����� ȹ���� �� ������ �ش� ��ȣ�� �´� bool �� true
        m_Boss.m_IsNum[m_SkillIndex] = true;

        switch (m_SkillIndex)
        {
            case 0:
                // �ִ� ü�� +100(ü�� ȸ�� +100)
                m_Player.SetMaxHpUp(100);
                break;
            case 1:
                // ���ݷ� +20
                m_Player.GunManager.SetDamage(20);
                break;
            case 2:
                // �� ��ü Slow ���� Ȱ��ȭ
                GameManager.Instance.SetSlow();
                break;
            case 3:
                // ���� +1
                m_Player.GunManager.SetPenetration();
                break;
            case 4:
                // �⺻ ���� ������ 50% ����
                m_Player.GunManager.SetIsDamageUp();
                break;
            case 5:
                // �̵��ӵ�2 ���� +1
                m_Player.SetMoveUp2();
                break;
            case 6:
                // �޴� ������ -30%
                m_Player.SunglassSlowStart();
                break;
            case 7:
                // 10�ʸ��� ü�� ȸ��
                m_Player.HealKitStart();
                break;
            case 8:
                // �߻� ���� +5
                m_Player.GunManager.SetFireRate2(5);
                break;
            default:
                return;
        }
        CloseUI();

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }

    private void CloseUI()
    {
        // timeScale�� 1�� �ǵ����� ���� Ŭ����UI ��Ȱ��ȭ
        m_BossClearUI.SetActive(false);

        Time.timeScale = 1;
    }
}
