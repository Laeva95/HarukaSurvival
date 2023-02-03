using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill02 : Skill
{
    [SerializeField]
    private GameObject[] m_Effects;
    // 무츠키
    protected override IEnumerator StartSkill()
    {
        while (GameManager.Instance.IsPlay)
        {
            for (int i = 0; i < m_Effects.Length; i++)
            {
                // 랜덤한 방향을 설정
                float x = Random.Range(-4f, 4f);
                float y = Random.Range(-4f, 4f);

                Vector3 dir = new Vector3(x, y, 0);

                // 랜덤한 위치에서 활성화
                m_Effects[i].transform.position = m_Player.transform.position + dir;
                m_Effects[i].SetActive(true);

                // Mine 스크립트를 가져옴
                Mine mine = m_Effects[i].GetComponent<Mine>();

                // 데미지 부여
                mine.SetDamage((int)(m_GunManager.Damage * (1.5f + m_Level * 1.5f)));

                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < m_Effects.Length; i++)
            {
                // Mine 스크립트를 가져옴
                Mine mine = m_Effects[i].GetComponent<Mine>();

                // 설치 후 준비 상태를 true
                mine.SetBoom(true);
            }

            yield return new WaitForSeconds(12f - (m_Level * 1f));

            for (int i = 0; i < m_Effects.Length; i++)
            {
                // Mine 스크립트를 가져옴
                Mine mine = m_Effects[i].GetComponent<Mine>();

                mine.Explosion();
            }
        }
    }
}
