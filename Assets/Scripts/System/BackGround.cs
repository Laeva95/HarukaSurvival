using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Haruka m_Player;
    private void Awake()
    {
        m_Player = FindObjectOfType<Haruka>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = m_Player.transform.position;
        Vector3 BackPos = transform.position;

        // 플레이어 이동 방향
        float dirX = playerPos.x - BackPos.x;
        float dirY = playerPos.y - BackPos.y;

        // 플레이어와의 거리
        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        if (dirX != 0)
            dirX = dirX > 0 ? dirX = 1 : dirX = -1;

        if (dirY != 0)
            dirY = dirY > 0 ? dirY = 1 : dirY = -1;

        switch (transform.tag)
        {
            case "Ground":
               if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 50);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 50);
                }
                else
                {
                    transform.Translate(Vector3.up * dirY * 50);
                    transform.Translate(Vector3.right * dirX * 50);
                }
                break;
            case "Monster":
                break;
            default:
                break;
        }
    }
}
