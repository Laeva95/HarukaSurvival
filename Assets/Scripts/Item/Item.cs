using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int m_Exp;
    private int m_ItemNum;
    private bool m_IsCatched;
    private WaitForSeconds m_CheckSec = new WaitForSeconds(0.0167f);

    private void OnEnable()
    {
        m_IsCatched = false;
    }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !m_IsCatched)
        {
            m_IsCatched = true;

            Haruka player = collision.GetComponent<Haruka>();

            StartCoroutine(GetItemCoroutine(player));
        }
    }
    IEnumerator GetItemCoroutine(Haruka _player)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 dir = (_player.transform.position - transform.position).normalized;
            transform.position += dir * 4f * Time.deltaTime;
            yield return m_CheckSec;
        }

        _player.SetExpUp(m_Exp);

        ObjectPoolingManager.Instance.InsertQueue(gameObject, m_ItemNum);
    }
}
