using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet2 : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_LayerMask;

    private int m_Damage;
    private bool m_IsBoom;

    private void OnEnable()
    {
        StartCoroutine(DespwanBullet());
    }
    IEnumerator DespwanBullet()
    {
        // Ȱ��ȭ �� �� 1�� ���� �ٸ� ��, ���� �ε����� �ʾ��� ���
        yield return new WaitForSeconds(1.5f);

        // ������Ʈ�� ���� Ȱ��ȭ �� ���¶�� ��Ȱ��ȭ �� ������Ʈ Ǯ�� �Ŵ��� ť�� �����
        if (gameObject.activeSelf)
        {
            m_IsBoom = false;
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBullet2Key);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf && m_IsBoom)
        {
            // �÷��̾� ��ó�� ��� ���� �ݶ��̴� Ȯ��
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2.5f, m_LayerMask);

            for (int i = 0; i < cols.Length; i++)
            {
                // ���Ϳ��� ������ �ο�
                Monster monster = cols[i].gameObject.GetComponent<Monster>();
                monster.MonsterOnDamage(m_Damage);

                // ���� �ʱ�ȭ
                m_IsBoom = false;
                ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_PlayerBullet2Key);
            }
            // ���� ����Ʈ
            ExplosionEffect();
        }
    }
    public void SetDamage(int _damage)
    {
        // ������ ����
        m_Damage = _damage;
    }
    public void SetBoom(bool _bool)
    {
        m_IsBoom = _bool;
    }
    private void ExplosionEffect()
    {
        // ���� ����Ʈ ����
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_Skill01EffectKey);

        // ���� ����Ʈ ���
        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
