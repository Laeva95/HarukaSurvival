using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill01 : Skill
{
    // 아루
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            if (m_Player.TargetTF != null)
            {
                // 오브젝트 풀링 매니저에서 플레이어 불렛을 꺼내옴
                GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerBullet2Key);

                // 플레이어 불렛에 데미지 부여
                PlayerBullet2 pBullet = bullet.GetComponent<PlayerBullet2>();
                pBullet.SetDamage((int)(m_GunManager.Damage * (2f + m_Level * 2f)));
                pBullet.SetBoom(true);

                // 플레이어 불렛의 포지션을 현재 플레이어의 위치로 이동
                bullet.transform.position = transform.position;

                // 플레이어 불렛의 RigidBody를 가져와서 가장 가까운 적을 향해 발사
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                Vector2 dir = (m_Player.TargetTF.position - transform.position).normalized;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                rigid.velocity = dir * 8f;
            }

            yield return new WaitForSeconds(11.5f - (m_Level * 1f));
        }
    }
}
