using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill00 : Skill
{
    // ���� ���̾� ����ũ
    [SerializeField]
    private LayerMask m_LayerMask;
    [SerializeField]
    private GameObject m_Effect;

    // ī����
    // ��ó ��� ���鿡�� ������
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            // �÷��̾� ��ó�� ��� ���� �ݶ��̴� Ȯ��
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2.5f + (m_Level * 0.15f), m_LayerMask);

            for (int i = 0; i < cols.Length; i++)
            {
                // ���Ϳ��� ������ �ο�
                Monster monster = cols[i].gameObject.GetComponent<Monster>();
                monster.MonsterOnDamage((int)(m_GunManager.Damage * (1.5f + m_Level * 0.3f)));

                // ���͸� �÷��̾��� �ݴ� �������� ���ĳ�
                Vector3 dir = (cols[i].gameObject.transform.position - m_Player.transform.position).normalized;
                monster.Rigid.AddForce(dir * (1.5f + 0.1f * m_Level), ForceMode2D.Impulse);
            }
            // ����Ʈ Ȱ��ȭ 0.8�� �� ���
            m_Effect.SetActive(true);
            yield return new WaitForSeconds(0.8f);

            // ����Ʈ ��Ȱ��ȭ �� ���
            m_Effect.SetActive(false);
            yield return new WaitForSeconds(3.2f - (0.3f * m_Level));
        }
    }
}
