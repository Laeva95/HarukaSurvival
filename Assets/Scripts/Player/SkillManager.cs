using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private Haruka m_Player;                    // 플레이어 스크립트
    [SerializeField]
    private LayerMask m_LayerMask;              // 플레이어가 감지할 몬스터의 레이어마스크

    [SerializeField]
    private Skill00 m_Skill00;
    [SerializeField]
    private Skill01 m_Skill01;
    [SerializeField]
    private Skill02 m_Skill02;
    [SerializeField]
    private Skill03 m_Skill03;

    public Skill00 Skill00 => m_Skill00;
    public Skill01 Skill01 => m_Skill01;
    public Skill02 Skill02 => m_Skill02;
    public Skill03 Skill03 => m_Skill03;

    void Start()
    {
        StartCoroutine(MonsterDistanceCheck());
    }

    IEnumerator MonsterDistanceCheck()
    {
        yield return new WaitForSeconds(1f);

        while (GameManager.Instance.IsPlay)
        {
            // 플레이어 근처의 모든 몬스터 콜라이더 확인
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 10f, m_LayerMask);
            // 가장 가까운 타겟
            Transform target = null;

            // 몬스터가 하나라도 존재하는 경우 실행
            if (cols.Length > 0)
            {
                // 가장 가까운 타겟과의 거리 변수
                float targetDistance = Mathf.Infinity;

                // 모든 몬스터와의 거리 비교
                for (int i = 0; i < cols.Length; i++)
                {
                    float distance = Vector3.SqrMagnitude(transform.position - cols[i].transform.position);

                    // 현재 거리보다 더 가까운 몬스터가 존재할 경우 해당 몬스터와의 거리, 트랜스폼 저장
                    if (targetDistance > distance)
                    {
                        targetDistance = distance;
                        target = cols[i].transform;
                    }
                }
            }
            m_Player.SetTargetTF(target);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
