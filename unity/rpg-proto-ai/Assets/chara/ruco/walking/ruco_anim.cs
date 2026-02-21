using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    Animator anim;
    SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        // 移動
        transform.position += (Vector3)dir.normalized * speed * Time.deltaTime;

        // アニメ切り替え
        anim.SetBool("isMoving", dir.magnitude > 0.1f);

        // ★ 左右反転（ここが重要）
        if (x > 0)
        {
            sr.flipX = true;   // 右向き
        }
        else if (x < 0)
        {
            sr.flipX = false;  // 左向き（元の向き）
        }
    }
}