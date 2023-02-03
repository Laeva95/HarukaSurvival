using System.Collections;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    protected MonsterConfig m_Config;             // ���� ��ũ���ͺ� ������Ʈ
    protected Transform m_PlayerTransform;        // �÷��̾� ��ġ
    protected Rigidbody2D m_Rigid;                // ���� ������ٵ�
    protected Animator m_Ani;                     // ���� �ִϸ�����
    protected SpriteRenderer m_SpRen;             // ���� ��������Ʈ ������
    protected int m_MonsterMaxHP;                 // ���� �ִ� ü��
    protected int m_MonsterHP;                    // ���� ü��
    protected int m_MonsterDamage;                // ���� ���ݷ�
    protected float m_MoveSpeed;                  // ���� �̵� �ӵ�
    protected float m_AttackDelay;                // ���� ���� ����
    protected bool m_IsAttack;                    // ���� ���� �ߺ� ���� ����
    protected MONSTER_NUMBER m_MonsterNum;        // ���͸� �����ϱ� ���� ��ü ��ȣ

    public Rigidbody2D Rigid => m_Rigid;

    private void Awake()
    {
        // ������Ʈ �ʱ�ȭ
        m_PlayerTransform = FindObjectOfType<Haruka>().transform;
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Ani = GetComponent<Animator>();
        m_SpRen = GetComponent<SpriteRenderer>();
    }

    // ��� ���� Ŭ�������� ����ϴ� �����̹Ƿ� �θ� Ŭ�������� �ۼ�
    // �ʿ��� ������ ������ �ۼ� �� base.OnEnable()�� ���� ȣ��
    protected virtual void OnEnable()
    {
        m_MonsterHP = m_MonsterMaxHP;
        m_IsAttack = false;

        // ���� 09�� ������ ���͵��� ������ �� �⺻ ������ ����
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

    // ��� ���� Ŭ�������� ����ϴ� �����̹Ƿ� �θ� Ŭ�������� �ۼ�
    // �Ϻ� Ŭ���������� ���� �ۼ��� �ʿ䰡 �����Ƿ� ���� �Լ��� �ۼ�
    protected virtual void MonsterMove()
    {
        float moveSpeed = CheckSlow(m_MoveSpeed);

        // �÷��̾��� ��ġ�� ����Ű�� ���� ����
        Vector3 dir = (m_PlayerTransform.position - transform.position).normalized;

        // ���� �������� ���¶�� �������� ����
        if (m_IsAttack)
        {
            return;
        }

        // ��������Ʈ �ø� Ȯ��
        MonsterSpriteRendererFlipX(dir);

        // �ش� ������ ���� �ʴ� m_MoveSpeed ��ŭ �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    protected void MonsterSpriteRendererFlipX(Vector3 _vec)
    {
        // �÷��̾��� ���⿡ ���� ��������Ʈ �������� flipX Ȱ��ȭ ���θ� ����
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
        // ��ü ���ο찡 �ɸ� ���¶�� �̼� 30% ����
        if (GameManager.Instance != null && GameManager.Instance.IsSlow)
        {
            _move *= 0.7f;
        }

        return _move;
    }
    protected virtual IEnumerator MonsterAttack(GameObject _obj)
    {
        // ���� ���� ���¸� true�� ����
        m_IsAttack = true;

        m_Ani.SetTrigger("Attack");

        // �÷��̾� ������Ʈ���� Haruka ��ũ��Ʈ�� ������
        Haruka player = _obj.GetComponent<Haruka>();

        // �÷��̾��� Damage �Լ� �۵�
        player.PlayerOnDamage(m_MonsterDamage);

        // ���� ������ �ð���ŭ ���
        yield return new WaitForSeconds(m_AttackDelay);

        // ���� ���¸� false�� �ǵ���
        m_IsAttack = false;
    }
    public abstract void MonsterOnDamage(int _damage);

    // ���� �ǰ� ����Ʈ �ڷ�ƾ
    protected IEnumerator MonsterOnDamageEffect()
    {
        // �ǰݴ��� ������ ü���� �����ִ��� Ȯ��, ���� 09�� ���� ������������
        if (m_MonsterHP > 0 && m_MonsterNum != MONSTER_NUMBER.MONSTER09)
        {
            // ������ red�� ����
            m_SpRen.color = Color.red;

            // 0.2�� ���
            yield return new WaitForSeconds(0.2f);

            // ������ white�� ����
            m_SpRen.color = Color.white;
        }
    }


    // ��� ����Ʈ ���� �Լ�
    // ��� ���� Ŭ�������� ���
    protected void DespawnEffect(int _monsterNum)
    {
        // ����Ʈ ���� �� ��ƼŬ ���, ������ ��ġ�� �����´�
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterEffectKey);

        ParticleSystem.MainModule newModule = effect.GetComponent<ParticleSystem>().main;
        newModule.startColor = m_Config.m_MonsterColor[_monsterNum];

        effect.transform.position = transform.position;
        effect.SetActive(true);
    }


    // �÷��̾�� �ε����� �� ȣ��Ǵ� �Լ�
    // ��� ���ʹ� �÷��̾�� �浹�� Attack�� �����ϹǷ� �θ� Ŭ�������� �ۼ�
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �÷��̾�� �浹���� ���, ���� ���� ��� ���� �ƴ϶�� ����
        if (collision.transform.tag == "Player" && !m_IsAttack && gameObject.activeSelf)
        {
            StartCoroutine(MonsterAttack(collision.gameObject));
        }
    }

    // ��Ȱ��ȭ�� ��� �ڷ�ƾ ����
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
