using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    // ����Ű UI
    [SerializeField]
    GameObject m_KeyButtonUI;
    // ���� ���� UI
    [SerializeField]
    GameObject m_GameButtonUI;

    private void Awake()
    {
        // �׻� �ִ� ������ ����
        Application.targetFrameRate = 60;
        //StartCoroutine(SetResoutionCoroutine());
    }
    // ���� ���� ��ư
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    // Ű ���� ��ư
    public void KeyDescription()
    {
        m_KeyButtonUI.SetActive(true);
    }
    // Ű ���� ���� ��ư
    public void EndKeyDescription()
    {
        m_KeyButtonUI.SetActive(false);
    }

    // ���� ���� ��ư
    public void GameDescription()
    {
        m_GameButtonUI.SetActive(true);
    }

    // ���� ���� ���� ��ư
    public void EndGameDescription()
    {
        m_GameButtonUI.SetActive(false);
    }

    void SetResolution()
    {
        int setWidth = 1600; // ����� ���� �ʺ�
        int setHeight = 900; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), Application.isMobilePlatform); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

    IEnumerator SetResoutionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        SetResolution();
    }
}
