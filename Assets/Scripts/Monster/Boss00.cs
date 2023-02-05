using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss00 : Monster
{
    [SerializeField] SpriteRenderer m_HpSpRen;
    bool m_IsFire;
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 12000;
        m_MonsterDamage = 10;
        m_MoveSpeed = 3.5f;
        m_AttackDelay = 1.5f;
        m_IsFire = false;
        m_MonsterNum = MONSTER_NUMBER.BOSS00;
        m_MonsterExp = 0;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Boss00Key;

        SetHpBar();

        m_Ani.SetBool("isWalk", true);

        base.OnEnable();
    }

    // ���� �̵�
    protected override void MonsterMove()
    {
        // ���ο� �������� Ȯ��
        float moveSpeed = CheckSlow(m_MoveSpeed);

        // �÷��̾��� ��ġ�� ����Ű�� ���� ����
        Vector3 dir = (m_PlayerTransform.position - transform.position).normalized;

        // ���� �������� ���¶�� �������� ����
        if (m_IsAttack || m_IsFire)
        {
            return;
        }

        // ��������Ʈ ������ �ø� Ȯ��
        MonsterSpriteRendererFlipX(dir);

        // �÷��̾���� �Ÿ��� 8 ���϶�� ź�� ���� ���, �ƴ϶�� �Ÿ��� ����
        if (Vector3.Distance(m_PlayerTransform.position, transform.position) < 8f 
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
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT0);

        // Shot00, Shot01 Ư�� Ƚ�� �ݺ�
        for (int i = 0; i < 8; i++)
        {
            Shot00();

            yield return new WaitForSeconds(0.07f);
        }

        yield return new WaitForSeconds(m_AttackDelay);

        Shot01();
        
        yield return new WaitForSeconds(m_AttackDelay * 0.5f);

        Shot01();

        yield return new WaitForSeconds(m_AttackDelay);

        m_IsFire = false;
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

        rigid.velocity = _dir * 6f;

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
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

            // ���⺤���� �������� ����
            float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

            // �÷��̾� ���⿡�� 20���� �߰��ؼ� �߻�
            if (i % 2 == 0)
            {
                angle += i * 10f;
            }
            else if (i % 2 == 1)
            {
                angle -= (i + 1) * 10f;
            }

            // �������� ���� ������ ����
            float rad = angle * Mathf.Deg2Rad;

            // ������ ���� ���� ���� �߻� ���� ����
            _dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            rigid.velocity = _dir * 5f;
        }
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);
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

            GameManager.Instance.ScoreUp(1200);

            Haruka player = m_PlayerTransform.gameObject.GetComponent<Haruka>();

            // ���� ������Ʈ�� ������Ʈ Ǯ �Ŵ����� ť�� �ٽ� �־���
            ObjectPoolingManager.Instance.InsertQueue(gameObject, m_MonsterSpawnNum);

            GameManager.Instance.BossClear();
        }
    }

    void SetHpBar()
    {
        // ü���� 0���� ũ�ų� ���ٸ� m_MonsterHP��, 0���� �۴ٸ� 0�� �־���
        float hp = (float)m_MonsterHP / (float)m_MonsterMaxHP;
        m_HpSpRen.color = m_MonsterHP >= 0 ? new Color(1, 1, 1, hp): new Color(1, 1, 1, 0);
    }
}
