using System.Collections;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    protected MonsterConfig m_Config;             // 몬스터 스크립터블 오브젝트
    protected Transform m_PlayerTransform;        // 플레이어 위치
    protected Rigidbody2D m_Rigid;                // 몬스터 리지드바디
    protected Animator m_Ani;                     // 몬스터 애니메이터
    protected SpriteRenderer m_SpRen;             // 몬스터 스프라이트 렌더터
    protected int m_MonsterMaxHP;                 // 몬스터 최대 체력
    protected int m_MonsterHP;                    // 몬스터 체력
    protected int m_MonsterDamage;                // 몬스터 공격력
    protected float m_MoveSpeed;                  // 몬스터 이동 속도
    protected float m_AttackDelay;                // 몬스터 공격 간격
    protected bool m_IsAttack;                    // 몬스터 공격 중복 방지 변수
    protected MONSTER_NUMBER m_MonsterNum;        // 몬스터를 구분하기 위한 개체 번호

    public Rigidbody2D Rigid => m_Rigid;

    private void Awake()
    {
        // 컴포넌트 초기화
        m_PlayerTransform = FindObjectOfType<Haruka>().transform;
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Ani = GetComponent<Animator>();
        m_SpRen = GetComponent<SpriteRenderer>();
    }

    // 모든 몬스터 클래스에서 사용하는 내용이므로 부모 클래스에서 작성
    // 필요한 내용은 개별로 작성 후 base.OnEnable()을 통해 호출
    protected virtual void OnEnable()
    {
        m_MonsterHP = m_MonsterMaxHP;
        m_IsAttack = false;

        // 몬스터 09를 제외한 몬스터들은 생성될 때 기본 색으로 변경
        if (m_MonsterNum != MONSTER_NUMBER.MONSTER09)
        {
            m_SpRen.color = Color.white;
        }
        if (ObjectPoolingManager.Instance != null)
        {
            ObjectPoolingManager.Instance.m_MonsterCount++;
        }
    }

    protected void Update()
    {
        MonsterMove();
    }

    // 모든 몬스터 클래스에서 사용하는 내용이므로 부모 클래스에서 작성
    // 일부 클래스에서는 따로 작성할 필요가 있으므로 가상 함수로 작성
    protected virtual void MonsterMove()
    {
        float moveSpeed = CheckSlow(m_MoveSpeed);

        // 플레이어의 위치를 가리키는 방향 벡터
        Vector3 dir = (m_PlayerTransform.position - transform.position).normalized;

        // 현재 공격중인 상태라면 움직이지 않음
        if (m_IsAttack)
        {
            return;
        }

        // 스프라이트 플립 확인
        MonsterSpriteRendererFlipX(dir);

        // 해당 방향을 향해 초당 m_MoveSpeed 만큼 이동
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    protected void MonsterSpriteRendererFlipX(Vector3 _vec)
    {
        // 플레이어의 방향에 따라 스프라이트 렌더러의 flipX 활성화 여부를 결정
        if (_vec.x >= 0)
        {
            m_SpRen.flipX = true;
        }
        else
        {
            m_SpRen.flipX = false;
        }
    }
    protected float CheckSlow(float _move)
    {
        // 전체 슬로우가 걸린 상태라면 이속 30% 감소
        if (GameManager.Instance != null && GameManager.Instance.IsSlow)
        {
            _move *= 0.7f;
        }

        return _move;
    }
    protected virtual IEnumerator MonsterAttack(GameObject _obj)
    {
        // 현재 공격 상태를 true로 변경
        m_IsAttack = true;

        m_Ani.SetTrigger("Attack");

        // 플레이어 오브젝트에서 Haruka 스크립트를 가져옴
        Haruka player = _obj.GetComponent<Haruka>();

        // 플레이어의 Damage 함수 작동
        player.PlayerOnDamage(m_MonsterDamage);

        // 공격 딜레이 시간만큼 대기
        yield return new WaitForSeconds(m_AttackDelay);

        // 공격 상태를 false로 되돌림
        m_IsAttack = false;
    }
    public abstract void MonsterOnDamage(int _damage);

    // 몬스터 피격 이펙트 코루틴
    protected IEnumerator MonsterOnDamageEffect()
    {
        // 피격당한 몬스터의 체력이 남아있는지 확인, 몬스터 09는 색을 변경하지않음
        if (m_MonsterHP > 0 && m_MonsterNum != MONSTER_NUMBER.MONSTER09)
        {
            // 색상을 red로 변경
            m_SpRen.color = Color.red;

            // 0.2초 대기
            yield return new WaitForSeconds(0.2f);

            // 색상을 white로 변경
            m_SpRen.color = Color.white;
        }
    }


    // 사망 이펙트 생성 함수
    // 모든 몬스터 클래스에서 사용
    protected void DespawnEffect(int _monsterNum)
    {
        // 이펙트 생성 후 파티클 재생, 몬스터의 위치로 가져온다
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterEffectKey);

        ParticleSystem.MainModule newModule = effect.GetComponent<ParticleSystem>().main;
        newModule.startColor = m_Config.m_MonsterColor[_monsterNum];

        effect.transform.position = transform.position;
        effect.SetActive(true);
    }


    // 플레이어와 부딪혔을 때 호출되는 함수
    // 모든 몬스터는 플레이어와 충돌시 Attack을 실행하므로 부모 클래스에서 작성
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 플레이어와 충돌했을 경우, 현재 공격 대기 중이 아니라면 실행
        if (collision.transform.tag == "Player" && !m_IsAttack && gameObject.activeSelf)
        {
            StartCoroutine(MonsterAttack(collision.gameObject));
        }
    }

    // 비활성화시 모든 코루틴 종료
    private void OnDisable()
    {
        if(ObjectPoolingManager.Instance != null)
        {
            ObjectPoolingManager.Instance.m_MonsterCount--;
        }
        StopAllCoroutines();
    }
    protected bool IsDead()
    {
        return m_MonsterHP <= 0;
    }
}
