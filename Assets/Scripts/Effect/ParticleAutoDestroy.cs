using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoDestroy : MonoBehaviour
{
    void OnEnable()
    {
        // 오브젝트가 활성화 될 때 실행
        gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(CoCheckAlive());
    }

    IEnumerator CoCheckAlive()
    {
        while (true)
        {
            // 0.5초마다 반복해서 확인
            yield return new WaitForSeconds(0.5f);
            // 파티클 시스템이 재생 중이지 않으면 오브젝트 파괴
            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                Destroy(gameObject);
                break;
            }
        }
    }
}
