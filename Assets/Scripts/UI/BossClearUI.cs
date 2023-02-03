using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// json���Ͽ��� ������ ������ ��� ���� Ŭ����
[System.Serializable]
public class BossClearInfo
{
    public string name;
    public string description;
}
public class BossClearUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BossClearUI;
    [SerializeField]
    private Haruka m_Player;
    [SerializeField]
    private Sprite[] m_Sprites;
    public BossClearSlot[] m_Slots;

    BossClearInfo m_Info = new BossClearInfo();                 // �������� ������ ���� Ŭ����

    public bool[] m_IsNum;                                      // �ߺ� ���� ������ ���� bool �迭

    private List<int> m_List;                                   // ��ų �ε����� ��� ���� ����Ʈ

    private int m_SkillCount = 9;                               // ���� ��ų �� ����


    private void Awake()
    {
        // �ڽ� ������Ʈ�� Slot�� ��� ������
        m_Slots = GetComponentsInChildren<BossClearSlot>();

        // ��ų �� ������ŭ�� ũ�⸦ �Ҵ�����
        m_List = new List<int>(m_SkillCount);

        // ��ų ������ŭ ����Ʈ�� ä����
        for (int i = 0; i < m_SkillCount; i++)
        {
            m_List.Add(i);
        }

        // ��ų ������ŭ �迭�� ä����
        m_IsNum = new bool[m_List.Count];
        for (int i = 0; i < m_IsNum.Length; i++)
        {
            m_IsNum[i] = false;
        }
    }

    public void BossClear()
    {
        // �ð� ����
        Time.timeScale = 0;

        // ������ UI�� Ȱ��ȭ ��Ŵ
        m_BossClearUI.SetActive(true);

        // ��ų �ε����� ����� ����Ʈ�� ������ ��ų�� �ִ��� Ȯ�� �� �׸񿡼� ����
        BossItemCheck();

        // ����Ʈ�� �׸��� �������� ����
        SuffleList(m_List);

        // ����Ʈ�� 0, 1, 2�ε��� ���� ���Կ� ������� �־���
        SelectBossItemIndex();

        // ������ �ؽ�Ʈ ������Ʈ
        SetItemText();
    }
    void BossItemCheck()
    {
        // ����Ʈ�� ��� �׸��� ���鼭 Ȱ��ȭ�� ���� �������� �ִ��� Ȯ��
        for (int i = 0; i < m_List.Count; i++)
        {
            // �̹� �����ϴ� �������� ����Ʈ���� ����
            switch (m_List[i])
            {
                case 0:
                    if (m_IsNum[0])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 1:
                    if (m_IsNum[1])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 2:
                    if (m_IsNum[2])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 3:
                    if (m_IsNum[3])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 4:
                    if (m_IsNum[4])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 5:
                    if (m_IsNum[5])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 6:
                    if (m_IsNum[6])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 7:
                    if (m_IsNum[7])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                case 8:
                    if (m_IsNum[8])
                    {
                        m_List.Remove(m_List[i]);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    List<T> SuffleList<T>(List<T> _list)
    {
        // ����Ʈ�� �׸��� 2�� �̻��϶��� �۵���
        for (int i = _list.Count - 1; i > 0; i--)
        {
            // ���� ���� ����
            // int�� ���������̹Ƿ� 0 ~ i-1������ ����
            int random = Random.Range(0, i);

            // ���� �ε���i�� ����ִ� _list[i]�� i�̸��� ������ �ε���random�� ����ִ� _list[random]�� ���� ���� �ٲ���
            T temp = _list[i];
            _list[i] = _list[random];
            _list[random] = temp;
        }
        // �Ű������� �޾ƿ� ����Ʈ�� ��� ���� �� ����
        return _list;
    }

    void SelectBossItemIndex()
    {
        // �� ���Կ� ��ų �ε����� �ο�
        for (int i = 0; i < m_Slots.Length; i++)
        {
            m_Slots[i].m_SkillIndex = m_List[i];
        }
    }

    void SetItemText()
    {
        for (int i = 0; i < m_Slots.Length; i++)
        {
            // �� ���Կ� �ο��� m_SkillIndex�� ���� �ؽ�Ʈ ������ ������
            LoadLevelText(m_Slots[i].m_SkillIndex);

            m_Slots[i].m_Image.sprite = m_Sprites[m_Slots[i].m_SkillIndex];
            m_Slots[i].m_NameText.text = m_Info.name;
            m_Slots[i].m_DescriptionText.text = m_Info.description;
        }
    }

    void LoadLevelText(int _num)
    {
        // ����� Json ������ �ҷ���
        TextAsset text = Resources.Load<TextAsset>($"boss/{GetFileName(_num)}");

        // �ҷ��� Json ������ ��ȯ�ؼ� m_Info Ŭ������ �־���
        m_Info = JsonUtility.FromJson<BossClearInfo>(text.text);
    }
    string GetFileName(int _num)
    {
        // ������ ������ ���� ���� �̸��� ������
        return string.Format("boss{0:D2}", _num);
    }
}
