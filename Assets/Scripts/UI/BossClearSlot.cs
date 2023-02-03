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
    // 정말 그 슬롯을 선택할건지 확인창을 띄우는 함수
    public void ConfirmSelect()
    {
        string str = "";
        if (m_SkillIndex == 0 || m_SkillIndex == 2 || m_SkillIndex == 6 || m_SkillIndex == 7)
        {
            str = "를";
        }
        else
        {
            str = "을";
        }
        // 레벨업 상태를 false로 넘겨줌
        ConfirmUI confirm = m_ConfirmUI.GetComponent<ConfirmUI>();
        confirm.m_Boss = this;
        confirm.m_IsLevelUp = false;
        confirm.m_Text.text = $"{m_NameText.text}{str} 선택하시겠습니까?";

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
        m_ConfirmUI.SetActive(true);
    }

    public void SelectSlot()
    {
        // 해당 슬롯이 클릭되었을 때 호출되는 함수
        // 각 슬롯의 인덱스에 맞는 효과가 발동함
        // 중복으로 획득할 수 없도록 해당 번호에 맞는 bool 값 true
        m_Boss.m_IsNum[m_SkillIndex] = true;

        switch (m_SkillIndex)
        {
            case 0:
                // 최대 체력 +100(체력 회복 +100)
                m_Player.SetMaxHpUp(100);
                break;
            case 1:
                // 공격력 +20
                m_Player.GunManager.SetDamage(20);
                break;
            case 2:
                // 몹 전체 Slow 상태 활성화
                GameManager.Instance.SetSlow();
                break;
            case 3:
                // 관통 +1
                m_Player.GunManager.SetPenetration();
                break;
            case 4:
                // 기본 공격 데미지 50% 증가
                m_Player.GunManager.SetIsDamageUp();
                break;
            case 5:
                // 이동속도2 레벨 +1
                m_Player.SetMoveUp2();
                break;
            case 6:
                // 받는 데미지 -30%
                m_Player.SunglassSlowStart();
                break;
            case 7:
                // 10초마다 체력 회복
                m_Player.HealKitStart();
                break;
            case 8:
                // 발사 개수 +5
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
        // timeScale을 1로 되돌리고 보스 클리어UI 비활성화
        m_BossClearUI.SetActive(false);

        Time.timeScale = 1;
    }
}
