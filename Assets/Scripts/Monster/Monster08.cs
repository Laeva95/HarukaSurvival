using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster08 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 1100;
        m_MonsterDamage = 50;
        m_MoveSpeed = 3f;
        m_AttackDelay = 0.1f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER08;
        m_MonsterExp = 50;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster08Key;

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
        if (m_IsAttack)
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

        // �÷��̾ ���� ���� ���͸� ���� ź��Ʈ Radian�� ���, Mathf.Rad2Deg�� ���� Degree�� ��ȯ
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        // ���� ������ ���� ���͸� Z�� ȸ����Ŵ
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // �ش� ������ ���� �ʴ� m_MoveSpeed ��ŭ �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    // ���� ����
    protected override IEnumerator MonsterAttack(GameObject _obj)
    {
        // ����Ʈ ����
        BoomEffect();

        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);

        yield return StartCoroutine(base.MonsterAttack(_obj));

        // ���� ������Ʈ�� ������Ʈ Ǯ �Ŵ����� ť�� �ٽ� �־���
        ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Monster08Key);
    }


    protected void BoomEffect()
    {
        // ����Ʈ ���� �� ��ƼŬ ���, ������ ��ġ�� �����´�
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterEffect2Key);

        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
