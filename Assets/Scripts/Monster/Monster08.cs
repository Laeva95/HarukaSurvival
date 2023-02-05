using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster08 : Monster
{
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 1100;
        m_MonsterDamage = 50;
        m_MoveSpeed = 3f;
        m_AttackDelay = 0.1f;
        m_MonsterNum = MONSTER_NUMBER.MONSTER08;
        m_MonsterExp = 50;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster08Key;

        base.OnEnable();
    }

    // 몬스터 이동
    protected override void MonsterMove()
    {
        float moveSpeed = m_MoveSpeed;

        // 슬로우가 걸린 상태라면 이속 30% 감소
        if (GameManager.Instance != null && GameManager.Instance.IsSlow)
        {
            moveSpeed *= 0.7f;
        }

        // 플레이어의 위치를 가리키는 방향 벡터
        Vector3 dir = (m_PlayerTransform.position - transform.position).normalized;

        // 현재 공격중인 상태라면 움직이지 않음
        if (m_IsAttack)
        {
            return;
        }
        // 플레이어의 방향에 따라 스프라이트 렌더러의 flipX 활성화 여부를 결정
        if (dir.x >= 0)
        {
            m_SpRen.flipX = true;
        }
        else
        {
            m_SpRen.flipX = false;
        }

        // 플레이어를 향한 방향 벡터를 통해 탄젠트 Radian값 계산, Mathf.Rad2Deg를 통해 Degree로 변환
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        // 구한 각도를 통해 몬스터를 Z축 회전시킴
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 해당 방향을 향해 초당 m_MoveSpeed 만큼 이동
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    // 몬스터 공격
    protected override IEnumerator MonsterAttack(GameObject _obj)
    {
        // 이펙트 생성
        BoomEffect();

        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);

        yield return StartCoroutine(base.MonsterAttack(_obj));

        // 몬스터 오브젝트를 오브젝트 풀 매니저의 큐에 다시 넣어줌
        ObjectPoolingManager.Instance.InsertQueue(gameObject, ObjectPoolingManager.m_Monster08Key);
    }


    protected void BoomEffect()
    {
        // 이펙트 생성 후 파티클 재생, 몬스터의 위치로 가져온다
        GameObject effect = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterEffect2Key);

        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
