using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill00 : Skill
{
    // 몬스터 레이어 마스크
    [SerializeField]
    private LayerMask m_LayerMask;
    [SerializeField]
    private GameObject m_Effect;

    // 카요코
    // 근처 모든 적들에게 데미지
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            // 플레이어 근처의 모든 몬스터 콜라이더 확인
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2.5f + (m_Level * 0.15f), m_LayerMask);

            for (int i = 0; i < cols.Length; i++)
            {
                // 몬스터에게 데미지 부여
                Monster monster = cols[i].gameObject.GetComponent<Monster>();
                monster.MonsterOnDamage((int)(m_GunManager.Damage * (1.5f + m_Level * 0.3f)));

                // 몬스터를 플레이어의 반대 방향으로 밀쳐냄
                Vector3 dir = (cols[i].gameObject.transform.position - m_Player.transform.position).normalized;
                monster.Rigid.AddForce(dir * (1.5f + 0.1f * m_Level), ForceMode2D.Impulse);
            }
            // 이펙트 활성화 0.8초 후 대기
            m_Effect.SetActive(true);
            yield return new WaitForSeconds(0.8f);

            // 이펙트 비활성화 후 대기
            m_Effect.SetActive(false);
            yield return new WaitForSeconds(3.2f - (0.3f * m_Level));
        }
    }
}
