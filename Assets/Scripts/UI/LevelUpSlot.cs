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


    // 정말 그 슬롯을 선택할건지 확인창을 띄우는 함수
    public void ConfirmSelect()
    {
        string str = "";
        if (m_SkillIndex == 6 || m_SkillIndex == 7 || m_SkillIndex == 11 || m_SkillIndex == 3 || m_SkillIndex == 13)
        {
            str = "를";
        }
        else
        {
            str = "을";
        }
        // 레벨업 상태를 true로 넘겨줌
        ConfirmUI confirm = m_ConfirmUI.GetComponent<ConfirmUI>();
        confirm.m_LevelUp = this;
        confirm.m_IsLevelUp = true;
        confirm.m_Text.text = $"{m_NameText.text}{str} 선택하시겠습니까?";

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
        m_ConfirmUI.SetActive(true);
    }

    public void SelectSlot()
    {
        // 해당 슬롯이 클릭되었을 때 호출되는 함수
        // 각 슬롯의 인덱스에 맞는 효과가 발동함
        switch (m_SkillIndex)
        {
            case 0:
                // 공격력 +5
                m_Player.GunManager.SetDamage(5);
                break;
            case 1:
                // 공격 딜레이 -20%
                m_Player.GunManager.SetFireDelay(0.8f);
                break;
            case 2:
                // 발사 개수 +1
                m_Player.GunManager.SetFireRate(1);
                break;
            case 3:
                // Skill02 레벨 +1
                m_SkillManager.Skill02.SkillLevelUp();
                break;
            case 4:
                // 최대 체력 +20(회복 +20)
                m_Player.SetMaxHpUp(20);
                break;
            case 5:
                // 체력 회복 +25%
                m_Player.SetHpUp((int)(m_Player.PlayerMaxHP * 0.25f));
                break;
            case 6:
                // 이동속도 레벨 +1
                m_Player.SetMoveUp();
                break;
            case 7:
                // 카메라 시야 범위 +0.2
                Camera.main.orthographicSize += 0.2f;
                Camera.main.orthographicSize = (float)Math.Round(Camera.main.orthographicSize * 10) / 10;
                break;
            case 8:
                // 관통 레벨 +1
                m_Player.GunManager.SetPenetration2();
                break;
            case 9:
                // 실드 레벨 +1
                m_Player.SetShieldUp();
                break;
            case 10:
                // Skill01 레벨 +1
                m_SkillManager.Skill01.SkillLevelUp();
                break;
            case 11:
                // Skill00 레벨 +1
                m_SkillManager.Skill00.SkillLevelUp();
                break;
            case 12:
                // Skill03 레벨 +1
                m_SkillManager.Skill03.SkillLevelUp();
                break;
            case 13:
                // 아이템 획득 범위 20%
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
        // timeScale을 1로 되돌리고 레벨업UI 비활성화
        m_LevelUpUI.SetActive(false);

        Time.timeScale = 1;
    }
}
