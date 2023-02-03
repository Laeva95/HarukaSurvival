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
        // Ȱ��ȭ �� �� 1�� ���� �÷��̾�, ���� �ε����� �ʾ��� ���
        yield return new WaitForSeconds(1f);

        BoomBullet();
    }
    void BoomBullet()
    {
        // ������Ʈ�� ���� Ȱ��ȭ �� ���¶�� ��Ȱ��ȭ �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBulletKey);

                bullet.transform.position = gameObject.transform.position;

                // ���� �ҷ��� ������ �ο�
                MonsterBullet pBullet = bullet.GetComponent<MonsterBullet>();
                pBullet.SetDamage(m_Damage);;

                // ���� �ҷ��� RigidBody�� �����ͼ� ���콺�� ����Ű�� ������ ���� �������� ������ ���� ���ؼ� �߻�
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                // for�� 1ȸ�� 0.25PI�� ������ ���� �߻�
                // i 8ȸ�� �ѹ���
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
            // �÷��̾ �ε��� ��� �������� �� �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
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
