using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Haruka : MonoBehaviour
{

    [SerializeField] Transform m_Gun, m_GunRightMuzzle, m_GunLeftMuzzle;                           // 플레이어 무기 오브젝트
    [SerializeField] LevelUpUI m_LevelUpUI;                     // 레벨 업 UI
    [SerializeField] GameObject m_Shield;                       // 플레이어 실드 오브젝트
    [SerializeField] GunManager m_GunManager;                   // 플레이어 무기 정보 매니저
    [SerializeField] Animator m_GunAni;                         // 플레이어 무기 애니메이터

    Animator m_Ani;                                             // 플레이어 애니메이터
    SpriteRenderer m_SpRen;                                     // 플레이어 스프라이트 렌더러
    Transform m_TargetTF;                                       // 가장 가까운 몬스터 트랜스폼

    float m_MoveSpeed;                                          // 플레이어 이동 속도
    int m_PlayerMaxHP;                                          // 플레이어 최대 체력
    int m_PlayerHP;                                             // 플레이어 현재 체력
    int m_PlayerMaxExp;                                         // 플레이어 최대 경험치
    int m_PlayerExp;                                            // 플레이어 현재 경험치
    int m_PlayerLevel;                                          // 플레이어 레벨
    int m_MoveLevel;                                            // 플레이어 이동 레벨
    int m_ShieldLevel;                                          // 플레이어 보호막 레벨

    // 캡슐화를 위한 get 프로퍼티
    public int MoveLevel => m_MoveLevel;
    public float MoveSpeed => m_MoveSpeed;
    public int ShieldLevel => m_ShieldLevel;
    public int PlayerMaxHP => m_PlayerMaxHP;
    public int PlayerHP => m_PlayerHP;
    public GunManager GunManager => m_GunManager;
    public bool IsLevelUp => m_IsLevelUp;
    public Transform TargetTF => m_TargetTF;

    bool m_IsSlide;                                             // 슬라이드 도중 다른 액션 방지
    bool m_IsSlideCool;                                         // 슬라이드 연속 사용 방지
    bool m_IsFire;                                              // 총 발사 연속 사용 방지
    bool m_IsHit;                                               // 연속 피격 방지
    bool m_IsSunglass;                                          // 선글라스 아이템 획득 유무
    bool m_IsLevelUp;                                           // 레벨업 UI 활성화 상태일 때 다른 액션 방지

    Vector3 m_DirVec;                                           // 현재 플레이어가 이동하는 방향(h + v)
    Vector2 m_MouseDir;                                         // 현재 마우스가 위치하는 좌표

    // 플레이어 관련 UI
    [SerializeField]
    private SpriteRenderer m_PlayerHpSpRen;                     // 체력바를 대신 할 헤일로 스프라이트 렌더러
    [SerializeField]
    private Slider m_PlayerExpBarSlider;                        // 경험치 바 슬라이더
    [SerializeField]
    private Text m_PlayerHealText;                              // 체력 회복량 텍스트
    [SerializeField]
    private GameObject m_PlayerHeal;                            // 체력 회복 텍스트 오브젝트

    private void Awake()
    {
        // 컴포넌트 초기화
        m_Ani = GetComponent<Animator>();
        m_SpRen = GetComponent<SpriteRenderer>();

        // 변수 초기화
        m_MoveSpeed = 2f;
        m_PlayerMaxHP = 100;
        m_PlayerHP = m_PlayerMaxHP;
        m_PlayerMaxExp = 5;
        m_PlayerExp = 0;
        m_PlayerLevel = 1;
        m_MoveLevel = 0;
        m_ShieldLevel = 0;

        // 슬라이더 UI 초기화
        SetHpBar();
        SetExpBar();

        // 실드의 활성화를 확인하기 위한 코루틴 실행
        StartCoroutine(ShieldCheck());
    }

    private void Update()
    {
        // 플레이어 이동 방향 설정
        //SetPlayerMoveDirection();

        // 타임 스케일이 0 일때는 작동하지 않음
        if (Time.timeScale == 0)
        {
            return;
        }
        // 방향에 따라 플레이어 이동
        PlayerMove();

        // 플레이어 구르기
        PlayerSlide();

        // 마우스 좌표에 따라 조준점 이동
        TakeAim();
            
        // 조준점을 향해 발사
        PlayerAttack();
    }

    // 플레이어 방향 설정
    //private void SetPlayerMoveDirection()
    //{
    //    float h = Input.GetAxisRaw("Horizontal");
    //    float v = Input.GetAxisRaw("Vertical");

    //    if (!m_IsSlide && !GameManager.Instance.IsMobile)
    //    {
    //        m_DirVec = new Vector3(h, v, 0).normalized;
    //    }
    //}
    private void OnMove(InputValue _value)
    {
        if (!m_IsSlide && !GameManager.Instance.IsMobile)
        {
            m_DirVec = _value.Get<Vector2>();
        }
    }
    // 조이스틱을 통한 플레이어 방향 설정
    public void SetDirection2(Vector3 _dirVec)
    {
        if (!m_IsSlide && GameManager.Instance.IsMobile)
        {
            m_DirVec = _dirVec.normalized;
        }
    }
    private void PlayerMove()
    {
        if (m_DirVec != Vector3.zero)
        {
            transform.position += m_DirVec * m_MoveSpeed * Time.deltaTime;
            m_Ani.SetBool("isWalk", true);
        }
        else
            m_Ani.SetBool("isWalk", false);
    }

    private void PlayerSlide()
    {
        // 이동중인 상태일때, 슬라이드가 쿨다운 상태가 아니라면 스페이스를 눌러 실행
        if(Input.GetKeyDown(KeyCode.Space) && !m_IsSlideCool && m_DirVec != Vector3.zero && !m_IsLevelUp 
            && !GameManager.Instance.IsMobile && Time.timeScale != 0)
        {
            StartCoroutine(SlideCoroutine());
        }
    }
    public void SlideBtn()
    {
        // 모바일 상태일 때, 슬라이드 버튼을 눌러 발동
        if (!m_IsSlideCool && m_DirVec != Vector3.zero && !m_IsLevelUp && GameManager.Instance.IsMobile 
            && Time.timeScale != 0)
        {
            StartCoroutine(SlideCoroutine());
        }
    }
    IEnumerator SlideCoroutine()
    {
        // 중복 실행 방지를 위한 값들을 true로 설정, 무기 오브젝트를 잠깐 비활성화 시킴
        m_IsSlide = true;
        m_IsSlideCool = true;
        m_Gun.gameObject.SetActive(false);

        // 이동속도를 잠깐 3배로 증가
        m_MoveSpeed *= 3f;

        // 이동 방향에 맞는 슬라이드 애니메이션 및 스프라이트 렌더러 flipX 실행(좌우 우선)
        if (m_DirVec.x >= 0.5f)
        {
            m_SpRen.flipX = false;
            m_Ani.SetTrigger("RightSlide");
        }
        else if (m_DirVec.x <= -0.5f)
        {
            m_SpRen.flipX = true;
            m_Ani.SetTrigger("RightSlide");
        }
        else if (m_DirVec.y > 0.5f)
        {
            m_Ani.SetTrigger("UpSlide");
        }
        else
        {
            m_Ani.SetTrigger("DownSlide");
        }

        // 이동속도 원상복구
        yield return new WaitForSeconds(0.2f);
        m_MoveSpeed /= 3f;

        // 다른 액션 방지 false, 총 오브젝트 활성화
        yield return new WaitForSeconds(0.1f);
        m_IsSlide = false;
        m_Gun.gameObject.SetActive(true);

        // 슬라이드 중복 실행 방지 false
        yield return new WaitForSeconds(0.2f);
        m_IsSlideCool = false;
    }

    // 마우스가 위치하는 방향에 따라 플레이어와 플레이어가 들고있는 총을 회전시키는 함수
    private void TakeAim()
    {
        if (m_IsSlide)
            return;

        SetMouseDirection();

        Vector3 dir = (m_Gun.position - (Vector3)m_MouseDir).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

        if (IsGunDirectionRight(angle))
        {
            PlayerSpriteRendererFlipX(false);
            m_Gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            PlayerSpriteRendererFlipX(true);
            m_Gun.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
        }

    }
    bool IsGunDirectionRight(float _angle)
    {
        return ((_angle > 0 && _angle < 90) || (_angle > 270 && _angle < 360));
    }
    void PlayerSpriteRendererFlipX(bool _bool)
    {
        // 플레이어와 총의 flipX를 false
        m_SpRen.flipX = _bool;

        SpriteRenderer gunSpRen = m_Gun.gameObject.GetComponent<SpriteRenderer>();
        gunSpRen.flipX = _bool;
    }

    // 마우스 포지션을 구함
    private void SetMouseDirection()
    {
        if (!GameManager.Instance.IsMobile)
        {
            // 마우스의 좌표를 받아온 후, 플레이어의 포지션에서 마우스의 좌표를 향한 방향 벡터를 구함
            m_MouseDir = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, -Input.mousePosition.z));
        }
        else
        {
            if (m_TargetTF != null)
            {
                // 가장 가까운 적의 위치를 넣어줌
                m_MouseDir = m_TargetTF.position;
            }
        }
    }
    private void PlayerAttack()
    {
        // 모바일인지 확인
        // 모바일이 아닐 경우
        if (!GameManager.Instance.IsMobile)
        {
            // 조건을 만족하면 마우스 왼쪽클릭으로 실행
            if (Input.GetMouseButton(0) && !m_IsSlide && !m_IsFire 
             && !m_IsLevelUp && !EventSystem.current.IsPointerOverGameObject())
            {
                StartCoroutine(PlayerGunFireCoroutine());
            }
        }
        // 모바일인 경우
        else
        {
            if (CanFire())
            {
                StartCoroutine(PlayerGunFireCoroutine());
            }
        }
    }
    bool CanFire()
    {
        return (!m_IsSlide && m_Gun.gameObject.activeSelf && !m_IsFire
             && !m_IsLevelUp && m_TargetTF != null);
    }
    IEnumerator PlayerGunFireCoroutine()
    {
        // 연속 사용 방지 true
        m_IsFire = true;

        // 확률 관통 랜덤값
        int ran = Random.Range(0, 5);

        for (int i = 0; i < m_GunManager.FireRate + m_GunManager.FireRate2; i++)
        {
            // 오브젝트 풀링 매니저에서 플레이어 불렛을 꺼내옴
            GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerBulletKey);

            // 플레이어 불렛에 데미지, 관통, 확률관통 부여
            PlayerBullet pBullet = bullet.GetComponent<PlayerBullet>();
            SetBulletDamage(pBullet, ran);

            // 플레이어 불렛의 시작 포지션을 마우스가 향하는 방향에 따라 우측 총구, 혹은 좌측 총구로 이동
            bullet.transform.position = m_MouseDir.x - transform.position.x >= 0 ? m_GunRightMuzzle.position : m_GunLeftMuzzle.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = (m_MouseDir - (Vector2)m_Gun.transform.position).normalized;
            rigid.velocity = SetBulletDir(dir) * 12f;
        }

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERSHOT);

        m_GunAni.SetTrigger("Attack");

        // 공격 딜레이만큼 대기
        yield return new WaitForSeconds(m_GunManager.FireDelay);

        // 연속 사용 방지 false
        m_IsFire = false;
    }
    void SetBulletDamage(PlayerBullet _pBullet, int _ran)
    {
        _pBullet.SetDamage(m_GunManager.IsDamageUp ? (int)(m_GunManager.Damage * 1.5f) : m_GunManager.Damage);

        // m_Penetration: 보스 클리어 보상으로 얻는 확정 1회 관통
        _pBullet.SetPenetration(m_GunManager.Penetration);

        // m_Penetration2: 레벨업 보상으로 얻는 확률 1회 관통
        _pBullet.SetPenetration(_ran < m_GunManager.Penetration2 ? 1 : 0);
    }
    Vector2 SetBulletDir(Vector2 dir)
    {
        // 발사하려는 위치의 방향 벡터중 x, y의 절댓값이 일정 이하라면 랜덤한 수치를 더해서 범위가 너무 좁아지지 않도록 설정
        dir.x = Mathf.Abs(dir.x) < 0.2f ? dir.x + (Random.Range(-0.4f, 0.4f) * m_GunManager.Accuracy) : dir.x;
        dir.y = Mathf.Abs(dir.y) < 0.2f ? dir.y + (Random.Range(-0.4f, 0.4f) * m_GunManager.Accuracy) : dir.y;

        dir.x *= Random.Range(1 - m_GunManager.Accuracy, 1 + m_GunManager.Accuracy);
        dir.y *= Random.Range(1 - m_GunManager.Accuracy, 1 + m_GunManager.Accuracy);

        return dir.normalized;
    }
    // 데미지 받음 함수
    public void PlayerOnDamage(int _damage)
    {
        // 슬라이드 도중은 무적판정
        if (m_IsSlide || m_IsHit)
            return;

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERHIT);

        // 실드가 활성화된 상태라면 실드가 먼저 비활성화됨
        if (m_Shield.activeSelf)
        {
            m_Shield.SetActive(false);
            return;
        }

        // 중복 피격 방지 true
        m_IsHit = true;

        // 보스 클리어 보상인 선글라스를 획득한 상태라면 받는 데미지에 0.7을 곱해줌
        m_PlayerHP -= m_IsSunglass ? (int)(_damage * 0.7f) : _damage;

        // 체력바 슬라이더 세팅
        SetHpBar();

        // 남은 체력이 0 이하라면 게임 오버
        if (m_PlayerHP <= 0)
        {
            GameManager.Instance.GameOver();
        }

        // 피격 시 색상 변경
        m_SpRen.color = Color.grey;

        // 기존 색상으로 복구
        StartCoroutine(ColorReset());
    }
    private IEnumerator ColorReset()
    {
        // 0.2초 대기
        yield return new WaitForSeconds(0.2f);

        // 중복 피격 방지 false
        m_IsHit = false;

        // 원래 색상으로 복구
        m_SpRen.color = Color.white;

        yield return new WaitForSeconds(0.3f);
    }

    public void SetIsLevelUp(bool _bool)
    {
        m_IsLevelUp = _bool;
    }

    // 경험치 증가 함수
    public void SetExpUp(int _exp)
    {
        m_PlayerExp += _exp;

        // Exp UI 설정
        SetExpBar();

        // 스코어 증가
        GameManager.Instance.ScoreUp(_exp);

        // 최대 경험치 이상이라면 레벨업
        if (m_PlayerExp >= m_PlayerMaxExp)
        {
            PlayerLevelUp();
        }
    }
    // 레벨 업 함수
    void PlayerLevelUp()
    {
        // 레벨 증가
        m_PlayerLevel++;

        // 현재 경험치 초기화
        m_PlayerExp = 0;

        // 최대 경험치량 증가
        m_PlayerMaxExp = 5 + (int)Mathf.Pow(m_PlayerLevel, 1.7f);

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERLEVELUP);

        // Exp UI 설정
        SetExpBar();

        // 현재 레벨업 상태
        m_IsLevelUp = true;

        m_LevelUpUI.LevelUp();
        GameManager.Instance.OpenStatus();
    }

    public void SetMaxHpUp(int _maxHp)
    {
        // 수치만큼 최대 체력 증가
        m_PlayerMaxHP += _maxHp;

        // 증가한 최대 체력만큼 회복
        SetHpUp(_maxHp);
    }
    public void SetHpUp(int _hp)
    {
        // 수치만큼 체력 회복
        m_PlayerHP += _hp;

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERHEAL);

        // 현재 체력이 최대 체력보다 크다면 현재 체력을 최대 체력으로 고정
        if (m_PlayerHP > m_PlayerMaxHP)
        {
            m_PlayerHP = m_PlayerMaxHP;
        }

        // Hp UI 설정
        SetHpBar();
        SetHealText(_hp);
    }

    void SetExpBar()
    {
        // 경험치 바 UI 값 설정
        m_PlayerExpBarSlider.maxValue = m_PlayerMaxExp;
        m_PlayerExpBarSlider.value = m_PlayerExp;
    }

    void SetHpBar()
    {
        // 체력이 0보다 크거나 같다면 m_playerHp를, 0보다 작거나 같다면 1을 넣어줌
        m_PlayerHpSpRen.color = m_PlayerHP > 0 ? new Color(1, 1, 1, (float)m_PlayerHP / (float)m_PlayerMaxHP) : new Color(1, 1, 1, 1);
    }

    // 힐 텍스트 설정
    void SetHealText(int _hp)
    {
        // 텍스트를 힐량으로 설정
        m_PlayerHealText.text = $"+{_hp}";

        // 텍스트 오브젝트가 활성화 되어있다면 먼저 비활성화 처리
        if (m_PlayerHeal.activeSelf)
        {
            m_PlayerHeal.SetActive(false);
        }
        m_PlayerHeal.SetActive(true);
    }

    public void SetMoveUp()
    {
        // 이동속도 증가
        m_MoveSpeed += 0.2f;
        m_MoveLevel++;
    }
    public void SetMoveUp2()
    {
        // 이동속도 증가 2
        m_MoveSpeed += 1f;
    }
    public void SunglassSlowStart()
    {
        // 선글라스 획득 시 true
        m_IsSunglass = true;
    }
    public void SetShieldUp()
    {
        // 실드 레벨 증가
        m_ShieldLevel++;
        ShieldRegen();
    }
    public void HealKitStart()
    {
        StartCoroutine(HealKitUp());
    }

    // 실드 활성화 코루틴
    IEnumerator ShieldCheck()
    {
        yield return null;
        // 게임이 진행중인 동안 반복 확인
        while (GameManager.Instance.IsPlay)
        {
            // 실드가 비활성화 된 경우
            if (!m_Shield.activeSelf)
            {
                if (m_ShieldLevel >= 1)
                {
                    // 일정 시간 후 실드 활성화
                    yield return new WaitForSeconds(30 - (4 * m_ShieldLevel));
                    ShieldRegen();
                }
            }
            yield return null;
        }
    }
    void ShieldRegen()
    {
        m_Shield.SetActive(true);
    }

    // 구급 상자 활성화 코루틴
    IEnumerator HealKitUp()
    {
        // 게임이 진행중인 동안 반복 확인
        while (GameManager.Instance.IsPlay)
        {
            // 10초마다 최대체력의 5%만큼 회복
            SetHpUp((int)(m_PlayerMaxHP * 0.05f));
            yield return new WaitForSeconds(10f);
        }
    }
    public void SetTargetTF(Transform _tf)
    {
        m_TargetTF = _tf;
    }
}
