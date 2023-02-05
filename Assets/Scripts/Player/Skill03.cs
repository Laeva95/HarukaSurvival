using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill03 : Skill
{
    // 히나
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            if (m_Player.TargetTF != null)
            {
                Vector2 dir = (m_Player.TargetTF.position - transform.position).normalized;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                int fireRate = (m_Level * 4) + 40;
                for (int i = 0; i < fireRate; i++)
                {
                    // 오브젝트 풀링 매니저에서 플레이어 불렛을 꺼내옴
                    GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerBullet3Key);

                    // 플레이어 불렛에 데미지 부여
                    PlayerBullet3 pBullet = bullet.GetComponent<PlayerBullet3>();
                    pBullet.SetDamage((int)(m_GunManager.Damage * (1f + m_Level * 0.2f)));

                    // 플레이어 불렛의 포지션을 현재 플레이어의 위치로 이동
                    bullet.transform.position = transform.position;

                    // 불렛에 속도 부여
                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                    // 방향에 랜덤성 부여
                    float angle2 = angle + Random.Range(-15f, 15f);

                    // 불렛의 각도를 랜덤성이 부여된 방향으로 회전
                    bullet.transform.rotation = Quaternion.AngleAxis(angle2, Vector3.forward);

                    float rad = angle2 * Mathf.Deg2Rad;

                    Vector2 _dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                    // 바라보는 방향으로 발사
                    rigid.velocity = _dir * 15f;
                    yield return new WaitForSeconds(0.05f);
                }
            }
            yield return new WaitForSeconds(14f - (m_Level * 1f));
        }
    }
}
