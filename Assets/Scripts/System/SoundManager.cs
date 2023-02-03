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
    // ���� �Ŵ��� ����, ������Ƽ
    // ���� ��ü�� �����ϰ�, �ٸ� Ŭ�������� ���� ����� �� �ֵ��� �̱��� ������ ���
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
    
    // ����� Ŭ�� �迭(�ν����� â���� �־���)
    [SerializeField]
    private AudioClip[] m_Clips;
    // ����� �ҽ� �迭
    private AudioSource[] m_Audio;
    // ���� ���� ������ ���� �����̴�
    [SerializeField]
    private Slider m_VolumeSlider;
    // ���� ũ�� ����
    private float m_Volume;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        // SoundManager ������Ʈ�� ������ AudioSource ������Ʈ�� ��� ������
        m_Audio = GetComponents<AudioSource>();

        // PlayerPrefs�� ����� float���� ������(���� ��� �⺻�� 0.1f)
        m_Volume = PlayerPrefs.GetFloat("volume", 0.2f);

        // �����̴� ���� ������ flaot������ ����
        m_VolumeSlider.value = m_Volume;

        // ��� ����� �ҽ��� ������ ������ float ������ ����
        for (int i = 0; i < m_Audio.Length; i++)
        {
            m_Audio[i].volume = m_VolumeSlider.value;
        }
    }

    // ���� ��� �Լ�
    public void SoundPlay(SOUND_NAME _NAME)
    {
        // for���� ���� ����� �ҽ� ������ ����
        for (int i = 0; i < m_Audio.Length; i++)
        {
            // ���� ����� �ҽ��� ��� ���� ��� ���� ����� �ҽ��� Ȯ��
            if (m_Audio[i].isPlaying)
            {
                if (m_Audio[i].clip == m_Clips[(int)_NAME])
                {
                    // ������ Ŭ���� ������ϰ�� ���� �� �ٽ� ���
                    m_Audio[i].Stop();
                    m_Audio[i].Play();
                    return;
                }
                continue;
            }
            // ����� �ҽ��� ��� ������ ���� ���
            // �Ű������� ���� ������ ���� ������ҽ��� Ŭ�� ����
            m_Audio[i].clip = m_Clips[(int)_NAME];

            if (m_Audio[i].clip != null)
            {
                // ����� Ŭ�� ��� �� ����
                m_Audio[i].Play();
                return;
            }
        }
    }
    // ���� �����̴��� ���� ��ȭ�� ���� �� ȣ���
    public void VolumeChange()
    {
        // ��� ����� �ҽ��� ������ �����̴� ������ ����
        for (int i = 0; i < m_Audio.Length; i++)
        {
            m_Audio[i].volume = m_VolumeSlider.value;
        }

        // ���� ������ �� ���� ����
        m_Volume = m_VolumeSlider.value;

        // PlayerPrefs�� float �� ����
        PlayerPrefs.SetFloat("volume", m_Volume);
    }
}
