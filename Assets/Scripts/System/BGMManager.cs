using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    // 배경 음악 관리 매니저 클래스

    [SerializeField]
    private AudioSource m_Audio;
    // 사운드 볼륨 조절을 위한 슬라이더
    [SerializeField]
    private Slider m_VolumeSlider;
    // 볼륨 크기 변수
    private float m_Volume;

    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        // PlayerPrefs에 저장된 float값을 가져옴(없을 경우 기본값 0.1f)
        m_Volume = PlayerPrefs.GetFloat("bgm", 0.1f);

        // 슬라이더 값을 가져온 flaot값으로 설정
        m_VolumeSlider.value = m_Volume;

        m_Audio.volume = m_VolumeSlider.value;
    }

    // 볼륨 슬라이더의 값에 변화가 있을 때 호출됨
    public void VolumeChange()
    {
        // 오디오 소스의 볼륨을 슬라이더 값으로 설정
        m_Audio.volume = m_VolumeSlider.value;

        // 볼륨 변수에 이 값을 저장
        m_Volume = m_VolumeSlider.value;

        // PlayerPrefs에 float 값 저장
        PlayerPrefs.SetFloat("bgm", m_Volume);
    }
}
