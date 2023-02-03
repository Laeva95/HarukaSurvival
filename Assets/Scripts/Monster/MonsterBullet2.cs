using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet2 : MonoBehaviour
{
    private int m_Damage;

    private void OnEnable()
    {
        StartCoroutine(DespwanBullet());
    }
    IEnumerator DespwanBullet()
    {
        // 활성화 된 후 1초 동안 플레이어, 벽과 부딪히지 않았을 경우
        yield return new WaitForSeconds(1f);

        BoomBullet();
    }
    void BoomBullet()
    {
        // 오브젝트가 아직 활성화 된 상태라면 비활성화 후 오브젝트 풀링 매니저 큐에 재삽입
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBulletKey);

                bullet.transform.position = gameObject.transform.position;

                // 몬스터 불렛에 데미지 부여
                MonsterBullet pBullet = bullet.GetComponent<MonsterBullet>();
                pBullet.SetDamage(m_Damage);;

                // 몬스터 불렛의 RigidBody를 가져와서 마우스가 가리키는 방향을 향해 퍼지도록 랜덤한 값을 곱해서 발사
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                // for문 1회당 0.25PI의 각도를 향해 발사
                // i 8회당 한바퀴
                Vector2 vec = new Vector2(Mathf.Cos(Mathf.PI * 0.25f * i), Mathf.Sin(Mathf.PI * 0.25f * i)).normalized;

                rigid.velocity = vec * 6f;
            }
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_MonsterBullet2Key);
            SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            // 플레이어에 부딪힐 경우 데미지를 준 후 오브젝트 풀링 매니저 큐에 재삽입
            Haruka player = collision.gameObject.GetComponent<Haruka>();
            player.PlayerOnDamage(m_Damage);

            BoomBullet();
        }
    }
    public void SetDamage(int _damage)
    {
        m_Damage = _damage;
    }
}
