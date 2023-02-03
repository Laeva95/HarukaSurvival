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

    // 확인 버튼 클릭 시 실행 함수
    public void SelectBtn()
    {
        // 상태가 레벨업인지, 보스클리어인지 확인
        if (m_IsLevelUp)
        {
            // 레벨업 슬롯 선택 함수 실행
            m_LevelUp.SelectSlot();
            m_LevelUp.m_Player.SetIsLevelUp(false);
        }
        else
        {
            // 보스클리어 슬롯 선택 함수 실행
            m_Boss.SelectSlot();
            GameManager.Instance.SetBossClear(false);
        }
        GameManager.Instance.CloseStatus();
        // 게임 오브젝트 비활성화
        gameObject.SetActive(false);
        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }

    // 취소 버튼 클릭 시 실행 함수
    public void CancelBtn()
    {
        // 게임 오브젝트 비활성화
        gameObject.SetActive(false);
        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }
}
