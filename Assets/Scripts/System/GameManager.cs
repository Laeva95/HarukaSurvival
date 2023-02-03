using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 게임 매니저 변수, 프로퍼티
    // 단일 객체만 유지하고, 다른 클래스에서 쉽게 사용할 수 있도록 싱글톤 패턴을 사용
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    [SerializeField]
    private Haruka m_Player;
    [SerializeField]
    private Animator m_HaloAni;
    [SerializeField]
    private BossClearUI m_BossClearUI;
    [SerializeField]
    private SkillManager m_SkillManager;

    // 게임 오버 UI
    [SerializeField]
    private GameObject m_GameOverUI;
    // 게임 클리어 UI
    [SerializeField]
    private GameObject m_GameClearUI;

    // 게임 중단 시 활성화되는 퍼즈 UI 게임 오브젝트
    [SerializeField]
    private GameObject m_PauseUI;
    // 누르면 퍼즈상태가 되는 버튼
    [SerializeField]
    private GameObject m_PauseBtn;
    // 게임 재시작을 물어보기 위한 UI 패널
    [SerializeField]
    private GameObject m_RestartPanel;
    // 모바일 조이스틱 셋
    [SerializeField]
    private GameObject m_MobileJoystick;
    // 모바일 회피 버튼
    [SerializeField]
    private GameObject m_MobileSlideBtn;
    // 스테이터스 UI 셋
    [SerializeField]
    private GameObject m_StatusUI;
    // 스테이터스 텍스트 패널
    [SerializeField]
    private GameObject m_StatusPanel;
    private Text m_StatusText;
    // 게임 스코어 텍스트
    [SerializeField]
    private Text m_ScoreText;

    private bool m_IsBossTime;
    private bool m_IsBossClear;
    private bool m_IsPlay;
    private bool m_IsSlow;
    private bool m_IsMobile;
    private float m_PlayTime;
    private int m_Score;
    private RectTransform m_StatusRect;

    // 캡슐화를 위한 get 프로퍼티
    public bool IsBoss => m_IsBossTime;
    public bool IsBossClear => m_IsBossClear;
    public bool IsPlay => m_IsPlay;
    public bool IsSlow => m_IsSlow;
    public bool IsMobile => m_IsMobile;
    public float PlayTime => m_PlayTime;

    private void Awake()
    {
        if (instance == null)
        {
            // instance가 비어있을 경우 해당 객체를 넣어줌
            instance = this;
        }

        m_IsPlay = true;
        m_IsBossTime = false;
        m_IsBossClear = false;
        m_IsSlow = false;
        m_IsMobile = false;
        m_PlayTime = 0;
        m_StatusText = m_StatusPanel.GetComponentInChildren<Text>();
        m_StatusRect = m_StatusPanel.GetComponent<RectTransform>();

        m_IsMobile = Application.isMobilePlatform;
        //StartCoroutine(SetResoutionCoroutine());
    }

    private void Start()
    {
        m_MobileSlideBtn.SetActive(m_IsMobile);
        m_MobileJoystick.SetActive(m_IsMobile);
    }
    private void Update()
    {
        CalPlayTime();

        Pause();
        if (!IsPlay && Input.GetMouseButtonDown(0))
        {
            GameRestart();
        }
    }
    public void ScoreUp(int _score)
    {
        m_Score += _score;
        m_ScoreText.text = $"Score: {m_Score}";
    }
    // 진행 시간 계산 함수
    void CalPlayTime()
    {
        // 시간 스케일이 0이거나, 보스가 나온 상태라면 정지
        if (Time.timeScale == 0 || m_IsBossTime)
        {
            return;
        }

        // 매 프레임 deltaTime을 더해줌
        m_PlayTime += Time.deltaTime;
    }

    // 보스가 스폰될 때 호출되는 함수
    public void BossSpawn()
    {
        m_IsBossTime = true;
    }

    // 보스가 죽었을 때 호출되는 함수
    public void BossClear()
    {
        StartCoroutine(BossClearCoroutine());
    }
    IEnumerator BossClearCoroutine()
    {
        // 관련 변수 설정
        m_IsBossClear = true;
        m_IsBossTime = false;

        // 사운드 재생
        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERLEVELUP);

        // 보스 클리어 UI의 BossClear 실행
        m_BossClearUI.BossClear();
        OpenStatus();

        // 플레이 시간을 2 추가하여 보스 중복 소환 방지
        m_PlayTime += 2;

        yield return new WaitForSeconds(2f);
    }

    // 보스 클리어 상태를 들어온 bool 값으로 설정
    public void SetBossClear(bool _bool)
    {
        m_IsBossClear = _bool;
    }

    // 게임 오버 실행 함수
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        // 게임이 끝났으므로 timeScale을 0으로 설정
        Time.timeScale = 0;

        // 카메라 연출
        // 매초 orthographicSize를 줄여 캐릭터가 가까워지는 연출
        Camera.main.orthographicSize = 4;
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER0);
        yield return new WaitForSecondsRealtime(1f);

        Camera.main.orthographicSize = 3;
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER0);
        yield return new WaitForSecondsRealtime(1f);

        Camera.main.orthographicSize = 2;
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER0);
        yield return new WaitForSecondsRealtime(1f);

        // 머리 위의 Halo가 파괴되는 애니메이션 재생
        m_HaloAni.SetTrigger("Break");
        yield return new WaitForSecondsRealtime(1.5f);

        // IsPlay를 false로 설정, 게임 오버 UI 활성화
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER1);
        m_GameOverUI.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        m_IsPlay = false;
    }

    // 게임 클리어 실행 함수
    public void GameClear()
    {
        StartCoroutine(GameClearCoroutine());
    }

    IEnumerator GameClearCoroutine()
    {
        // 게임이 끝났으므로 timeScale을 0으로 설정
        Time.timeScale = 0;

        // 클리어 사운드 재생 후 게임 클리어 UI 활성화
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMECLEAR);

        yield return new WaitForSecondsRealtime(2f);

        m_GameClearUI.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        // 게임이 더 진행되지 않도록 IsPlay = false
        m_IsPlay = false;
    }

    private void Pause()
    {
        // 게임이 플레이 중일때, esc를 누를 경우
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }
    }

    // 버튼으로도 호출 가능하도록 public으로 작성
    public void OnPause()
    {
        // 게임이 진행중이 아니라면 리턴
        if (!IsPlay || m_PlayTime < 1 || m_Player.PlayerHP <= 0)
            return;

        // 현재 퍼즈 상태가 아닐 경우
        if (!m_PauseUI.activeSelf)
        {
            // 퍼즈 UI 활성화 및 퍼즈 버튼 비활성화, timeScale을 0으로 설정
            m_PauseUI.SetActive(true);
            m_PauseBtn.SetActive(false);
            OpenStatus();
            Time.timeScale = 0;
        }
        // 현재 퍼즈 상태일 경우
        else
        {
            // 퍼즈 UI 비활성화 및 퍼즈 버튼 활성화, 재시작 패널 비활성화
            m_PauseUI.SetActive(false);
            m_PauseBtn.SetActive(true);
            m_RestartPanel.SetActive(false);
            // isLevelUp, isBoss 모두 false일때만 timeScale을 1로 되돌림
            if (!m_Player.IsLevelUp && !IsBossClear)
            {
                Time.timeScale = 1;
                CloseStatus();
            }
        }

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }

    // 버튼으로도 호출할 수 있도록 public으로 작성
    public void OnGameRestart()
    {
        // 게임 재시작 패널을 호출, 취소하는 함수
        if (!m_RestartPanel.activeSelf)
        {
            m_RestartPanel.SetActive(true);
        }
        else
        {
            m_RestartPanel.SetActive(false);
        }

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }
    // 버튼으로도 호출할 수 있도록 public으로 작성
    public void GameRestart()
    {
        // 게임이 끝났을 때 다시 시작하기 위한 함수
        // timeScale을 1로 되돌린 후 main Scene을 불러옴
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // 슬로우 효과 On 함수
    public void SetSlow()
    {
        m_IsSlow = true;
    }
    // 스테이터스 UI 열기
    public void OpenStatus()
    {
        m_StatusUI.SetActive(true);
    }
    // 스테이터스 UI 닫기
    public void CloseStatus()
    {
        m_StatusUI.SetActive(false);
        m_StatusPanel.SetActive(false);
    }

    // 아이콘 클릭 시 스테이터스 패널 호출
    public void ClickStatusPanel(STATUS _status, RectTransform _pos)
    {
        // 스테이터스 텍스트 업데이트
        StatusPanelUpdate(_status);

        // 스테이터스 패널 오브젝트 활성화, 포지션 설정
        m_StatusPanel.SetActive(true);
        m_StatusRect.position = _pos.position;
        m_StatusPanel.transform.position = m_StatusPanel.transform.position + Vector3.up * 1;
    }
    // 스테이터스 패널 텍스트 업데이트
    private void StatusPanelUpdate(STATUS _status)
    {
        switch (_status)
        {
            case STATUS.HARUKA:
                m_StatusText.text = $"공격력: {m_Player.GunManager.Damage}\n체력: {m_Player.PlayerHP} / {m_Player.PlayerMaxHP}\n시야: {Camera.main.orthographicSize}\n이동속도: {m_Player.MoveSpeed}";
                break;
            case STATUS.GUN:
                int tempI = m_Player.GunManager.IsDamageUp ? (int)(m_Player.GunManager.Damage * 1.5f) : m_Player.GunManager.Damage;
                string tempS = string.Format("{0:N2}", m_Player.GunManager.FireDelay);
                m_StatusText.text = $"데미지: {tempI}\n발사 개수: {m_Player.GunManager.FireRate + m_Player.GunManager.FireRate2}\n쿨다운: {tempS}초\n관통확률: {m_Player.GunManager.Penetration2 * 20}%";
                break;
            case STATUS.MUTSUKI:
                m_StatusText.text = m_SkillManager.Skill02.Level > 0 ?
                    $"레벨: {m_SkillManager.Skill02.Level}\n데미지: {(int)(m_Player.GunManager.Damage * (1.5f + m_SkillManager.Skill02.Level * 1.5f))}\n쿨다운: {12f - (m_SkillManager.Skill02.Level * 1f)}초\n범위: {2}"
                    :$"레벨: 0";
                break;
            case STATUS.ARU:
                m_StatusText.text = m_SkillManager.Skill01.Level > 0 ?
                    $"레벨: {m_SkillManager.Skill01.Level}\n데미지: {(int)(m_Player.GunManager.Damage * (2f + m_SkillManager.Skill01.Level * 2f))}\n쿨다운: {11.5f - (m_SkillManager.Skill01.Level * 1f)}초\n범위: {2.5}"
                    :$"레벨: 0";
                break;
            case STATUS.KAYOKO:
                m_StatusText.text = m_SkillManager.Skill00.Level > 0 ?
                    $"레벨: {m_SkillManager.Skill00.Level}\n데미지: {(int)(m_Player.GunManager.Damage * (1f + m_SkillManager.Skill00.Level * 0.3f))}\n쿨다운: {4f - (0.3f * m_SkillManager.Skill00.Level)}초\n범위: {2.5f + (m_SkillManager.Skill00.Level * 0.15f)}"
                    :$"레벨: 0";
                break;
            case STATUS.HINA:
                m_StatusText.text = m_SkillManager.Skill03.Level > 0 ?
                    $"레벨: {m_SkillManager.Skill03.Level}\n데미지: {(int)(m_Player.GunManager.Damage * (1f + m_SkillManager.Skill03.Level * 0.2f))}\n쿨다운: {17f - (m_SkillManager.Skill03.Level * 1f)}초\n발사 개수: {(m_SkillManager.Skill03.Level * 4) + 40}"
                    : $"레벨: 0";
                break;
            case STATUS.SHIELD:
                m_StatusText.text = m_Player.ShieldLevel > 0 ?
                    $"레벨: {m_Player.ShieldLevel}\n쿨다운: {30 - (4 * m_Player.ShieldLevel)}초"
                    :$"레벨: 0";
                break;
            default:
                break;
        }
    }

    void SetResolution()
    {
        int setWidth = 1600; // 사용자 설정 너비
        int setHeight = 900; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), false); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    IEnumerator SetResoutionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        SetResolution();
    }
}
