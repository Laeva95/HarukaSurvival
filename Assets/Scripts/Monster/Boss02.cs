using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02 : Monster
{
    [SerializeField] SpriteRenderer m_HpSpRen;
    bool m_IsFire;
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 105000;
        m_MonsterDamage = 20;
        m_MoveSpeed = 4.5f;
        m_AttackDelay = 1.2f;
        m_IsFire = false;
        m_MonsterNum = MONSTER_NUMBER.BOSS02;

        SetHpBar();

        m_Ani.SetBool("isWalk", true);

        base.OnEnable();
    }

    // ���� �̵�
    protected override void MonsterMove()
    {
        float moveSpeed = m_MoveSpeed;

        // ���ο찡 �ɸ� ���¶�� �̼� 30% ����
        if (GameManager.Instance != null && GameManager.Instance.IsSlow)
        {
            moveSpeed *= 0.7f;
        }

        // �÷��̾��� ��ġ�� ����Ű�� ���� ����
        Vector3 dir = (m_PlayerTransform.position - transform.position).normalized;

        // ���� �������� ���¶�� �������� ����
        if (m_IsAttack || m_IsFire)
        {
            return;
        }

        // �÷��̾��� ���⿡ ���� ��������Ʈ �������� flipX Ȱ��ȭ ���θ� ����
        if (dir.x >= 0)
        {
            m_SpRen.flipX = true;
        }
        else
        {
            m_SpRen.flipX = false;
        }

        // �÷��̾���� �Ÿ��� 9 ���϶�� ź������ ���, �ƴ϶�� �Ÿ��� ����
        if (Vector3.Distance(m_PlayerTransform.position, transform.position) < 9f
            && !m_IsFire && !m_IsAttack && ObjectPoolingManager.Instance.IsSpawn)
        {
            StartCoroutine(FireCoroutine());
            return;
        }

        m_Ani.SetBool("isWalk", true);
        // �ش� ������ ���� �ʴ� _moveSpeed ��ŭ �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    // ź�� ���� ���� �ڷ�ƾ
    IEnumerator FireCoroutine( )
    {
        m_IsFire = true;
        int ran = Random.Range(0, 3);

        // Shot00���� Shot02������ ���� �� �ϳ� ���
        if (ran == 0)
        {
            yield return Shot00Start();
        }
        else if (ran == 1)
        {
            yield return Shot01Start();
        }
        else if (ran == 2)
        {
            yield return Shot02Start();
        }

        m_IsFire = false;
    }
    IEnumerator Shot00Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT0);

            for (int j = 0; j < 7; j++)
            {
                Shot00();

                yield return new WaitForSeconds(0.07f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(m_AttackDelay);
    }
    void Shot00()
    {
        GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBulletKey);

        // ���� �ҷ��� ������ �ο�
        MonsterBullet pBullet = bullet.GetComponent<MonsterBullet>();
        pBullet.SetDamage(m_MonsterDamage);

        // ���� �ҷ��� �������� �÷��̾��� ������ ���� ���� �ѱ�, Ȥ�� ���� �ѱ��� �̵�
        bullet.transform.position = m_SpRen.flipX ? transform.position + (Vector3.right * 0.5f) : transform.position + (Vector3.left * 0.5f);

        // ���� �ҷ��� RigidBody�� �����ͼ� ���콺�� ����Ű�� ������ ���� �������� ������ ���� ���ؼ� �߻�
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        // �÷��̾��� ��ġ�� ����Ű�� ���� ����
        Vector3 _dir = (m_PlayerTransform.position - transform.position).normalized;

        rigid.velocity = _dir * 8f;

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
    }
    IEnumerator Shot01Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Shot01();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(1.2f);
        }
        yield return new WaitForSeconds(m_AttackDelay);
    }
    void Shot01()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBulletKey);

            // ���� �ҷ��� ������ �ο�
            MonsterBullet pBullet = bullet.GetComponent<MonsterBullet>();
            pBullet.SetDamage(m_MonsterDamage);

            // ���� �ҷ��� �������� �÷��̾��� ������ ���� ���� �ѱ�, Ȥ�� ���� �ѱ��� �̵�
            bullet.transform.position = m_SpRen.flipX ? transform.position + (Vector3.right * 0.5f) : transform.position + (Vector3.left * 0.5f);

            // ���� �ҷ��� RigidBody�� �����ͼ� ���콺�� ����Ű�� ������ ���� �������� ������ ���� ���ؼ� �߻�
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            // �÷��̾��� ��ġ�� ����Ű�� ���� ����
            Vector3 _dir = (m_PlayerTransform.position - transform.position).normalized;

            float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

            // �÷��̾� ���⿡�� 15���� �߰��ؼ� �߻�
            if (i % 2 == 0)
            {
                angle += i * 7.5f;
            }
            else if (i % 2 == 1)
            {
                angle -= (i + 1) * 7.5f;
            }
            float rad = angle * Mathf.Deg2Rad;

            _dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            rigid.velocity = _dir * 5.5f;
        }
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);
        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
    }

    IEnumerator Shot02Start( )
    {
        for (int i = 0; i < 3; i++)
        {
            Shot02();
            yield return new WaitForSeconds(1.2f);
        }
        yield return new WaitForSeconds(m_AttackDelay);
    }

    void Shot02()
    {
        GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBullet2Key);

        // ���� �ҷ��� ������ �ο�
        MonsterBullet2 pBullet = bullet.GetComponent<MonsterBullet2>();
        pBullet.SetDamage(m_MonsterDamage);

        // ���� �ҷ��� �������� �÷��̾��� ������ ���� ���� �ѱ�, Ȥ�� ���� �ѱ��� �̵�
        bullet.transform.position = m_SpRen.flipX ? transform.position + (Vector3.right * 0.5f) : transform.position + (Vector3.left * 0.5f);

        // ���� �ҷ��� RigidBody�� �����ͼ� ���콺�� ����Ű�� ������ ���� �������� ������ ���� ���ؼ� �߻�
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        // �÷��̾��� ��ġ�� ����Ű�� ���� ����
        Vector3 _dir = (m_PlayerTransform.position - transform.position).normalized;

        rigid.velocity = _dir * 6f;

        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
    }

    // ���� ����
    protected override IEnumerator MonsterAttack(GameObject _obj)
    {
        if (m_IsFire)
        {
            yield return new WaitForSeconds(0.1f);
            StopCoroutine(MonsterAttack(_obj));
        }
        m_Ani.SetBool("isWalk", false);

        base.MonsterAttack(_obj);
    }

    // ���� �ǰ�
    public override void MonsterOnDamage(int _damage)
    {
        m_MonsterHP -= _damage;
        SetHpBar();

        StartCoroutine(MonsterOnDamageEffect());

        if (m_MonsterHP <= 0)
        {
            DespawnEffect((int)m_MonsterNum);

            GameManager.Instance.ScoreUp(10500);

            Haruka player = m_PlayerTransform.gameObject.GetComponent<Haruka>();

            // ���� ������Ʈ�� ������Ʈ Ǯ �Ŵ����� ť�� �ٽ� �־���
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Boss02Key);

            GameManager.Instance.BossClear();
        }
    }

    void SetHpBar()
    {
        // ü���� 0���� ũ�ų� ���ٸ� m_MonsterHP��, 0���� �۴ٸ� 0�� �־���
        float hp = (float)m_MonsterHP / (float)m_MonsterMaxHP;
        m_HpSpRen.color = m_MonsterHP >= 0 ? new Color(1, 1, 1, hp) : new Color(1, 1, 1, 0);
    }
}
