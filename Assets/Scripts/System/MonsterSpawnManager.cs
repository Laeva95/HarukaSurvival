using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 몬스터 스폰 관리 클래스
public class MonsterSpawnManager : MonoBehaviour
{
    // 플레이어 Transform
    [SerializeField]
    private Transform m_Player;

    // 몬스터 스폰 가능 최대치
    private int m_monsterCountMax = 80;

    void Start()
    {
        StartCoroutine(MonsterSpawn());
    }
    IEnumerator MonsterSpawn()
    {
        yield return null;

        MonsterGenerate(25, ObjectPoolingManager.m_Monster00Key);

        // 게임이 진행중일때만 실행
        while (GameManager.Instance.IsPlay)
        {
            // 몬스터 생성
            // 0~300초
            if (GameManager.Instance.PlayTime < 300 && !GameManager.Instance.IsBoss)
            {
                // 0~15
                if (GameManager.Instance.PlayTime < 15)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster00Key);
                }
                // 15~30
                else if (GameManager.Instance.PlayTime < 30)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster00Key);
                }
                // 30~60
                else if (GameManager.Instance.PlayTime < 60)
                {
                    MonsterGenerate(3, ObjectPoolingManager.m_Monster00Key);
                }
                // 60~120
                else if (GameManager.Instance.PlayTime < 120)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster00Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster01Key);
                }
                // 120~180
                else if (GameManager.Instance.PlayTime < 180)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster00Key);
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster01Key);
                }
                // 180~240
                else if (GameManager.Instance.PlayTime < 240)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster01Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster02Key);
                }
                // 240~300
                else if (GameManager.Instance.PlayTime < 300)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster01Key);
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster02Key);
                }
            }
            // 301~600초
            else if (GameManager.Instance.PlayTime >= 301 && GameManager.Instance.PlayTime < 600 && !GameManager.Instance.IsBoss)
            {
                // 300~360
                if (GameManager.Instance.PlayTime < 360)
                {
                    MonsterGenerate(5, ObjectPoolingManager.m_Monster01Key);
                }
                // 360~420
                else if (GameManager.Instance.PlayTime < 420)
                {
                    MonsterGenerate(3, ObjectPoolingManager.m_Monster01Key);

                    if ((int)(GameManager.Instance.PlayTime % 3) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster04Key);
                    }
                }
                // 420~480
                else if (GameManager.Instance.PlayTime < 480)
                {
                    MonsterGenerate(3, ObjectPoolingManager.m_Monster02Key);

                    if ((int)(GameManager.Instance.PlayTime % 2) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster04Key);
                    }
                }
                // 480~540
                else if (GameManager.Instance.PlayTime < 540)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster04Key);
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster02Key);
                }
                // 540~600
                else if (GameManager.Instance.PlayTime < 600)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster04Key);
                    if ((int)(GameManager.Instance.PlayTime % 10) == 0)
                    {
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster05Key);
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster03Key);
                    }

                }
                // 301초부터 600초까지 10초마다 
                if ((int)(GameManager.Instance.PlayTime % 10) == 0)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster03Key);
                }
            }
            // 601~900초
            else if (GameManager.Instance.PlayTime >= 601 && GameManager.Instance.PlayTime < 900 && !GameManager.Instance.IsBoss)
            {
                // 600~660
                if (GameManager.Instance.PlayTime < 660)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster04Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster01Key);
                    if ((int)(GameManager.Instance.PlayTime % 8) == 0)
                    {
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster05Key);
                    }
                }
                // 660~720
                else if (GameManager.Instance.PlayTime < 720)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster04Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster06Key);
                    if ((int)(GameManager.Instance.PlayTime % 7) == 0)
                    {
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster05Key);
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster08Key);
                    }
                }
                // 720~780
                else if (GameManager.Instance.PlayTime < 780)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster06Key);
                    if ((int)(GameManager.Instance.PlayTime % 4) == 0)
                    {
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster05Key);
                    }
                    if ((int)(GameManager.Instance.PlayTime % 6) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster08Key);
                    }
                }
                // 780~840
                else if (GameManager.Instance.PlayTime < 840)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster06Key);
                    if ((int)(GameManager.Instance.PlayTime % 3) == 0)
                    {
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster05Key);
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster08Key);
                    }
                }
                // 840~900
                else if (GameManager.Instance.PlayTime < 900)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster06Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster05Key);
                    if ((int)(GameManager.Instance.PlayTime % 2) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster08Key);
                    }
                }

                // 601초부터 900초까지 10초마다 
                if ((int)(GameManager.Instance.PlayTime % 10) == 0)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster07Key);
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster06Key);
                }
            }
            // 901~1200초
            else if (GameManager.Instance.PlayTime >= 901 && GameManager.Instance.PlayTime < 1200 && !GameManager.Instance.IsBoss)
            {
                // 900~960
                if (GameManager.Instance.PlayTime < 960)
                {
                    MonsterGenerate(5, ObjectPoolingManager.m_Monster01Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster11Key);
                    if ((int)(GameManager.Instance.PlayTime % 8) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster05Key);
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster08Key);
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster03Key);
                    }
                }
                // 960~1020
                else if (GameManager.Instance.PlayTime < 1020)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster11Key);
                    MonsterGenerate(3, ObjectPoolingManager.m_Monster06Key);
                    if ((int)(GameManager.Instance.PlayTime % 6) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster05Key);
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster08Key);
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster03Key);
                    }
                }
                // 1020~1080
                else if (GameManager.Instance.PlayTime < 1080)
                {
                    MonsterGenerate(2, ObjectPoolingManager.m_Monster11Key);
                    if ((int)(GameManager.Instance.PlayTime % 4) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster05Key);
                    }
                    if ((int)(GameManager.Instance.PlayTime % 6) == 0)
                    {
                        MonsterGenerate(2, ObjectPoolingManager.m_Monster09Key);
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster07Key);
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster03Key);
                    }
                }
                // 1080~1140
                else if (GameManager.Instance.PlayTime < 1140)
                {
                    MonsterGenerate(3, ObjectPoolingManager.m_Monster11Key);
                    if ((int)(GameManager.Instance.PlayTime % 3) == 0)
                    {
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster09Key);
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster08Key);
                        MonsterGenerate(1, ObjectPoolingManager.m_Monster07Key);
                    }
                }
                // 1140~1200
                else if (GameManager.Instance.PlayTime < 1200)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster04Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster05Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster06Key);
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster11Key);

                    MonsterGenerate(1, ObjectPoolingManager.m_Monster09Key);

                    MonsterGenerate(1, ObjectPoolingManager.m_Monster07Key);
                }

                // 901초부터 1200초까지 10초마다 
                if ((int)(GameManager.Instance.PlayTime % 10) == 0)
                {
                    MonsterGenerate(1, ObjectPoolingManager.m_Monster10Key);
                }
            }
            // 보스00  생성
            if (((int)GameManager.Instance.PlayTime == 300 || (int)GameManager.Instance.PlayTime == 301) 
                && GameManager.Instance.PlayTime < 600 && !GameManager.Instance.IsBoss)
            {
                MonsterGenerate(1, ObjectPoolingManager.m_Boss00Key);
                MonsterGenerate(10, ObjectPoolingManager.m_Monster02Key);
                GameManager.Instance.BossSpawn();
            }
            // 보스01  생성
            if (((int)GameManager.Instance.PlayTime == 600 || (int)GameManager.Instance.PlayTime == 601) 
                && GameManager.Instance.PlayTime < 1200 && !GameManager.Instance.IsBoss)
            {
                MonsterGenerate(1, ObjectPoolingManager.m_Boss01Key);
                MonsterGenerate(10, ObjectPoolingManager.m_Monster04Key);
                GameManager.Instance.BossSpawn();
            }
            // 보스02  생성
            if (((int)GameManager.Instance.PlayTime == 900 || (int)GameManager.Instance.PlayTime == 901) 
                && GameManager.Instance.PlayTime < 1800 && !GameManager.Instance.IsBoss)
            {
                MonsterGenerate(1, ObjectPoolingManager.m_Boss02Key);
                MonsterGenerate(10, ObjectPoolingManager.m_Monster06Key);
                GameManager.Instance.BossSpawn();
            }
            // 보스03  생성
            if (((int)GameManager.Instance.PlayTime == 1200 || (int)GameManager.Instance.PlayTime == 1201) 
                && GameManager.Instance.PlayTime < 2400 && !GameManager.Instance.IsBoss)
            {
                MonsterGenerate(1, ObjectPoolingManager.m_Boss03Key);
                GameManager.Instance.BossSpawn();
            }
            // 1초 대기 후 소환 반복
            yield return new WaitForSeconds(1f);
        }
    }

    // Monster 생성 함수
    private void MonsterGenerate(int _count, int _monsterNum)
    {
        for (int i = 0; i < _count; i++)
        {
            // 현재 존재하는 몬스터가 최대 카운터 이상이고, 소환하려는 몬스터가 보스가 아닐 경우 소환하지 않음
            if (ObjectPoolingManager.Instance.m_MonsterCount >= m_monsterCountMax && _monsterNum < ObjectPoolingManager.m_Boss00Key)
            {
                return;
            }

            // 랜덤한 방향을 설정
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            Vector3 dir = new Vector3(x, y, 0).normalized;

            // 플레이어 위치에서 일정 거리 떨어진 위치에 몬스터 생성
            GameObject obj = ObjectPoolingManager.Instance.GetQueue(_monsterNum);
            obj.transform.position = m_Player.position + dir * 12;
        }
    }
}
