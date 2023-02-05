using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster11 : Monster
{
    // ���� �Ǿ��� �� ���� �ʱ�ȭ
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
