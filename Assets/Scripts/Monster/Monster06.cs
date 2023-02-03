using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster06 : Monster
{
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 850;
        m_MonsterDamage = 15;
        m_MoveSpeed = 2.5f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER06;

        base.OnEnable();
    }



    // 몬스터 피격
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

            // 몬스터 오브젝트를 오브젝트 풀 매니저의 큐에 다시 넣어줌
            ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Monster06Key);
        }
    }
}
