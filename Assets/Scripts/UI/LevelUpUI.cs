using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �ҷ����� �� Ŭ����
[System.Serializable]
public class LevelUpInfo
{
    public string name;
    public string description;
}

public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_LevelUpUI;
    [SerializeField]
    private Haruka m_Player;
    [SerializeField]
    private SkillManager m_SkillManager;
    public LevelUpSlot[] m_SkillSlots;

    LevelUpInfo m_Info = new LevelUpInfo();

    // ��ų �ε����� ��� ���� ����Ʈ
    private List<int> m_SkillList;

    // ���� ��ų �� ����
    private int m_SkillCount = 13;


    private void Awake()
    {
        // �ڽ� ������Ʈ�� Slot�� ��� ������
        m_SkillSlots = GetComponentsInChildren<LevelUpSlot>();

        // ��ų �� ������ŭ�� ũ�⸦ �Ҵ�����
        m_SkillList = new List<int>(m_SkillCount);

        // ��ų ������ŭ ����Ʈ�� ä����
        for (int i = 0; i < m_SkillCount; i++)
        {
            m_SkillList.Add(i);
        }
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        m_LevelUpUI.SetActive(true);

        SkillMaxLevelCheck();
        SuffleSkillList(m_SkillList);
        SelectSkillIndex();
        SetSkillText();
    }
    void SkillMaxLevelCheck()
    {
        for (int i = 0; i < m_SkillList.Count; i++)
        {
            switch (m_SkillList[i])
            {
                case 1:
                    if (m_Player.GunManager.FireDelay <= 0.3f)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 2:
                    if (m_Player.GunManager.FireRate >= 10)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 3:
                    if (m_SkillManager.Skill02.Level >= 10)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 6:
                    if (m_Player.MoveLevel >= 5)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 7:
                    if (Camera.main.orthographicSize >= 5.9f)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 8:
                    if (m_Player.GunManager.Penetration2 >= 5)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 9:
                    if (m_Player.ShieldLevel >= 5)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 10:
                    if (m_SkillManager.Skill01.Level >= 10)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 11:
                    if (m_SkillManager.Skill00.Level >= 10)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                case 12:
                    if (m_SkillManager.Skill03.Level >= 10)
                    {
                        m_SkillList.Remove(m_SkillList[i]);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    List<T> SuffleSkillList<T>(List<T> _list)
    {
        for (int i = _list.Count - 1; i > 0; i--)
        {
            int random = Random.Range(0, i);
            T temp = _list[i];
            _list[i] = _list[random];
            _list[random] = temp;
        }
        return _list;
    }

    void SelectSkillIndex()
    {
        for (int i = 0; i < m_SkillSlots.Length; i++)
        {
             m_SkillSlots[i].m_SkillIndex = m_SkillList[i];
        }
    }

    void SetSkillText()
    {
        for (int i = 0; i < m_SkillSlots.Length; i++)
        {
            LoadLevelText(m_SkillSlots[i].m_SkillIndex);

            m_SkillSlots[i].m_NameText.text = m_Info.name;
            m_SkillSlots[i].m_DescriptionText.text = m_Info.description;
        }
    }

    void LoadLevelText(int _num)
    {
        TextAsset text = Resources.Load<TextAsset>($"level/{string.Format("level{0:D2}", _num)}");

        m_Info = JsonUtility.FromJson<LevelUpInfo>(text.text);
    }
    string GetFileName(int _num)
    {
        // ������ ������ ���� ���� �̸��� ������
        return string.Format("level{0:D2}", _num);
    }
}
