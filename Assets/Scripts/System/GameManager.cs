using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���� �Ŵ��� ����, ������Ƽ
    // ���� ��ü�� �����ϰ�, �ٸ� Ŭ�������� ���� ����� �� �ֵ��� �̱��� ������ ���
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

    // ���� ���� UI
    [SerializeField]
    private GameObject m_GameOverUI;
    // ���� Ŭ���� UI
    [SerializeField]
    private GameObject m_GameClearUI;

    // ���� �ߴ� �� Ȱ��ȭ�Ǵ� ���� UI ���� ������Ʈ
    [SerializeField]
    private GameObject m_PauseUI;
    // ������ ������°� �Ǵ� ��ư
    [SerializeField]
    private GameObject m_PauseBtn;
    // ���� ������� ����� ���� UI �г�
    [SerializeField]
    private GameObject m_RestartPanel;
    // ����� ���̽�ƽ ��
    [SerializeField]
    private GameObject m_MobileJoystick;
    // ����� ȸ�� ��ư
    [SerializeField]
    private GameObject m_MobileSlideBtn;
    // �������ͽ� UI ��
    [SerializeField]
    private GameObject m_StatusUI;
    // �������ͽ� �ؽ�Ʈ �г�
    [SerializeField]
    private GameObject m_StatusPanel;
    private Text m_StatusText;
    // ���� ���ھ� �ؽ�Ʈ
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

    // ĸ��ȭ�� ���� get ������Ƽ
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
            // instance�� ������� ��� �ش� ��ü�� �־���
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
    // ���� �ð� ��� �Լ�
    void CalPlayTime()
    {
        // �ð� �������� 0�̰ų�, ������ ���� ���¶�� ����
        if (Time.timeScale == 0 || m_IsBossTime)
        {
            return;
        }

        // �� ������ deltaTime�� ������
        m_PlayTime += Time.deltaTime;
    }

    // ������ ������ �� ȣ��Ǵ� �Լ�
    public void BossSpawn()
    {
        m_IsBossTime = true;
    }

    // ������ �׾��� �� ȣ��Ǵ� �Լ�
    public void BossClear()
    {
        StartCoroutine(BossClearCoroutine());
    }
    IEnumerator BossClearCoroutine()
    {
        // ���� ���� ����
        m_IsBossClear = true;
        m_IsBossTime = false;

        // ���� ���
        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERLEVELUP);

        // ���� Ŭ���� UI�� BossClear ����
        m_BossClearUI.BossClear();
        OpenStatus();

        // �÷��� �ð��� 2 �߰��Ͽ� ���� �ߺ� ��ȯ ����
        m_PlayTime += 2;

        yield return new WaitForSeconds(2f);
    }

    // ���� Ŭ���� ���¸� ���� bool ������ ����
    public void SetBossClear(bool _bool)
    {
        m_IsBossClear = _bool;
    }

    // ���� ���� ���� �Լ�
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        // ������ �������Ƿ� timeScale�� 0���� ����
        Time.timeScale = 0;

        // ī�޶� ����
        // ���� orthographicSize�� �ٿ� ĳ���Ͱ� ��������� ����
        Camera.main.orthographicSize = 4;
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER0);
        yield return new WaitForSecondsRealtime(1f);

        Camera.main.orthographicSize = 3;
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER0);
        yield return new WaitForSecondsRealtime(1f);

        Camera.main.orthographicSize = 2;
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER0);
        yield return new WaitForSecondsRealtime(1f);

        // �Ӹ� ���� Halo�� �ı��Ǵ� �ִϸ��̼� ���
        m_HaloAni.SetTrigger("Break");
        yield return new WaitForSecondsRealtime(1.5f);

        // IsPlay�� false�� ����, ���� ���� UI Ȱ��ȭ
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMEOVER1);
        m_GameOverUI.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        m_IsPlay = false;
    }

    // ���� Ŭ���� ���� �Լ�
    public void GameClear()
    {
        StartCoroutine(GameClearCoroutine());
    }

    IEnumerator GameClearCoroutine()
    {
        // ������ �������Ƿ� timeScale�� 0���� ����
        Time.timeScale = 0;

        // Ŭ���� ���� ��� �� ���� Ŭ���� UI Ȱ��ȭ
        SoundManager.Instance.SoundPlay(SOUND_NAME.GAMECLEAR);

        yield return new WaitForSecondsRealtime(2f);

        m_GameClearUI.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        // ������ �� ������� �ʵ��� IsPlay = false
        m_IsPlay = false;
    }

    private void Pause()
    {
        // ������ �÷��� ���϶�, esc�� ���� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }
    }

    // ��ư���ε� ȣ�� �����ϵ��� public���� �ۼ�
    public void OnPause()
    {
        // ������ �������� �ƴ϶�� ����
        if (!IsPlay || m_PlayTime < 1 || m_Player.PlayerHP <= 0)
            return;

        // ���� ���� ���°� �ƴ� ���
        if (!m_PauseUI.activeSelf)
        {
            // ���� UI Ȱ��ȭ �� ���� ��ư ��Ȱ��ȭ, timeScale�� 0���� ����
            m_PauseUI.SetActive(true);
            m_PauseBtn.SetActive(false);
            OpenStatus();
            Time.timeScale = 0;
        }
        // ���� ���� ������ ���
        else
        {
            // ���� UI ��Ȱ��ȭ �� ���� ��ư Ȱ��ȭ, ����� �г� ��Ȱ��ȭ
            m_PauseUI.SetActive(false);
            m_PauseBtn.SetActive(true);
            m_RestartPanel.SetActive(false);
            // isLevelUp, isBoss ��� false�϶��� timeScale�� 1�� �ǵ���
            if (!m_Player.IsLevelUp && !IsBossClear)
            {
                Time.timeScale = 1;
                CloseStatus();
            }
        }

        SoundManager.Instance.SoundPlay(SOUND_NAME.UI);
    }

    // ��ư���ε� ȣ���� �� �ֵ��� public���� �ۼ�
    public void OnGameRestart()
    {
        // ���� ����� �г��� ȣ��, ����ϴ� �Լ�
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
    // ��ư���ε� ȣ���� �� �ֵ��� public���� �ۼ�
    public void GameRestart()
    {
        // ������ ������ �� �ٽ� �����ϱ� ���� �Լ�
        // timeScale�� 1�� �ǵ��� �� main Scene�� �ҷ���
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // ���ο� ȿ�� On �Լ�
    public void SetSlow()
    {
        m_IsSlow = true;
    }
    // �������ͽ� UI ����
    public void OpenStatus()
    {
        m_StatusUI.SetActive(true);
    }
    // �������ͽ� UI �ݱ�
    public void CloseStatus()
    {
        m_StatusUI.SetActive(false);
        m_StatusPanel.SetActive(false);
    }

    // ������ Ŭ�� �� �������ͽ� �г� ȣ��
    public void ClickStatusPanel(STATUS _status, RectTransform _pos)
    {
        // �������ͽ� �ؽ�Ʈ ������Ʈ
        StatusPanelUpdate(_status);

        // �������ͽ� �г� ������Ʈ Ȱ��ȭ, ������ ����
        m_StatusPanel.SetActive(true);
        m_StatusRect.position = _pos.position;
        m_StatusPanel.transform.position = m_StatusPanel.transform.position + Vector3.up * 1;
    }
    // �������ͽ� �г� �ؽ�Ʈ ������Ʈ
    private void StatusPanelUpdate(STATUS _status)
    {
        switch (_status)
        {
            case STATUS.HARUKA:
                m_StatusText.text = $"���ݷ�: {m_Player.GunManager.Damage}\nü��: {m_Player.PlayerHP} / {m_Player.PlayerMaxHP}\n�þ�: {Camera.main.orthographicSize}\n�̵��ӵ�: {m_Player.MoveSpeed}";
                break;
            case STATUS.GUN:
                int tempI = m_Player.GunManager.IsDamageUp ? (int)(m_Player.GunManager.Damage * 1.5f) : m_Player.GunManager.Damage;
                string tempS = string.Format("{0:N2}", m_Player.GunManager.FireDelay);
                m_StatusText.text = $"������: {tempI}\n�߻� ����: {m_Player.GunManager.FireRate + m_Player.GunManager.FireRate2}\n��ٿ�: {tempS}��\n����Ȯ��: {m_Player.GunManager.Penetration2 * 20}%";
                break;
            case STATUS.MUTSUKI:
                m_StatusText.text = m_SkillManager.Skill02.Level > 0 ?
                    $"����: {m_SkillManager.Skill02.Level}\n������: {(int)(m_Player.GunManager.Damage * (1.5f + m_SkillManager.Skill02.Level * 1.5f))}\n��ٿ�: {12f - (m_SkillManager.Skill02.Level * 1f)}��\n����: {2}"
                    :$"����: 0";
                break;
            case STATUS.ARU:
                m_StatusText.text = m_SkillManager.Skill01.Level > 0 ?
                    $"����: {m_SkillManager.Skill01.Level}\n������: {(int)(m_Player.GunManager.Damage * (2f + m_SkillManager.Skill01.Level * 2f))}\n��ٿ�: {11.5f - (m_SkillManager.Skill01.Level * 1f)}��\n����: {2.5}"
                    :$"����: 0";
                break;
            case STATUS.KAYOKO:
                m_StatusText.text = m_SkillManager.Skill00.Level > 0 ?
                    $"����: {m_SkillManager.Skill00.Level}\n������: {(int)(m_Player.GunManager.Damage * (1f + m_SkillManager.Skill00.Level * 0.3f))}\n��ٿ�: {4f - (0.3f * m_SkillManager.Skill00.Level)}��\n����: {2.5f + (m_SkillManager.Skill00.Level * 0.15f)}"
                    :$"����: 0";
                break;
            case STATUS.HINA:
                m_StatusText.text = m_SkillManager.Skill03.Level > 0 ?
                    $"����: {m_SkillManager.Skill03.Level}\n������: {(int)(m_Player.GunManager.Damage * (1f + m_SkillManager.Skill03.Level * 0.2f))}\n��ٿ�: {17f - (m_SkillManager.Skill03.Level * 1f)}��\n�߻� ����: {(m_SkillManager.Skill03.Level * 4) + 40}"
                    : $"����: 0";
                break;
            case STATUS.SHIELD:
                m_StatusText.text = m_Player.ShieldLevel > 0 ?
                    $"����: {m_Player.ShieldLevel}\n��ٿ�: {30 - (4 * m_Player.ShieldLevel)}��"
                    :$"����: 0";
                break;
            default:
                break;
        }
    }

    void SetResolution()
    {
        int setWidth = 1600; // ����� ���� �ʺ�
        int setHeight = 900; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), false); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

    IEnumerator SetResoutionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        SetResolution();
    }
}
