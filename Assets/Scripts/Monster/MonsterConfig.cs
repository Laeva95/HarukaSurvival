using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTER_NUMBER
{
    MONSTER00 = 0,
    MONSTER01 = 1,
    MONSTER02 = 2,
    MONSTER03 = 3,
    MONSTER04 = 4,
    MONSTER05 = 5,
    MONSTER06 = 6,
    MONSTER07 = 7,
    MONSTER08 = 8,
    MONSTER09 = 9,
    MONSTER10 = 10,
    MONSTER11 = 11,


    BOSS00 = 12,
    BOSS01 = 13,
    BOSS02 = 14,
    BOSS03 = 15,


}

[CreateAssetMenu(menuName = "Monster/Monster Config", fileName = "MonsterConfig.asset")]
public class MonsterConfig : ScriptableObject
{
    // ªÁ∏¡ ¿Ã∆Â∆Æ, ƒ√∑Ø
    public Color[] m_MonsterColor;
    public GameObject m_Effect;
}
