using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster11 : Monster
{
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 4500;
        m_MonsterDamage = 20;
        m_MoveSpeed = 1.2f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER11;
        m_MonsterExp = 30;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster11Key;

        base.OnEnable();
    }
}
