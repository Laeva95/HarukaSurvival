using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private Haruka m_Player;                    // �÷��̾� ��ũ��Ʈ
    [SerializeField]
    private LayerMask m_LayerMask;              // �÷��̾ ������ ������ ���̾��ũ

    [SerializeField]
    private Skill00 m_Skill00;
    [SerializeField]
    private Skill01 m_Skill01;
    [SerializeField]
    private Skill02 m_Skill02;
    [SerializeField]
    private Skill03 m_Skill03;

    public Skill00 Skill00 => m_Skill00;
    public Skill01 Skill01 => m_Skill01;
    public Skill02 Skill02 => m_Skill02;
    public Skill03 Skill03 => m_Skill03;

    void Start()
    {
        StartCoroutine(MonsterDistanceCheck());
    }

    IEnumerator MonsterDistanceCheck()
    {
        yield return new WaitForSeconds(1f);

        while (GameManager.Instance.IsPlay)
        {
            // �÷��̾� ��ó�� ��� ���� �ݶ��̴� Ȯ��
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 10f, m_LayerMask);
            // ���� ����� Ÿ��
            Transform target = null;

            // ���Ͱ� �ϳ��� �����ϴ� ��� ����
            if (cols.Length > 0)
            {
                // ���� ����� Ÿ�ٰ��� �Ÿ� ����
                float targetDistance = Mathf.Infinity;

                // ��� ���Ϳ��� �Ÿ� ��
                for (int i = 0; i < cols.Length; i++)
                {
                    float distance = Vector3.SqrMagnitude(transform.position - cols[i].transform.position);

                    // ���� �Ÿ����� �� ����� ���Ͱ� ������ ��� �ش� ���Ϳ��� �Ÿ�, Ʈ������ ����
                    if (targetDistance > distance)
                    {
                        targetDistance = distance;
                        target = cols[i].transform;
                    }
                }
            }
            m_Player.SetTargetTF(target);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
