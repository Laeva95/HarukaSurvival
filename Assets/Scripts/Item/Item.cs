using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected int m_ItemNum;
    protected bool m_IsCatched;
    protected WaitForSeconds m_CheckSec = new WaitForSeconds(0.0167f);

    private void OnEnable()
    {
        m_IsCatched = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemCheck") && !m_IsCatched)
        {
            m_IsCatched = true;

            StartCoroutine(GetItemCoroutine(collision));
        }
        if (collision.CompareTag("Player") && m_IsCatched)
        {
            UsingItem(collision);
        }
    }
    protected IEnumerator GetItemCoroutine(Collider2D collision)
    {
        while (gameObject.activeSelf)
        {
            Vector3 dir = (collision.transform.position - transform.position).normalized;
            transform.position += dir * 6f * Time.deltaTime;
            yield return m_CheckSec;
        }
    }
    protected abstract void UsingItem(Collider2D collision);
}
