using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    private int m_Damage;

    private void OnEnable()
    {
        StartCoroutine(DespwanBullet());
    }
    IEnumerator DespwanBullet()
    {
        // 활성화 된 후 3초 동안 플레이어, 벽과 부딪히지 않았을 경우
        yield return new WaitForSeconds(3f);

        // 오브젝트가 아직 활성화 된 상태라면 비활성화 후 오브젝트 풀링 매니저 큐에 재삽입
        if (gameObject.activeSelf)
        {
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_MonsterBulletKey);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            // 플레이어에 부딪힐 경우 데미지를 준 후 오브젝트 풀링 매니저 큐에 재삽입
            Haruka player = collision.gameObject.GetComponent<Haruka>();
            player.PlayerOnDamage(m_Damage);

            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_MonsterBulletKey);
        }
    }
    public void SetDamage(int _damage)
    {
        m_Damage = _damage;
    }
}
