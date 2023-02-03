using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// json파일에서 정보를 가져와 담기 위한 클래스
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

    BossClearInfo m_Info = new BossClearInfo();                 // 아이템의 정보를 담을 클래스

    public bool[] m_IsNum;                                      // 중복 선택 방지를 위한 bool 배열

    private List<int> m_List;                                   // 스킬 인덱스를 담기 위한 리스트

    private int m_SkillCount = 9;                               // 현재 스킬 총 개수


    private void Awake()
    {
        // 자식 오브젝트의 Slot을 모두 가져옴
        m_Slots = GetComponentsInChildren<BossClearSlot>();

        // 스킬 총 개수만큼의 크기를 할당해줌
        m_List = new List<int>(m_SkillCount);

        // 스킬 개수만큼 리스트를 채워줌
        for (int i = 0; i < m_SkillCount; i++)
        {
            m_List.Add(i);
        }

        // 스킬 개수만큼 배열을 채워줌
        m_IsNum = new bool[m_List.Count];
        for (int i = 0; i < m_IsNum.Length; i++)
        {
            m_IsNum[i] = false;
        }
    }

    public void BossClear()
    {
        // 시간 정지
        Time.timeScale = 0;

        // 레벨업 UI를 활성화 시킴
        m_BossClearUI.SetActive(true);

        // 스킬 인덱스가 저장된 리스트에 만렙인 스킬이 있는지 확인 후 항목에서 제거
        BossItemCheck();

        // 리스트의 항목을 무작위로 정렬
        SuffleList(m_List);

        // 리스트의 0, 1, 2인덱스 값을 슬롯에 순서대로 넣어줌
        SelectBossItemIndex();

        // 슬롯의 텍스트 업데이트
        SetItemText();
    }
    void BossItemCheck()
    {
        // 리스트의 모든 항목을 돌면서 활성화된 보스 아이템이 있는지 확인
        for (int i = 0; i < m_List.Count; i++)
        {
            // 이미 존재하는 아이템은 리스트에서 삭제
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
        // 리스트에 항목이 2개 이상일때만 작동함
        for (int i = _list.Count - 1; i > 0; i--)
        {
            // 랜덤 숫자 저장
            // int형 랜덤범위이므로 0 ~ i-1까지의 범위
            int random = Random.Range(0, i);

            // 현재 인덱스i에 들어있는 _list[i]와 i미만의 무작위 인덱스random에 들어있는 _list[random]의 값을 서로 바꿔줌
            T temp = _list[i];
            _list[i] = _list[random];
            _list[random] = temp;
        }
        // 매개변수로 받아온 리스트를 모두 섞은 후 리턴
        return _list;
    }

    void SelectBossItemIndex()
    {
        // 각 슬롯에 스킬 인덱스를 부여
        for (int i = 0; i < m_Slots.Length; i++)
        {
            m_Slots[i].m_SkillIndex = m_List[i];
        }
    }

    void SetItemText()
    {
        for (int i = 0; i < m_Slots.Length; i++)
        {
            // 각 슬롯에 부여된 m_SkillIndex에 따른 텍스트 정보를 가져옴
            LoadLevelText(m_Slots[i].m_SkillIndex);

            m_Slots[i].m_Image.sprite = m_Sprites[m_Slots[i].m_SkillIndex];
            m_Slots[i].m_NameText.text = m_Info.name;
            m_Slots[i].m_DescriptionText.text = m_Info.description;
        }
    }

    void LoadLevelText(int _num)
    {
        // 저장된 Json 파일을 불러옴
        TextAsset text = Resources.Load<TextAsset>($"boss/{GetFileName(_num)}");

        // 불러온 Json 파일을 변환해서 m_Info 클래스에 넣어줌
        m_Info = JsonUtility.FromJson<BossClearInfo>(text.text);
    }
    string GetFileName(int _num)
    {
        // 지정된 포맷을 통해 파일 이름을 가져옴
        return string.Format("boss{0:D2}", _num);
    }
}
