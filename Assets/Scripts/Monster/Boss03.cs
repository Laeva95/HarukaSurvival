using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03 : Monster
{
    [SerializeField] SpriteRenderer m_HpSpRen;
    bool m_IsFire;
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 405000;
        m_MonsterDamage = 30;
        m_MoveSpeed = 5f;
        m_AttackDelay = 1f;
        m_IsFire = false;
        m_MonsterNum = MONSTER_NUMBER.BOSS03;

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
            m_HpSpRen.flipX = true;
        }
        else
        {
            m_SpRen.flipX = false;
            m_HpSpRen.flipX = false;
        }

        // �÷��̾���� �Ÿ��� 9 ���϶�� ź������ ���, �ƴ϶�� �Ÿ��� ����
        if (Vector3.Distance(m_PlayerTransform.position, transform.position) < 9f
            && !m_IsFire && !m_IsAttack && ObjectPoolingManager.Instance.IsSpawn)
        {
            StartCoroutine(FireCoroutine());
            return;
        }

        m_Ani.SetBool("isWalk", true);
        // �ش� ������ ���� �ʴ� m_MoveSpeed ��ŭ �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    // ź�� ���� ���� �ڷ�ƾ
    IEnumerator FireCoroutine()
    {
        m_IsFire = true;

        int ran = Random.Range(0, 4);

        // Shot00���� Shot03���� ���� �� �ϳ� ���
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
        else if (ran == 3)
        {
            yield return Shot03Start();
        }

        // 1�� ���
        yield return new WaitForSeconds(m_AttackDelay);

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

        rigid.velocity = _dir * 10f;

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
    }
    IEnumerator Shot01Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Shot01();
            yield return new WaitForSeconds(1f);
        }
    }
    void Shot01()
    {
        for (int i = 0; i < 24; i++)
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

            // �� ������ ���� ź�� �߻�
            float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

            angle += (i + 1) * 15f;

            float rad = angle * Mathf.Deg2Rad;

            _dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            rigid.velocity = _dir * 6f;
        }
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
    }

    IEnumerator Shot02Start()
    {
        yield return Shot02();
    }

    IEnumerator Shot02()
    {
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 4; i++)
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
                Vector3 _dir = Vector3.zero;

                // j�� 0�϶�, 1�϶��� ����
                if (j == 0)
                {
                    // ���� 4�������� �߻�
                    switch (i)
                    {
                        case 0:
                            _dir = new Vector3(1, 0);
                            break;
                        case 1:
                            _dir = new Vector3(-1, 0);
                            break;
                        case 2:
                            _dir = new Vector3(0, 1);
                            break;
                        case 3:
                            _dir = new Vector3(0, -1);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // �밢 4�������� �߻�
                    switch (i)
                    {
                        case 0:
                            _dir = new Vector3(1, 1).normalized;
                            break;
                        case 1:
                            _dir = new Vector3(-1, -1).normalized;
                            break;
                        case 2:
                            _dir = new Vector3(-1, 1).normalized;
                            break;
                        case 3:
                            _dir = new Vector3(1, -1).normalized;
                            break;
                        default:
                            break;
                    }
                }

                rigid.velocity = _dir * 4f;
            }

            m_Ani.SetBool("isWalk", false);
            m_Ani.SetTrigger("Attack");

            SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Shot03Start()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return Shot03();
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator Shot03()
    {
        // �÷��̾��� ���� ���� ����
        Vector3 dir = (m_PlayerTransform.position - transform.position).normalized;

        // ����Ʈ ����
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_BossEffectKey);

        // ����Ʈ ��ġ �ѱ� �������� �̵�
        effect.transform.position = m_SpRen.flipX ? transform.position + (Vector3.right * 0.5f) : transform.position + (Vector3.left * 0.5f);

        // �÷��̾� ��ġ�� ������ ���ؼ� �־���
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(0.5f);

        // ����Ʈ ����
        ObjectPoolingManager.Instance.InsertQueue(effect, ObjectPoolingManager.m_BossEffectKey);

        // ���� ���
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT0);

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");

        for (int i = 0; i < 40; i++)
        {
            GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBulletKey);

            // ���� �ҷ��� ������ �ο�
            MonsterBullet pBullet = bullet.GetComponent<MonsterBullet>();
            pBullet.SetDamage(m_MonsterDamage);

            // ���� �ҷ��� �������� �÷��̾��� ������ ���� ���� �ѱ�, Ȥ�� ���� �ѱ��� �̵�
            bullet.transform.position = m_SpRen.flipX ? transform.position + (Vector3.right * 0.5f) : transform.position + (Vector3.left * 0.5f);

            bullet.transform.position += dir * i / 2;
        }
        yield return new WaitForSeconds(0.8f);
    }

    // ���� ����
    protected override IEnumerator MonsterAttack(GameObject _obj)
    {
        // ź�� ���� �����̶�� ������ ������������
        if (m_IsFire)
        {
            yield return null;
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

            GameManager.Instance.ScoreUp(40500);

            Haruka player = m_PlayerTransform.gameObject.GetComponent<Haruka>();

            // ���� ������Ʈ�� ������Ʈ Ǯ �Ŵ����� ť�� �ٽ� �־���
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Boss03Key);

            // GameManager.Instance.BossClear();
            GameManager.Instance.GameClear();
        }
    }

    void SetHpBar()
    {
        // ü���� 0���� ũ�ų� ���ٸ� m_MonsterHP��, 0���� �۴ٸ� 0�� �־���
        float hp = (float)m_MonsterHP / (float)m_MonsterMaxHP;
        m_HpSpRen.color = m_MonsterHP >= 0 ? new Color(1, 1, 1, hp) : new Color(1, 1, 1, 0);
    }
}
