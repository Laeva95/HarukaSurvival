using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SOUND_NAME
{
    GAMECLEAR = 0,
    GAMEOVER0 = 1,
    GAMEOVER1 = 2,
    PLAYERHEAL = 3,
    PLAYERLEVELUP = 4,
    MONSTERSHOT0 = 5,
    MONSTERSHOT1 = 6,
    PLAYERHIT = 7,
    PLAYERSHOT = 8,
    UI = 9,
    MONSTERDEAD0 = 10,

}
public class SoundManager : MonoBehaviour
{
    // 사운드 매니저 변수, 프로퍼티
    // 단일 객체만 유지하고, 다른 클래스에서 쉽게 사용할 수 있도록 싱글톤 패턴을 사용
    private static SoundManager instance;
    public static SoundManager Instance
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
    
    // 오디오 클립 배열(인스펙터 창에서 넣어줌)
    [SerializeField]
    private AudioClip[] m_Clips;
    // 오디오 소스 배열
    private AudioSource[] m_Audio;
    // 사운드 볼륨 조절을 위한 슬라이더
    [SerializeField]
    private Slider m_VolumeSlider;
    // 볼륨 크기 변수
    private float m_Volume;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        // SoundManager 오브젝트가 보유한 AudioSource 컴포넌트를 모두 가져옴
        m_Audio = GetComponents<AudioSource>();

        // PlayerPrefs에 저장된 float값을 가져옴(없을 경우 기본값 0.1f)
        m_Volume = PlayerPrefs.GetFloat("volume", 0.2f);

        // 슬라이더 값을 가져온 flaot값으로 설정
        m_VolumeSlider.value = m_Volume;

        // 모든 오디오 소스의 볼륨을 가져온 float 값으로 설정
        for (int i = 0; i < m_Audio.Length; i++)
        {
            m_Audio[i].volume = m_VolumeSlider.value;
        }
    }

    // 사운드 재생 함수
    public void SoundPlay(SOUND_NAME _NAME)
    {
        // for문을 통해 오디오 소스 각각에 접근
        for (int i = 0; i < m_Audio.Length; i++)
        {
            // 현재 오디오 소스가 재생 중일 경우 다음 오디오 소스를 확인
            if (m_Audio[i].isPlaying)
            {
                if (m_Audio[i].clip == m_Clips[(int)_NAME])
                {
                    // 동일한 클립이 재생중일경우 정지 후 다시 재생
                    m_Audio[i].Stop();
                    m_Audio[i].Play();
                    return;
                }
                continue;
            }
            // 오디오 소스가 재생 중이지 않을 경우
            // 매개변수로 들어온 정보에 따라 오디오소스의 클립 변경
            m_Audio[i].clip = m_Clips[(int)_NAME];

            if (m_Audio[i].clip != null)
            {
                // 변경된 클립 재생 후 리턴
                m_Audio[i].Play();
                return;
            }
        }
    }
    // 볼륨 슬라이더의 값에 변화가 있을 때 호출됨
    public void VolumeChange()
    {
        // 모든 오디오 소스의 볼륨을 슬라이더 값으로 설정
        for (int i = 0; i < m_Audio.Length; i++)
        {
            m_Audio[i].volume = m_VolumeSlider.value;
        }

        // 볼륨 변수에 이 값을 저장
        m_Volume = m_VolumeSlider.value;

        // PlayerPrefs에 float 값 저장
        PlayerPrefs.SetFloat("volume", m_Volume);
    }
}
