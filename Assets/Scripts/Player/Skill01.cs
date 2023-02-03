using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill01 : Skill
{
    // �Ʒ�
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            if (m_Player.TargetTF != null)
            {
                // ������Ʈ Ǯ�� �Ŵ������� �÷��̾� �ҷ��� ������
                GameObject bullet = ObjectPoolingManager.Instance.GetQueue(ObjectPoolingManager.m_PlayerBullet2Key);

                // �÷��̾� �ҷ��� ������ �ο�
                PlayerBullet2 pBullet = bullet.GetComponent<PlayerBullet2>();
                pBullet.SetDamage((int)(m_GunManager.Damage * (2f + m_Level * 2f)));
                pBullet.SetBoom(true);

                // �÷��̾� �ҷ��� �������� ���� �÷��̾��� ��ġ�� �̵�
                bullet.transform.position = transform.position;

                // �÷��̾� �ҷ��� RigidBody�� �����ͼ� ���� ����� ���� ���� �߻�
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
