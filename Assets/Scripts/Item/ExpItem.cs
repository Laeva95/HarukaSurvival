using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    private int m_Exp;

    public void SetExp(int _exp)
    {
        m_Exp = _exp;
        if (_exp < 10)
        {
            m_ItemNum = ObjectPoolingManager.m_ExpItem0Key;
        }
        else if (_exp < 50)
        {
            m_ItemNum = ObjectPoolingManager.m_ExpItem1Key;
        }
        else if (_exp < 100)
        {
            m_ItemNum = ObjectPoolingManager.m_ExpItem2Key;
        }
        else
        {
            m_ItemNum = ObjectPoolingManager.m_ExpItem3Key;
        }
    }

    protected override void UsingItem(Collider2D collision)
    {
        Haruka player = collision.GetComponent<Haruka>();
        player.SetExpUp(m_Exp);
        StopAllCoroutines();
        ObjectPoolingManager.Instance.InsertQueue(gameObject, m_ItemNum);
    }
}
