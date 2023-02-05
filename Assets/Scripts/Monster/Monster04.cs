using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster04 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 750;
        m_MonsterDamage = 10;
        m_MoveSpeed = 1.2f;
        m_AttackDelay = 1.2f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER04;
        m_MonsterExp = 15;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster04Key;

        base.OnEnable();
    }
}
