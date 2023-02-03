using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_LayerMask;
    private int m_Damage;
    private bool m_IsBoom;

    public void SetDamage(int _damage)
    {
        m_Damage = _damage;
    }
    public void SetBoom(bool _bool)
    {
        m_IsBoom = _bool;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            Explosion();
        }
    }
    public void Explosion()
    {
        if (!m_IsBoom)
        {
            return;
        }
        // 플레이어 근처의 모든 몬스터 콜라이더 확인
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, m_LayerMask);

        for (int i = 0; i < cols.Length; i++)
        {
            // 몬스터에게 데미지 부여
            Monster monster = cols[i].gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);

            // 변수 초기화
            m_IsBoom = false;
            gameObject.SetActive(false);
        }
        // 폭발 이펙트
        ExplosionEffect();
    }

    private void ExplosionEffect()
    {
        // 폭발 이펙트 생성
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterEffect2Key);

        // 폭발 이펙트 재생
        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
