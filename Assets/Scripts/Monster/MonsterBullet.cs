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
        // Ȱ��ȭ �� �� 3�� ���� �÷��̾�, ���� �ε����� �ʾ��� ���
        yield return new WaitForSeconds(3f);

        // ������Ʈ�� ���� Ȱ��ȭ �� ���¶�� ��Ȱ��ȭ �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
        if (gameObject.activeSelf)
        {
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_MonsterBulletKey);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            // �÷��̾ �ε��� ��� �������� �� �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
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
