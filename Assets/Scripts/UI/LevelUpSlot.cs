using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpSlot : MonoBehaviour
{
    public Text m_NameText;
    public Text m_DescriptionText;
    public int m_SkillIndex;
    public Haruka m_Player;
    [SerializeField]
    private GameObject m_LevelUpUI;
    [SerializeField]
    private GameObject m_ConfirmUI;
    [SerializeField]
    private SkillManager m_SkillManager;


    // ���� �� ������ �����Ұ��� Ȯ��â�� ���� �Լ�
    public void ConfirmSelect()
    {
        string str = "";
        if (m_SkillIndex == 6 || m_SkillIndex == 7 || m_SkillIndex == 11 || m_SkillIndex == 3 || m_SkillIndex == 13)
        {
            str = "��";
        }
        else
        {
            str = "��";
        }
        // ������ ���¸� true�� �Ѱ���
        ConfirmUI confirm = m_ConfirmUI.GetComponent<ConfirmUI>();
        confirm.m_LevelUp = this;
        confirm.m_IsLevelUp = true;
        confirm.m_Text.text = $"{m_NameText.text}{str} �����Ͻðڽ��ϱ�?";

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
        m_ConfirmUI.SetActive(true);
    }

    public void SelectSlot()
    {
        // �ش� ������ Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
        // �� ������ �ε����� �´� ȿ���� �ߵ���
        switch (m_SkillIndex)
        {
            case 0:
                // ���ݷ� +5
                m_Player.GunManager.SetDamage(5);
                break;
            case 1:
                // ���� ������ -20%
                m_Player.GunManager.SetFireDelay(0.8f);
                break;
            case 2:
                // �߻� ���� +1
                m_Player.GunManager.SetFireRate(1);
                break;
            case 3:
                // Skill02 ���� +1
                m_SkillManager.Skill02.SkillLevelUp();
                break;
            case 4:
                // �ִ� ü�� +20(ȸ�� +20)
                m_Player.SetMaxHpUp(20);
                break;
            case 5:
                // ü�� ȸ�� +25%
                m_Player.SetHpUp((int)(m_Player.PlayerMaxHP * 0.25f));
                break;
            case 6:
                // �̵��ӵ� ���� +1
                m_Player.SetMoveUp();
                break;
            case 7:
                // ī�޶� �þ� ���� +0.2
                Camera.main.orthographicSize += 0.2f;
                Camera.main.orthographicSize = (float)Math.Round(Camera.main.orthographicSize * 10) / 10;
                break;
            case 8:
                // ���� ���� +1
                m_Player.GunManager.SetPenetration2();
                break;
            case 9:
                // �ǵ� ���� +1
                m_Player.SetShieldUp();
                break;
            case 10:
                // Skill01 ���� +1
                m_SkillManager.Skill01.SkillLevelUp();
                break;
            case 11:
                // Skill00 ���� +1
                m_SkillManager.Skill00.SkillLevelUp();
                break;
            case 12:
                // Skill03 ���� +1
                m_SkillManager.Skill03.SkillLevelUp();
                break;
            case 13:
                // ������ ȹ�� ���� 20%
                m_Player.SetItemCheck(1.2f);
                break;
            default:
                break;
        }
        CloseUI();

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }

    private void CloseUI()
    {
        // timeScale�� 1�� �ǵ����� ������UI ��Ȱ��ȭ
        m_LevelUpUI.SetActive(false);

        Time.timeScale = 1;
    }
}
