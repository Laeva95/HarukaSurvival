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
    private void OnEnable()
    {
        m_IsBoom = false;
        StartCoroutine("LifeTimeCoroutine");
    }
    private void OnDisable()
    {
        StopCoroutine("LifeTimeCoroutine");
    }
    IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        SetBoom(true);

        yield return new WaitForSeconds(10f);

        if (gameObject.activeSelf)
        {
            Explosion();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_IsBoom)
        {
            return;
        }
        if (collision.gameObject.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            Explosion();
        }
    }
    public void Explosion()
    {
        // 플레이어 근처의 모든 몬스터 콜라이더 확인
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, m_LayerMask);

        for (int i = 0; i < cols.Length; i++)
        {
            // 몬스터에게 데미지 부여
            Monster monster = cols[i].gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);
        }
        // 변수 초기화
        m_IsBoom = false;
        ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerSkill2Key);

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
