using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private int m_Damage;
    private int m_Penetration;
    private void OnEnable()
    {
        StartCoroutine(DespwanBullet());
    }
    IEnumerator DespwanBullet()
    {
        // 활성화 된 후 0.7초 동안 다른 적, 벽과 부딪히지 않았을 경우
        yield return new WaitForSeconds(0.7f);

        // 오브젝트가 아직 활성화 된 상태라면 비활성화 후 오브젝트 풀링 매니저 큐에 재삽입
        if (gameObject.activeSelf)
        {
            m_Penetration = 0;
            ObjectPoolingManager.Instance.InsertQueue(gameObject ,ObjectPoolingManager.m_PlayerBulletKey);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            // 몬스터에 부딪힐 경우 데미지를 줌
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);

            // 관통 횟수가 남아있다면 1 감소, 남아있지 않다면 오브젝트 큐에 재삽입
            if(m_Penetration > 0)
            {
                m_Penetration--;
            }
            else
            {
                ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBulletKey);
            }
        }
    }
    public void SetDamage(int _damage)
    {
        // 데미지 설정
        m_Damage = _damage;
    }
    public void SetPenetration(int _pen)
    {
        // 관통 설정
        m_Penetration += _pen;
    }
}
