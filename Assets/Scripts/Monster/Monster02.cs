using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster02 : Monster
{
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 190;
        m_MonsterDamage = 10;
        m_MoveSpeed = 1.8f;
        m_AttackDelay = 1f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER02;
        m_MonsterExp = 3;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster02Key;

        base.OnEnable();
    }
}
