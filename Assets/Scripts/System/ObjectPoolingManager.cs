using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    // 오브젝트 풀링 매니저로 관리할 게임 오브젝트 프리펩
    [SerializeField]
    private GameObject m_PlayerBulletPrefab;
    [SerializeField]
    private GameObject m_PlayerBullet2Prefab;
    [SerializeField]
    private GameObject m_PlayerBullet3Prefab;
    [SerializeField]
    private GameObject m_MonsterBulletPrefab;
    [SerializeField]
    private GameObject m_MonsterBullet2Prefab;

    [SerializeField]
    private GameObject m_ExpItem0;
    [SerializeField]
    private GameObject m_ExpItem1;
    [SerializeField]
    private GameObject m_ExpItem2;
    [SerializeField]
    private GameObject m_ExpItem3;

    [SerializeField]
    private GameObject m_Monster00Prefab;
    [SerializeField]
    private GameObject m_Monster01Prefab;
    [SerializeField]
    private GameObject m_Monster02Prefab;
    [SerializeField]
    private GameObject m_Monster03Prefab;
    [SerializeField]
    private GameObject m_Monster04Prefab;
    [SerializeField]
    private GameObject m_Monster05Prefab;
    [SerializeField]
    private GameObject m_Monster06Prefab;
    [SerializeField]
    private GameObject m_Monster07Prefab;
    [SerializeField]
    private GameObject m_Monster08Prefab;
    [SerializeField]
    private GameObject m_Monster09Prefab;
    [SerializeField]
    private GameObject m_Monster10Prefab;
    [SerializeField]
    private GameObject m_Monster11Prefab;

    [SerializeField]
    private GameObject m_Boss00Prefab;
    [SerializeField]
    private GameObject m_Boss01Prefab;
    [SerializeField]
    private GameObject m_Boss02Prefab;
    [SerializeField]
    private GameObject m_Boss03Prefab;

    [SerializeField]
    private GameObject m_Skill01EffectPrefab;
    [SerializeField]
    private GameObject m_MonsterEffectPrefab;
    [SerializeField]
    private GameObject m_MonsterEffect2Prefab;
    [SerializeField]
    private GameObject m_BossEffectPrefab;


    // 게임 오브젝트를 넣어줄 Queue
    private Queue<GameObject> m_PlayerBulletQueue = new Queue<GameObject>();
    private Queue<GameObject> m_PlayerBullet2Queue = new Queue<GameObject>();
    private Queue<GameObject> m_PlayerBullet3Queue = new Queue<GameObject>();
    private Queue<GameObject> m_MonsterBulletQueue = new Queue<GameObject>();
    private Queue<GameObject> m_MonsterBullet2Queue = new Queue<GameObject>();

    private Queue<GameObject> m_ExpItem0Queue = new Queue<GameObject>();
    private Queue<GameObject> m_ExpItem1Queue = new Queue<GameObject>();
    private Queue<GameObject> m_ExpItem2Queue = new Queue<GameObject>();
    private Queue<GameObject> m_ExpItem3Queue = new Queue<GameObject>();

    private Queue<GameObject> m_Monster00Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster01Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster02Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster03Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster04Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster05Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster06Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster07Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster08Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster09Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster10Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Monster11Queue = new Queue<GameObject>();

    private Queue<GameObject> m_Boss00Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Boss01Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Boss02Queue = new Queue<GameObject>();
    private Queue<GameObject> m_Boss03Queue = new Queue<GameObject>();

    private Queue<GameObject> m_Skill01EffectQueue = new Queue<GameObject>();
    private Queue<GameObject> m_MonsterEffectQueue = new Queue<GameObject>();
    private Queue<GameObject> m_MonsterEffect2Queue = new Queue<GameObject>();
    private Queue<GameObject> m_BossEffectQueue = new Queue<GameObject>();

    // Dictionary 키 값 관리를 위한 상수
    // 변경할 수 없도록 const로 작성
    public const int m_PlayerBulletKey = 100;
    public const int m_PlayerBullet2Key = 101;
    public const int m_PlayerBullet3Key = 102;
    public const int m_MonsterBulletKey = 110;
    public const int m_MonsterBullet2Key = 111;

    public const int m_ExpItem0Key = 500;
    public const int m_ExpItem1Key = 501;
    public const int m_ExpItem2Key = 502;
    public const int m_ExpItem3Key = 503;

    public const int m_Monster00Key = 1000;
    public const int m_Monster01Key = 1001;
    public const int m_Monster02Key = 1002;
    public const int m_Monster03Key = 1003;
    public const int m_Monster04Key = 1004;
    public const int m_Monster05Key = 1005;
    public const int m_Monster06Key = 1006;
    public const int m_Monster07Key = 1007;
    public const int m_Monster08Key = 1008;
    public const int m_Monster09Key = 1009;
    public const int m_Monster10Key = 1010;
    public const int m_Monster11Key = 1011;

    public const int m_Boss00Key = 2000;
    public const int m_Boss01Key = 2001;
    public const int m_Boss02Key = 2002;
    public const int m_Boss03Key = 2003;

    public const int m_Skill01EffectKey = 10000;
    public const int m_MonsterEffectKey = 10010;
    public const int m_MonsterEffect2Key = 10011;
    public const int m_BossEffectKey = 10020;

    // Queue들을 관리하기 위한 Dictionary
    public Dictionary<int, Queue<GameObject>> m_queueDic = new Dictionary<int, Queue<GameObject>>();

    // 모든 오브젝트 풀 Queue가 초기화 되었는지 확인을 위한 변수
    private bool m_IsSpawn;
    public bool IsSpawn => m_IsSpawn;

    // 현재 소환된 몬스터 수 카운트
    public int m_MonsterCount;

    // 오브젝트 풀링 매니저 변수, 프로퍼티
    // 단일 객체만 유지하고, 다른 클래스에서 쉽게 사용할 수 있도록 싱글톤 패턴을 사용
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            // instance가 비어있을 경우 해당 객체를 넣어줌
            instance = this;
        }


        // 큐를 관리하기 위한 딕셔너리에 큐 추가
        m_queueDic.Add(m_PlayerBulletKey, m_PlayerBulletQueue);
        m_queueDic.Add(m_PlayerBullet2Key, m_PlayerBullet2Queue);
        m_queueDic.Add(m_PlayerBullet3Key, m_PlayerBullet3Queue);
        m_queueDic.Add(m_MonsterBulletKey, m_MonsterBulletQueue);
        m_queueDic.Add(m_MonsterBullet2Key, m_MonsterBullet2Queue);

        m_queueDic.Add(m_ExpItem0Key, m_ExpItem0Queue);
        m_queueDic.Add(m_ExpItem1Key, m_ExpItem1Queue);
        m_queueDic.Add(m_ExpItem2Key, m_ExpItem2Queue);
        m_queueDic.Add(m_ExpItem3Key, m_ExpItem3Queue);

        m_queueDic.Add(m_Monster00Key, m_Monster00Queue);
        m_queueDic.Add(m_Monster01Key, m_Monster01Queue);
        m_queueDic.Add(m_Monster02Key, m_Monster02Queue);
        m_queueDic.Add(m_Monster03Key, m_Monster03Queue);
        m_queueDic.Add(m_Monster04Key, m_Monster04Queue);
        m_queueDic.Add(m_Monster05Key, m_Monster05Queue);
        m_queueDic.Add(m_Monster06Key, m_Monster06Queue);
        m_queueDic.Add(m_Monster07Key, m_Monster07Queue);
        m_queueDic.Add(m_Monster08Key, m_Monster08Queue);
        m_queueDic.Add(m_Monster09Key, m_Monster09Queue);
        m_queueDic.Add(m_Monster10Key, m_Monster10Queue);
        m_queueDic.Add(m_Monster11Key, m_Monster11Queue);

        m_queueDic.Add(m_Boss00Key, m_Boss00Queue);
        m_queueDic.Add(m_Boss01Key, m_Boss01Queue);
        m_queueDic.Add(m_Boss02Key, m_Boss02Queue);
        m_queueDic.Add(m_Boss03Key, m_Boss03Queue);

        m_queueDic.Add(m_Skill01EffectKey, m_Skill01EffectQueue);
        m_queueDic.Add(m_MonsterEffectKey, m_MonsterEffectQueue);
        m_queueDic.Add(m_MonsterEffect2Key, m_MonsterEffect2Queue);
        m_queueDic.Add(m_BossEffectKey, m_BossEffectQueue);

        // 오브젝트 풀 초기화
        InitQueue(m_PlayerBulletPrefab, m_PlayerBulletQueue, 150);
        InitQueue(m_PlayerBullet2Prefab, m_PlayerBullet2Queue, 10);
        InitQueue(m_PlayerBullet3Prefab, m_PlayerBullet3Queue, 30);
        InitQueue(m_MonsterBulletPrefab, m_MonsterBulletQueue, 150);
        InitQueue(m_MonsterBullet2Prefab, m_MonsterBullet2Queue, 10);

        InitQueue(m_ExpItem0, m_ExpItem0Queue, 50);
        InitQueue(m_ExpItem1, m_ExpItem1Queue, 50);
        InitQueue(m_ExpItem2, m_ExpItem2Queue, 50);
        InitQueue(m_ExpItem3, m_ExpItem3Queue, 50);

        InitQueue(m_Monster00Prefab, m_Monster00Queue, 30);
        InitQueue(m_Monster01Prefab, m_Monster01Queue, 30);
        InitQueue(m_Monster02Prefab, m_Monster02Queue, 30);
        InitQueue(m_Monster03Prefab, m_Monster03Queue, 100);
        InitQueue(m_Monster04Prefab, m_Monster04Queue, 30);
        InitQueue(m_Monster05Prefab, m_Monster05Queue, 30);
        InitQueue(m_Monster06Prefab, m_Monster06Queue, 30);
        InitQueue(m_Monster07Prefab, m_Monster07Queue, 100);
        InitQueue(m_Monster08Prefab, m_Monster08Queue, 30);
        InitQueue(m_Monster09Prefab, m_Monster09Queue, 30);
        InitQueue(m_Monster10Prefab, m_Monster10Queue, 30);
        InitQueue(m_Monster11Prefab, m_Monster11Queue, 30);

        InitQueue(m_Boss00Prefab, m_Boss00Queue, 2);
        InitQueue(m_Boss01Prefab, m_Boss01Queue, 2);
        InitQueue(m_Boss02Prefab, m_Boss02Queue, 2);
        InitQueue(m_Boss03Prefab, m_Boss03Queue, 2);

        InitQueue(m_Skill01EffectPrefab, m_Skill01EffectQueue, 30);
        InitQueue(m_MonsterEffectPrefab, m_MonsterEffectQueue, 30);
        InitQueue(m_MonsterEffect2Prefab, m_MonsterEffect2Queue, 30);
        InitQueue(m_BossEffectPrefab, m_BossEffectQueue, 30);

        
        // 스폰 완료시 true
        m_IsSpawn = true;
        m_MonsterCount = 0;
    }

    // 오브젝트 풀 Queue에 오브젝트를 생성해서 채워주는 초기화 함수
    private void InitQueue(GameObject _obj, Queue<GameObject> _queue, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            // 예상하지 않은 충돌을 방지하기 위해 멀리 떨어진 곳에서 생성
            GameObject obj = Instantiate(_obj, new Vector3(5000, 5000), Quaternion.identity);
            _queue.Enqueue(obj);

            obj.SetActive(false);
        }
    }

    // 사용한 오브젝트를 다시 큐에 넣기 위한 함수
    public void InsertQueue(GameObject _obj, int _queueKey)
    {
        // 오브젝트의 속성 초기화
        Rigidbody2D rigid = _obj.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = 0f;
        }
        _obj.transform.rotation = Quaternion.identity;

        m_queueDic[_queueKey].Enqueue(_obj);
        _obj.SetActive(false);
    }

    // 오브젝트 풀에서 사용할 오브젝트를 꺼내는 함수
    public GameObject GetQueue(int _queueKey)
    {
        GameObject obj = m_queueDic[_queueKey].Dequeue();
        obj.SetActive(true);
        // 큐에 오브젝트가 남아있지 않으면 추가 생성
        if (m_queueDic[_queueKey].Count < 1)
        {
            InitQueue(obj, m_queueDic[_queueKey], 10);
        }
        return obj;
    }
}
