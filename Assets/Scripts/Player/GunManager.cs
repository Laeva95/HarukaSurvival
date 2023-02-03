using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    // �÷��̾� ���ݰ� ���õ� ���� ������ ����ϴ� Ŭ����

    private int m_Damage;                    // ������
    private int m_FireRate;                  // �߻� Ƚ��
    private int m_FireRate2;                 // �߻� Ƚ�� 2
    private int m_BossPenetration;               // ���� Ƚ��
    private int m_Penetration;              // Ȯ�� ����
    private float m_FireDelay;               // �߻� ����
    private float m_Accuracy;                // ��Ȯ��
    private bool m_IsDamageUp;               // �⺻���� ���� ����

    // ĸ��ȭ�� ���� get ������Ƽ
    public int Damage => m_Damage;
    public int FireRate => m_FireRate;
    public int FireRate2 => m_FireRate2;
    public int Penetration => m_BossPenetration;
    public int Penetration2 => m_Penetration;
    public float FireDelay => m_FireDelay;
    public float Accuracy => m_Accuracy;
    public bool IsDamageUp => m_IsDamageUp;

    private void Awake()
    {
        // �ʱⰪ �ο�
        m_Damage = 20;
        m_FireRate = 5;
        m_FireRate2 = 0;
        m_BossPenetration = 0;
        m_Penetration = 0;
        m_FireDelay = 1f;
        m_Accuracy = 0.5f;
        m_IsDamageUp = false;
    }
    public void SetDamage(int _damage)
    {
        // �Ű����� ��ŭ ������ ����
        m_Damage += _damage;
    }
    public void SetFireRate(int _fireRate)
    {
        // �Ű����� ��ŭ �߻� Ƚ�� ����
        m_FireRate += _fireRate;
    }
    public void SetFireRate2(int _fireRate)
    {
        m_FireRate2 += _fireRate;
    }
    public void SetPenetration()
    {
        // ���� ����
        m_BossPenetration++;
    }
    public void SetPenetration2()
    {
        // Ȯ�� ���� ����
        m_Penetration++;
    }
    public void SetFireDelay(float _del)
    {
        // �Ű�������ŭ ���� ������ ����
        // 0.3 ���Ϸ� �������ٸ� 0.3���� ����
        m_FireDelay *= _del;
        m_FireDelay = m_FireDelay <= 0.3f ? 0.3f : m_FireDelay;
    }
    public void SetAccuracy(float _acc)
    {
        // �Ű�������ŭ ���� ����, ����
        // 0.8���� ũ�ٸ� 0.85��, 0.2���� �۴ٸ� 0.15�� ����
        m_Accuracy *= _acc;

        m_Accuracy = m_Accuracy > 0.85f ? 0.85f : m_Accuracy;
        m_Accuracy = m_Accuracy < 0.15f ? 0.15f : m_Accuracy;
    }
    public void SetIsDamageUp()
    {
        // �⺻���� ������ ���� ���׷��̵�
        m_IsDamageUp = true;
    }
}
