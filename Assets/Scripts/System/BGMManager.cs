using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    // ��� ���� ���� �Ŵ��� Ŭ����

    [SerializeField]
    private AudioSource m_Audio;
    // ���� ���� ������ ���� �����̴�
    [SerializeField]
    private Slider m_VolumeSlider;
    // ���� ũ�� ����
    private float m_Volume;

    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        // PlayerPrefs�� ����� float���� ������(���� ��� �⺻�� 0.1f)
        m_Volume = PlayerPrefs.GetFloat("bgm", 0.1f);

        // �����̴� ���� ������ flaot������ ����
        m_VolumeSlider.value = m_Volume;

        m_Audio.volume = m_VolumeSlider.value;
    }

    // ���� �����̴��� ���� ��ȭ�� ���� �� ȣ���
    public void VolumeChange()
    {
        // ����� �ҽ��� ������ �����̴� ������ ����
        m_Audio.volume = m_VolumeSlider.value;

        // ���� ������ �� ���� ����
        m_Volume = m_VolumeSlider.value;

        // PlayerPrefs�� float �� ����
        PlayerPrefs.SetFloat("bgm", m_Volume);
    }
}
