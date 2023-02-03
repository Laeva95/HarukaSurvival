using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데이터 불러오기 용 클래스
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

    // 스킬 인덱스를 담기 위한 리스트
    private List<int> m_SkillList;

    // 현재 스킬 총 개수
    private int m_SkillCount = 13;


    private void Awake()
    {
        // 자식 오브젝트의 Slot을 모두 가져옴
        m_SkillSlots = GetComponentsInChildren<LevelUpSlot>();

        // 스킬 총 개수만큼의 크기를 할당해줌
        m_SkillList = new List<int>(m_SkillCount);

        // 스킬 개수만큼 리스트를 채워줌
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
        // 지정된 포맷을 통해 파일 이름을 가져옴
        return string.Format("level{0:D2}", _num);
    }
}
