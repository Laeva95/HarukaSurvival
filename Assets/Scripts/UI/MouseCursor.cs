using UnityEngine;
using System.Collections;
 
public class MouseCursor : MonoBehaviour
{
    //마우스 포인터로 사용할 텍스처
    public Texture2D cursorTexture;

    //내부에서 사용할 필드
    private Vector2 m_MouserVec;

    public void Start()
    {
        // 시작 시 코루틴 호출
        StartCoroutine("MyCursor");
    }

    IEnumerator MyCursor()
    {
        //모든 렌더링이 완료될 때까지 대기
        yield return new WaitForEndOfFrame();

        m_MouserVec.x = cursorTexture.width / 2;
        m_MouserVec.y = cursorTexture.height / 2;

        //이제 새로운 마우스 커서를 화면에 표시합니다.
        Cursor.SetCursor(cursorTexture, m_MouserVec, CursorMode.Auto);
    }
}