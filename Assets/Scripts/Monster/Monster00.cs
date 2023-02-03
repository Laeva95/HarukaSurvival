using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster00 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 45;
        m_MonsterDamage = 10;
        m_MoveSpeed = 1.2f;
        m_AttackDelay = 1f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER00;

        base.OnEnable();
    }

    // ���� �ǰ�
    public override void MonsterOnDamage(int _damage)
    {
        m_MonsterHP -= _damage;
        StartCoroutine(MonsterOnDamageEffect());

        if (IsDead())
        {
            DespawnEffect((int)m_MonsterNum);
            SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERDEAD0);

            Haruka player = m_PlayerTransform.gameObject.GetComponent<Haruka>();
            player.SetExpUp(1);

            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Monster00Key);
        }
    }
}
