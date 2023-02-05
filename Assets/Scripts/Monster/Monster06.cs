using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster06 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 650;
        m_MonsterDamage = 15;
        m_MoveSpeed = 2.5f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER06;
        m_MonsterExp = 20;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster06Key;

        base.OnEnable();
    }
}
