using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster00 : Monster
{
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 45;
        m_MonsterDamage = 10;
        m_MoveSpeed = 1.2f;
        m_AttackDelay = 1f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER00;
        m_MonsterExp = 1;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster00Key;

        base.OnEnable();
    }
}
