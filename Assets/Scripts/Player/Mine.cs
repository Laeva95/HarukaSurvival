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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && gameObject.activeSelf && collision.gameObject.activeSelf)
        {
            Explosion();
        }
    }
    public void Explosion()
    {
        if (!m_IsBoom)
        {
            return;
        }
        // �÷��̾� ��ó�� ��� ���� �ݶ��̴� Ȯ��
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, m_LayerMask);

        for (int i = 0; i < cols.Length; i++)
        {
            // ���Ϳ��� ������ �ο�
            Monster monster = cols[i].gameObject.GetComponent<Monster>();
            monster.MonsterOnDamage(m_Damage);

            // ���� �ʱ�ȭ
            m_IsBoom = false;
            gameObject.SetActive(false);
        }
        // ���� ����Ʈ
        ExplosionEffect();
    }

    private void ExplosionEffect()
    {
        // ���� ����Ʈ ����
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterEffect2Key);

        // ���� ����Ʈ ���
        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
