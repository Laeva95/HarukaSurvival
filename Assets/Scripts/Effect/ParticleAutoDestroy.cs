using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoDestroy : MonoBehaviour
{
    void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ �� �� ����
        gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(CoCheckAlive());
    }

    IEnumerator CoCheckAlive()
    {
        while (true)
        {
            // 0.5�ʸ��� �ݺ��ؼ� Ȯ��
            yield return new WaitForSeconds(0.5f);
            // ��ƼŬ �ý����� ��� ������ ������ ������Ʈ �ı�
            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                Destroy(gameObject);
                break;
            }
        }
    }
}
