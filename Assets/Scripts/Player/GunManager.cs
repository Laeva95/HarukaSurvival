using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    // 플레이어 공격과 관련된 각종 변수를 담당하는 클래스

    private int m_Damage;                    // 데미지
    private int m_FireRate;                  // 발사 횟수
    private int m_FireRate2;                 // 발사 횟수 2
    private int m_BossPenetration;               // 관통 횟수
    private int m_Penetration;              // 확률 관통
    private float m_FireDelay;               // 발사 간격
    private float m_Accuracy;                // 정확도
    private bool m_IsDamageUp;               // 기본공격 위력 증가

    // 캡슐화를 위한 get 프로퍼티
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
        // 초기값 부여
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
        // 매개변수 만큼 데미지 증가
        m_Damage += _damage;
    }
    public void SetFireRate(int _fireRate)
    {
        // 매개변수 만큼 발사 횟수 증가
        m_FireRate += _fireRate;
    }
    public void SetFireRate2(int _fireRate)
    {
        m_FireRate2 += _fireRate;
    }
    public void SetPenetration()
    {
        // 관통 증가
        m_BossPenetration++;
    }
    public void SetPenetration2()
    {
        // 확률 관통 증가
        m_Penetration++;
    }
    public void SetFireDelay(float _del)
    {
        // 매개변수만큼 공격 딜레이 감소
        // 0.3 이하로 내려갔다면 0.3으로 고정
        m_FireDelay *= _del;
        m_FireDelay = m_FireDelay <= 0.3f ? 0.3f : m_FireDelay;
    }
    public void SetAccuracy(float _acc)
    {
        // 매개변수만큼 명중 증가, 감소
        // 0.8보다 크다면 0.85로, 0.2보다 작다면 0.15로 고정
        m_Accuracy *= _acc;

        m_Accuracy = m_Accuracy > 0.85f ? 0.85f : m_Accuracy;
        m_Accuracy = m_Accuracy < 0.15f ? 0.15f : m_Accuracy;
    }
    public void SetIsDamageUp()
    {
        // 기본공격 데미지 증가 업그레이드
        m_IsDamageUp = true;
    }
}
