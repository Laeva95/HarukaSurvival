using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill02 : Skill
{
    // ����Ű
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            for (int i = 0; i < 3 + (m_Level / 2); i++)
            {
                GameObject skillEffect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerSkill2Key);
                // ������ ������ ����
                float x = Random.Range(-4f, 4f);
                float y = Random.Range(-4f, 4f);

                Vector3 dir = new Vector3(x, y, 0);

                // ������ ��ġ���� Ȱ��ȭ
                skillEffect.transform.position = m_Player.transform.position + dir;

                // Mine ��ũ��Ʈ�� ������
                Mine mine = skillEffect.GetComponent<Mine>();

                // ������ �ο�
                mine.SetDamage((int)(m_GunManager.Damage * (1.5f + m_Level * 1.5f)));

                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(12f - (m_Level * 1f));
        }
    }
}
