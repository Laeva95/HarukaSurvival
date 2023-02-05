using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster01 : Monster
{
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 95;
        m_MonsterDamage = 10;
        m_MoveSpeed = 1.5f;
        m_AttackDelay = 1f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER01;
        m_MonsterExp = 2;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster01Key;

        base.OnEnable();
    }
}
