using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet3 : MonoBehaviour
{
    private int m_Damage;
    private void OnEnable()
    {
        StartCoroutine(DespwanBullet());
    }
    IEnumerator DespwanBullet()
    {
        // 활성화 된 후 0.8초 동안 다른 적, 벽과 부딪히지 않았을 경우
        yield return new WaitForSeconds(0.8f);

        // 오브젝트가 아직 활성화 된 상태라면 비활성화 후 오브젝트 풀링 매니저 큐에 재삽입
        if (gameObject.activeSelf)
        {
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBullet3Key);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            // 몬스터를 관통하고 데미지를 줌
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);
        }
    }
    public void SetDamage(int _damage)
    {
        // 데미지 설정
        m_Damage = _damage;
    }
}
