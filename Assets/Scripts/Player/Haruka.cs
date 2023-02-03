using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Haruka : MonoBehaviour
{

    [SerializeField] Transform m_Gun, m_GunRightMuzzle, m_GunLeftMuzzle;                           // �÷��̾� ���� ������Ʈ
    [SerializeField] LevelUpUI m_LevelUpUI;                     // ���� �� UI
    [SerializeField] GameObject m_Shield;                       // �÷��̾� �ǵ� ������Ʈ
    [SerializeField] GunManager m_GunManager;                   // �÷��̾� ���� ���� �Ŵ���
    [SerializeField] Animator m_GunAni;                         // �÷��̾� ���� �ִϸ�����

    Animator m_Ani;                                             // �÷��̾� �ִϸ�����
    SpriteRenderer m_SpRen;                                     // �÷��̾� ��������Ʈ ������
    Transform m_TargetTF;                                       // ���� ����� ���� Ʈ������

    float m_MoveSpeed;                                          // �÷��̾� �̵� �ӵ�
    int m_PlayerMaxHP;                                          // �÷��̾� �ִ� ü��
    int m_PlayerHP;                                             // �÷��̾� ���� ü��
    int m_PlayerMaxExp;                                         // �÷��̾� �ִ� ����ġ
    int m_PlayerExp;                                            // �÷��̾� ���� ����ġ
    int m_PlayerLevel;                                          // �÷��̾� ����
    int m_MoveLevel;                                            // �÷��̾� �̵� ����
    int m_ShieldLevel;                                          // �÷��̾� ��ȣ�� ����

    // ĸ��ȭ�� ���� get ������Ƽ
    public int MoveLevel => m_MoveLevel;
    public float MoveSpeed => m_MoveSpeed;
    public int ShieldLevel => m_ShieldLevel;
    public int PlayerMaxHP => m_PlayerMaxHP;
    public int PlayerHP => m_PlayerHP;
    public GunManager GunManager => m_GunManager;
    public bool IsLevelUp => m_IsLevelUp;
    public Transform TargetTF => m_TargetTF;

    bool m_IsSlide;                                             // �����̵� ���� �ٸ� �׼� ����
    bool m_IsSlideCool;                                         // �����̵� ���� ��� ����
    bool m_IsFire;                                              // �� �߻� ���� ��� ����
    bool m_IsHit;                                               // ���� �ǰ� ����
    bool m_IsSunglass;                                          // ���۶� ������ ȹ�� ����
    bool m_IsLevelUp;                                           // ������ UI Ȱ��ȭ ������ �� �ٸ� �׼� ����

    Vector3 m_DirVec;                                           // ���� �÷��̾ �̵��ϴ� ����(h + v)
    Vector2 m_MouseDir;                                         // ���� ���콺�� ��ġ�ϴ� ��ǥ

    // �÷��̾� ���� UI
    [SerializeField]
    private SpriteRenderer m_PlayerHpSpRen;                     // ü�¹ٸ� ��� �� ���Ϸ� ��������Ʈ ������
    [SerializeField]
    private Slider m_PlayerExpBarSlider;                        // ����ġ �� �����̴�
    [SerializeField]
    private Text m_PlayerHealText;                              // ü�� ȸ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject m_PlayerHeal;                            // ü�� ȸ�� �ؽ�Ʈ ������Ʈ

    private void Awake()
    {
        // ������Ʈ �ʱ�ȭ
        m_Ani = GetComponent<Animator>();
        m_SpRen = GetComponent<SpriteRenderer>();

        // ���� �ʱ�ȭ
        m_MoveSpeed = 2f;
        m_PlayerMaxHP = 100;
        m_PlayerHP = m_PlayerMaxHP;
        m_PlayerMaxExp = 5;
        m_PlayerExp = 0;
        m_PlayerLevel = 1;
        m_MoveLevel = 0;
        m_ShieldLevel = 0;

        // �����̴� UI �ʱ�ȭ
        SetHpBar();
        SetExpBar();

        // �ǵ��� Ȱ��ȭ�� Ȯ���ϱ� ���� �ڷ�ƾ ����
        StartCoroutine(ShieldCheck());
    }

    private void Update()
    {
        // �÷��̾� �̵� ���� ����
        //SetPlayerMoveDirection();

        // Ÿ�� �������� 0 �϶��� �۵����� ����
        if (Time.timeScale == 0)
        {
            return;
        }
        // ���⿡ ���� �÷��̾� �̵�
        PlayerMove();

        // �÷��̾� ������
        PlayerSlide();

        // ���콺 ��ǥ�� ���� ������ �̵�
        TakeAim();
            
        // �������� ���� �߻�
        PlayerAttack();
    }

    // �÷��̾� ���� ����
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
    // ���̽�ƽ�� ���� �÷��̾� ���� ����
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
        // �̵����� �����϶�, �����̵尡 ��ٿ� ���°� �ƴ϶�� �����̽��� ���� ����
        if(Input.GetKeyDown(KeyCode.Space) && !m_IsSlideCool && m_DirVec != Vector3.zero && !m_IsLevelUp 
            && !GameManager.Instance.IsMobile && Time.timeScale != 0)
        {
            StartCoroutine(SlideCoroutine());
        }
    }
    public void SlideBtn()
    {
        // ����� ������ ��, �����̵� ��ư�� ���� �ߵ�
        if (!m_IsSlideCool && m_DirVec != Vector3.zero && !m_IsLevelUp && GameManager.Instance.IsMobile 
            && Time.timeScale != 0)
        {
            StartCoroutine(SlideCoroutine());
        }
    }
    IEnumerator SlideCoroutine()
    {
        // �ߺ� ���� ������ ���� ������ true�� ����, ���� ������Ʈ�� ��� ��Ȱ��ȭ ��Ŵ
        m_IsSlide = true;
        m_IsSlideCool = true;
        m_Gun.gameObject.SetActive(false);

        // �̵��ӵ��� ��� 3��� ����
        m_MoveSpeed *= 3f;

        // �̵� ���⿡ �´� �����̵� �ִϸ��̼� �� ��������Ʈ ������ flipX ����(�¿� �켱)
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

        // �̵��ӵ� ���󺹱�
        yield return new WaitForSeconds(0.2f);
        m_MoveSpeed /= 3f;

        // �ٸ� �׼� ���� false, �� ������Ʈ Ȱ��ȭ
        yield return new WaitForSeconds(0.1f);
        m_IsSlide = false;
        m_Gun.gameObject.SetActive(true);

        // �����̵� �ߺ� ���� ���� false
        yield return new WaitForSeconds(0.2f);
        m_IsSlideCool = false;
    }

    // ���콺�� ��ġ�ϴ� ���⿡ ���� �÷��̾�� �÷��̾ ����ִ� ���� ȸ����Ű�� �Լ�
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
        // �÷��̾�� ���� flipX�� false
        m_SpRen.flipX = _bool;

        SpriteRenderer gunSpRen = m_Gun.gameObject.GetComponent<SpriteRenderer>();
        gunSpRen.flipX = _bool;
    }

    // ���콺 �������� ����
    private void SetMouseDirection()
    {
        if (!GameManager.Instance.IsMobile)
        {
            // ���콺�� ��ǥ�� �޾ƿ� ��, �÷��̾��� �����ǿ��� ���콺�� ��ǥ�� ���� ���� ���͸� ����
            m_MouseDir = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, -Input.mousePosition.z));
        }
        else
        {
            if (m_TargetTF != null)
            {
                // ���� ����� ���� ��ġ�� �־���
                m_MouseDir = m_TargetTF.position;
            }
        }
    }
    private void PlayerAttack()
    {
        // ��������� Ȯ��
        // ������� �ƴ� ���
        if (!GameManager.Instance.IsMobile)
        {
            // ������ �����ϸ� ���콺 ����Ŭ������ ����
            if (Input.GetMouseButton(0) && !m_IsSlide && !m_IsFire 
             && !m_IsLevelUp && !EventSystem.current.IsPointerOverGameObject())
            {
                StartCoroutine(PlayerGunFireCoroutine());
            }
        }
        // ������� ���
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
        // ���� ��� ���� true
        m_IsFire = true;

        // Ȯ�� ���� ������
        int ran = Random.Range(0, 5);

        for (int i = 0; i < m_GunManager.FireRate + m_GunManager.FireRate2; i++)
        {
            // ������Ʈ Ǯ�� �Ŵ������� �÷��̾� �ҷ��� ������
            GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerBulletKey);

            // �÷��̾� �ҷ��� ������, ����, Ȯ������ �ο�
            PlayerBullet pBullet = bullet.GetComponent<PlayerBullet>();
            SetBulletDamage(pBullet, ran);

            // �÷��̾� �ҷ��� ���� �������� ���콺�� ���ϴ� ���⿡ ���� ���� �ѱ�, Ȥ�� ���� �ѱ��� �̵�
            bullet.transform.position = m_MouseDir.x - transform.position.x >= 0 ? m_GunRightMuzzle.position : m_GunLeftMuzzle.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = (m_MouseDir - (Vector2)m_Gun.transform.position).normalized;
            rigid.velocity = SetBulletDir(dir) * 12f;
        }

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERSHOT);

        m_GunAni.SetTrigger("Attack");

        // ���� �����̸�ŭ ���
        yield return new WaitForSeconds(m_GunManager.FireDelay);

        // ���� ��� ���� false
        m_IsFire = false;
    }
    void SetBulletDamage(PlayerBullet _pBullet, int _ran)
    {
        _pBullet.SetDamage(m_GunManager.IsDamageUp ? (int)(m_GunManager.Damage * 1.5f) : m_GunManager.Damage);

        // m_Penetration: ���� Ŭ���� �������� ��� Ȯ�� 1ȸ ����
        _pBullet.SetPenetration(m_GunManager.Penetration);

        // m_Penetration2: ������ �������� ��� Ȯ�� 1ȸ ����
        _pBullet.SetPenetration(_ran < m_GunManager.Penetration2 ? 1 : 0);
    }
    Vector2 SetBulletDir(Vector2 dir)
    {
        // �߻��Ϸ��� ��ġ�� ���� ������ x, y�� ������ ���� ���϶�� ������ ��ġ�� ���ؼ� ������ �ʹ� �������� �ʵ��� ����
        dir.x = Mathf.Abs(dir.x) < 0.2f ? dir.x + (Random.Range(-0.4f, 0.4f) * m_GunManager.Accuracy) : dir.x;
        dir.y = Mathf.Abs(dir.y) < 0.2f ? dir.y + (Random.Range(-0.4f, 0.4f) * m_GunManager.Accuracy) : dir.y;

        dir.x *= Random.Range(1 - m_GunManager.Accuracy, 1 + m_GunManager.Accuracy);
        dir.y *= Random.Range(1 - m_GunManager.Accuracy, 1 + m_GunManager.Accuracy);

        return dir.normalized;
    }
    // ������ ���� �Լ�
    public void PlayerOnDamage(int _damage)
    {
        // �����̵� ������ ��������
        if (m_IsSlide || m_IsHit)
            return;

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERHIT);

        // �ǵ尡 Ȱ��ȭ�� ���¶�� �ǵ尡 ���� ��Ȱ��ȭ��
        if (m_Shield.activeSelf)
        {
            m_Shield.SetActive(false);
            return;
        }

        // �ߺ� �ǰ� ���� true
        m_IsHit = true;

        // ���� Ŭ���� ������ ���۶󽺸� ȹ���� ���¶�� �޴� �������� 0.7�� ������
        m_PlayerHP -= m_IsSunglass ? (int)(_damage * 0.7f) : _damage;

        // ü�¹� �����̴� ����
        SetHpBar();

        // ���� ü���� 0 ���϶�� ���� ����
        if (m_PlayerHP <= 0)
        {
            GameManager.Instance.GameOver();
        }

        // �ǰ� �� ���� ����
        m_SpRen.color = Color.grey;

        // ���� �������� ����
        StartCoroutine(ColorReset());
    }
    private IEnumerator ColorReset()
    {
        // 0.2�� ���
        yield return new WaitForSeconds(0.2f);

        // �ߺ� �ǰ� ���� false
        m_IsHit = false;

        // ���� �������� ����
        m_SpRen.color = Color.white;

        yield return new WaitForSeconds(0.3f);
    }

    public void SetIsLevelUp(bool _bool)
    {
        m_IsLevelUp = _bool;
    }

    // ����ġ ���� �Լ�
    public void SetExpUp(int _exp)
    {
        m_PlayerExp += _exp;

        // Exp UI ����
        SetExpBar();

        // ���ھ� ����
        GameManager.Instance.ScoreUp(_exp);

        // �ִ� ����ġ �̻��̶�� ������
        if (m_PlayerExp >= m_PlayerMaxExp)
        {
            PlayerLevelUp();
        }
    }
    // ���� �� �Լ�
    void PlayerLevelUp()
    {
        // ���� ����
        m_PlayerLevel++;

        // ���� ����ġ �ʱ�ȭ
        m_PlayerExp = 0;

        // �ִ� ����ġ�� ����
        m_PlayerMaxExp = 5 + (int)Mathf.Pow(m_PlayerLevel, 1.7f);

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERLEVELUP);

        // Exp UI ����
        SetExpBar();

        // ���� ������ ����
        m_IsLevelUp = true;

        m_LevelUpUI.LevelUp();
        GameManager.Instance.OpenStatus();
    }

    public void SetMaxHpUp(int _maxHp)
    {
        // ��ġ��ŭ �ִ� ü�� ����
        m_PlayerMaxHP += _maxHp;

        // ������ �ִ� ü�¸�ŭ ȸ��
        SetHpUp(_maxHp);
    }
    public void SetHpUp(int _hp)
    {
        // ��ġ��ŭ ü�� ȸ��
        m_PlayerHP += _hp;

        SoundManager.Instance.SoundPlay(SOUND_NAME.PLAYERHEAL);

        // ���� ü���� �ִ� ü�º��� ũ�ٸ� ���� ü���� �ִ� ü������ ����
        if (m_PlayerHP > m_PlayerMaxHP)
        {
            m_PlayerHP = m_PlayerMaxHP;
        }

        // Hp UI ����
        SetHpBar();
        SetHealText(_hp);
    }

    void SetExpBar()
    {
        // ����ġ �� UI �� ����
        m_PlayerExpBarSlider.maxValue = m_PlayerMaxExp;
        m_PlayerExpBarSlider.value = m_PlayerExp;
    }

    void SetHpBar()
    {
        // ü���� 0���� ũ�ų� ���ٸ� m_playerHp��, 0���� �۰ų� ���ٸ� 1�� �־���
        m_PlayerHpSpRen.color = m_PlayerHP > 0 ? new Color(1, 1, 1, (float)m_PlayerHP / (float)m_PlayerMaxHP) : new Color(1, 1, 1, 1);
    }

    // �� �ؽ�Ʈ ����
    void SetHealText(int _hp)
    {
        // �ؽ�Ʈ�� �������� ����
        m_PlayerHealText.text = $"+{_hp}";

        // �ؽ�Ʈ ������Ʈ�� Ȱ��ȭ �Ǿ��ִٸ� ���� ��Ȱ��ȭ ó��
        if (m_PlayerHeal.activeSelf)
        {
            m_PlayerHeal.SetActive(false);
        }
        m_PlayerHeal.SetActive(true);
    }

    public void SetMoveUp()
    {
        // �̵��ӵ� ����
        m_MoveSpeed += 0.2f;
        m_MoveLevel++;
    }
    public void SetMoveUp2()
    {
        // �̵��ӵ� ���� 2
        m_MoveSpeed += 1f;
    }
    public void SunglassSlowStart()
    {
        // ���۶� ȹ�� �� true
        m_IsSunglass = true;
    }
    public void SetShieldUp()
    {
        // �ǵ� ���� ����
        m_ShieldLevel++;
        ShieldRegen();
    }
    public void HealKitStart()
    {
        StartCoroutine(HealKitUp());
    }

    // �ǵ� Ȱ��ȭ �ڷ�ƾ
    IEnumerator ShieldCheck()
    {
        yield return null;
        // ������ �������� ���� �ݺ� Ȯ��
        while (GameManager.Instance.IsPlay)
        {
            // �ǵ尡 ��Ȱ��ȭ �� ���
            if (!m_Shield.activeSelf)
            {
                if (m_ShieldLevel >= 1)
                {
                    // ���� �ð� �� �ǵ� Ȱ��ȭ
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

    // ���� ���� Ȱ��ȭ �ڷ�ƾ
    IEnumerator HealKitUp()
    {
        // ������ �������� ���� �ݺ� Ȯ��
        while (GameManager.Instance.IsPlay)
        {
            // 10�ʸ��� �ִ�ü���� 5%��ŭ ȸ��
            SetHpUp((int)(m_PlayerMaxHP * 0.05f));
            yield return new WaitForSeconds(10f);
        }
    }
    public void SetTargetTF(Transform _tf)
    {
        m_TargetTF = _tf;
    }
}
