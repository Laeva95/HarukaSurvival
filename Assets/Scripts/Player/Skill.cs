using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected Haruka m_Player;
    [SerializeField]
    protected GunManager m_GunManager;

    // 스킬의 레벨 변수
    protected int m_Level = 0;
    public int Level => m_Level;


    // 스킬 레벨업 함수
    // 최초 레벨업 시 오브젝트를 활성화 해줌
    public void SkillLevelUp()
    {
        m_Level++;

        // 오브젝트가 활성화되어있지 않다면 활성화
        // 스킬 코루틴 실행
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(StartSkill());
        }
    }

    // 스킬 반복 코루틴
    // 한번 활성화되면 게임 종료까지 반복적으로 실행
    // 스킬마다 내용이 다르고, 반드시 작성해야 하므로 추상 함수로 작성
    protected abstract IEnumerator StartSkill();
}
