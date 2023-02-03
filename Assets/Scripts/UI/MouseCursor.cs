using UnityEngine;
using System.Collections;
 
public class MouseCursor : MonoBehaviour
{
    //���콺 �����ͷ� ����� �ؽ�ó
    public Texture2D cursorTexture;

    //���ο��� ����� �ʵ�
    private Vector2 m_MouserVec;

    public void Start()
    {
        // ���� �� �ڷ�ƾ ȣ��
        StartCoroutine("MyCursor");
    }

    IEnumerator MyCursor()
    {
        //��� �������� �Ϸ�� ������ ���
        yield return new WaitForEndOfFrame();

        m_MouserVec.x = cursorTexture.width / 2;
        m_MouserVec.y = cursorTexture.height / 2;

        //���� ���ο� ���콺 Ŀ���� ȭ�鿡 ǥ���մϴ�.
        Cursor.SetCursor(cursorTexture, m_MouserVec, CursorMode.Auto);
    }
}