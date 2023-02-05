using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03 : Monster
{
    [SerializeField] SpriteRenderer m_HpSpRen;
    bool m_IsFire;
    // 스폰 되었을 때 변수 초기화
    protected override void OnEnable()
    {
        m_MonsterMaxHP = 500;
        m_MonsterDamage = 10;
        m_MoveSpeed = 2.4f;
        m_AttackDelay = 1.5f;
        m_IsFire = false;
        m_MonsterNum = MONSTER_NUMBER.MONSTER03;
        m_MonsterExp = 30;
        m_MonsterSpawnNum = ObjectPoolingManager.m_Monster03Key;

        SetHpBar();

        m_Ani.SetBool("isWalk", true);

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
        if (m_IsAttack || m_IsFire)
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

        // 플레이어와의 거리가 6 이하라면 탄막패턴 사용, 아니라면 거리를 좁힘
        if (Vector3.Distance(m_PlayerTransform.position, transform.position) < 6f
            && !m_IsFire && !m_IsAttack && ObjectPoolingManager.Instance.IsSpawn)
        {
            StartCoroutine(FireCoroutine());
            return;
        }

        m_Ani.SetBool("isWalk", true);
        // 해당 방향을 향해 초당 m_MoveSpeed 만큼 이동
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    IEnumerator FireCoroutine()
    {
        m_IsFire = true;
        SoundManager.Instance.SoundPlay(SOUND_NAME.MONSTERSHOT1);

        GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_MonsterBulletKey);

        // 몬스터 불렛에 데미지 부여
        MonsterBullet pBullet = bullet.GetComponent<MonsterBullet>();
        pBullet.SetDamage(m_MonsterDamage);

        // 몬스터 불렛의 포지션을 플레이어의 방향을 통해 우측 총구, 혹은 좌측 총구로 이동
        bullet.transform.position = m_SpRen.flipX ? transform.position + (Vector3.right * 0.5f) : transform.position + (Vector3.left * 0.5f);

        // 몬스터 불렛의 RigidBody를 가져와서 마우스가 가리키는 방향을 향해 퍼지도록 랜덤한 값을 곱해서 발사
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        // 플레이어의 위치를 가리키는 방향 벡터
        Vector3 _dir = (m_PlayerTransform.position - transform.position).normalized;

        rigid.velocity = _dir * 6f;

        m_Ani.SetBool("isWalk", false);
        m_Ani.SetTrigger("Attack");
        
        yield return new WaitForSeconds(m_AttackDelay);
        m_IsFire = false;
    }

    // 몬스터 공격
    protected override IEnumerator MonsterAttack(GameObject _obj)
    {
        if (m_IsFire)
        {
            yield return new WaitForSeconds(0.1f);
            StopCoroutine(MonsterAttack(_obj));
        }
        m_Ani.SetBool("isWalk", false);

        base.MonsterAttack(_obj);
    }
    public override void MonsterOnDamage(int _damage)
    {
        base.MonsterOnDamage(_damage);
        SetHpBar();
    }

    void SetHpBar()
    {
        // 체력이 0보다 크거나 같다면 m_MonsterHP를, 0보다 작다면 0을 넣어줌
        float hp = (float)m_MonsterHP / (float)m_MonsterMaxHP;
        m_HpSpRen.color = m_MonsterHP >= 0 ? new Color(1, 1, 1, hp) : new Color(1, 1, 1, 0);
    }
}
