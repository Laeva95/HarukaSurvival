using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03 : Monster
{
    [SerializeField] SpriteRenderer m_HpSpRen;
    bool m_IsFire;
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 500;
        m_MonsterDamage = 10;
        m_MoveSpeed = 2.4f;
        m_AttackDelay = 1.5f;
        m_IsFire = false;
        m_MonsterNum = MONSTER_NUMBER.MONSTER03;
        m_MonsterExp = 30;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster03Key;

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

        // �÷��̾���� �Ÿ��� 6 ���϶�� ź������ ���, �ƴ϶�� �Ÿ��� ����
        if (Vector3.Distance(m_PlayerTransform.position, transform.position) < 6f
            && !m_IsFire && !m_IsAttack && ObjectPoolingManager.Instance.IsSpawn)
        {
            StartCoroutine(FireCoroutine());
            return;
        }

        m_Ani.SetBool("isWalk", true);
        // �ش� ������ ���� �ʴ� m_MoveSpeed ��ŭ �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    IEnumerator FireCoroutine()
    {
        m_IsFire = true;
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);

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
        
        yield return new WaitForSeconds(m_AttackDelay);
        m_IsFire = false;
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
    public override void MonsterOnDamage(int _damage)
    {
        base.MonsterOnDamage(_damage);
        SetHpBar();
    }

    void SetHpBar()
    {
        // ü���� 0���� ũ�ų� ���ٸ� m_MonsterHP��, 0���� �۴ٸ� 0�� �־���
        float hp = (float)m_MonsterHP / (float)m_MonsterMaxHP;
        m_HpSpRen.color = m_MonsterHP >= 0 ? new Color(1, 1, 1, hp) : new Color(1, 1, 1, 0);
    }
}
