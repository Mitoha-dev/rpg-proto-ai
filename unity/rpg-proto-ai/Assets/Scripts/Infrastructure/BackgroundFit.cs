using UnityEngine;

public class BackgroundFit : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // カメラの高さ・幅に合わせてスケールを調整
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float spriteHeight = sr.sprite.bounds.size.y;
        float spriteWidth = sr.sprite.bounds.size.x;

        transform.localScale = new Vector3(
            camWidth / spriteWidth,
            camHeight / spriteHeight,
            1f
        );

        // カメラの中央に配置
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 1f);
    }
}