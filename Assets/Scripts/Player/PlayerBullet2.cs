using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet2 : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_LayerMask;

    private int m_Damage;
    private bool m_IsBoom;

    private void OnEnable()
    {
        StartCoroutine(DespwanBullet());
    }
    IEnumerator DespwanBullet()
    {
        // 활성화 된 후 1초 동안 다른 적, 벽과 부딪히지 않았을 경우
        yield return new WaitForSeconds(1.5f);

        // 오브젝트가 아직 활성화 된 상태라면 비활성화 후 오브젝트 풀링 매니저 큐에 재삽입
        if (gameObject.activeSelf)
        {
            m_IsBoom = false;
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBullet2Key);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf && m_IsBoom)
        {
            // 플레이어 근처의 모든 몬스터 콜라이더 확인
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2.5f, m_LayerMask);

            for (int i = 0; i < cols.Length; i++)
            {
                // 몬스터에게 데미지 부여
                Monster monster = cols[i].gameObject.GetComponent<Monster>();
                monster.MonsterOnDamage(m_Damage);

                // 변수 초기화
                m_IsBoom = false;
                ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBullet2Key);
            }
            // 폭발 이펙트
            ExplosionEffect();
        }
    }
    public void SetDamage(int _damage)
    {
        // 데미지 설정
        m_Damage = _damage;
    }
    public void SetBoom(bool _bool)
    {
        m_IsBoom = _bool;
    }
    private void ExplosionEffect()
    {
        // 폭발 이펙트 생성
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_Skill01EffectKey);

        // 폭발 이펙트 재생
        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
