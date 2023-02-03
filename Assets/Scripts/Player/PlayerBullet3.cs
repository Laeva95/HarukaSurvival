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
        // Ȱ��ȭ �� �� 0.8�� ���� �ٸ� ��, ���� �ε����� �ʾ��� ���
        yield return new WaitForSeconds(0.8f);

        // ������Ʈ�� ���� Ȱ��ȭ �� ���¶�� ��Ȱ��ȭ �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
        if (gameObject.activeSelf)
        {
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBullet3Key);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            // ���͸� �����ϰ� �������� ��
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);
        }
    }
    public void SetDamage(int _damage)
    {
        // ������ ����
        m_Damage = _damage;
    }
}
