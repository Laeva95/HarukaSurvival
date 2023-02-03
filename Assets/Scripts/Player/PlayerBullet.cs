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
        // Ȱ��ȭ �� �� 0.7�� ���� �ٸ� ��, ���� �ε����� �ʾ��� ���
        yield return new WaitForSeconds(0.7f);

        // ������Ʈ�� ���� Ȱ��ȭ �� ���¶�� ��Ȱ��ȭ �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
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
            // ���Ϳ� �ε��� ��� �������� ��
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);

            // ���� Ƚ���� �����ִٸ� 1 ����, �������� �ʴٸ� ������Ʈ ť�� �����
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
        // ������ ����
        m_Damage = _damage;
    }
    public void SetPenetration(int _pen)
    {
        // ���� ����
        m_Penetration += _pen;
    }
}
