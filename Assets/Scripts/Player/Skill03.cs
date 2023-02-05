using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill03 : Skill
{
    // ����
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
                    // ������Ʈ Ǯ�� �Ŵ������� �÷��̾� �ҷ��� ������
                    GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerBullet3Key);

                    // �÷��̾� �ҷ��� ������ �ο�
                    PlayerBullet3 pBullet = bullet.GetComponent<PlayerBullet3>();
                    pBullet.SetDamage((int)(m_GunManager.Damage * (1f + m_Level * 0.2f)));

                    // �÷��̾� �ҷ��� �������� ���� �÷��̾��� ��ġ�� �̵�
                    bullet.transform.position = transform.position;

                    // �ҷ��� �ӵ� �ο�
                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                    // ���⿡ ������ �ο�
                    float angle2 = angle + Random.Range(-15f, 15f);

                    // �ҷ��� ������ �������� �ο��� �������� ȸ��
                    bullet.transform.rotation = Quaternion.AngleAxis(angle2, Vector3.forward);

                    float rad = angle2 * Mathf.Deg2Rad;

                    Vector2 _dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                    // �ٶ󺸴� �������� �߻�
                    rigid.velocity = _dir * 15f;
                    yield return new WaitForSeconds(0.05f);
                }
            }
            yield return new WaitForSeconds(14f - (m_Level * 1f));
        }
    }
}
