using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster05 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 2250;
        m_MonsterDamage = 20;
        m_MoveSpeed = 1f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER05;
        m_MonsterExp = 25;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster05Key;

        base.OnEnable();
    }
}
