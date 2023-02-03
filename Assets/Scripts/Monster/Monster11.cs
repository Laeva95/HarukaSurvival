using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster11 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 4500;
        m_MonsterDamage = 20;
        m_MoveSpeed = 1.2f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER11;

        base.OnEnable();
    }

    // ���� �ǰ�
    public override void MonsterOnDamage(int _damage)
    {
        m_MonsterHP -= _damage;

        StartCoroutine(MonsterOnDamageEffect());

        if (m_MonsterHP <= 0)
        {
            DespawnEffect((int)m_MonsterNum);

            SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERDEAD0);

            Haruka player = m_PlayerTransform.gameObject.GetComponent<Haruka>();
            player.SetExpUp(30);

            // ���� ������Ʈ�� ������Ʈ Ǯ �Ŵ����� ť�� �ٽ� �־���
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Monster11Key);
        }
    }
}
