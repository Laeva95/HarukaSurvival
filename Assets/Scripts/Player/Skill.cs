using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected Haruka m_Player;
    [SerializeField]
    protected GunManager m_GunManager;

    // ��ų�� ���� ����
    protected int m_Level = 0;
    public int Level => m_Level;


    // ��ų ������ �Լ�
    // ���� ������ �� ������Ʈ�� Ȱ��ȭ ����
    public void SkillLevelUp()
    {
        m_Level++;

        // ������Ʈ�� Ȱ��ȭ�Ǿ����� �ʴٸ� Ȱ��ȭ
        // ��ų �ڷ�ƾ ����
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(StartSkill());
        }
    }

    // ��ų �ݺ� �ڷ�ƾ
    // �ѹ� Ȱ��ȭ�Ǹ� ���� ������� �ݺ������� ����
    // ��ų���� ������ �ٸ���, �ݵ�� �ۼ��ؾ� �ϹǷ� �߻� �Լ��� �ۼ�
    protected abstract IEnumerator StartSkill();
}
