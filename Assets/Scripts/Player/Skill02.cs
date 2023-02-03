using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill02 : Skill
{
    [SerializeField]
    private GameObject[] m_Effects;
    // ����Ű
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            for (int i = 0; i < m_Effects.Length; i++)
            {
                // ������ ������ ����
                float x = Random.Range(-4f, 4f);
                float y = Random.Range(-4f, 4f);

                Vector3 dir = new Vector3(x, y, 0);

                // ������ ��ġ���� Ȱ��ȭ
                m_Effects[i].transform.position = m_Player.transform.position + dir;
                m_Effects[i].SetActive(true);

                // Mine ��ũ��Ʈ�� ������
                Mine mine = m_Effects[i].GetComponent<Mine>();

                // ������ �ο�
                mine.SetDamage((int)(m_GunManager.Damage * (1.5f + m_Level * 1.5f)));

                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < m_Effects.Length; i++)
            {
                // Mine ��ũ��Ʈ�� ������
                Mine mine = m_Effects[i].GetComponent<Mine>();

                // ��ġ �� �غ� ���¸� true
                mine.SetBoom(true);
            }

            yield return new WaitForSeconds(12f - (m_Level * 1f));

            for (int i = 0; i < m_Effects.Length; i++)
            {
                // Mine ��ũ��Ʈ�� ������
                Mine mine = m_Effects[i].GetComponent<Mine>();

                mine.Explosion();
            }
        }
    }
}
