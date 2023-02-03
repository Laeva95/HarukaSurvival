using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster06 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 850;
        m_MonsterDamage = 15;
        m_MoveSpeed = 2.5f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER06;

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
            player.SetExpUp(20);

            // ���� ������Ʈ�� ������Ʈ Ǯ �Ŵ����� ť�� �ٽ� �־���
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Monster06Key);
        }
    }
}
