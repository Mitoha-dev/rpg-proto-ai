using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    public float startY = -1.1f;
    Animator anim;
    SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(x, 0, 0) * speed * Time.deltaTime;
        anim.SetBool("isMoving", x != 0);

        if (x > 0)
        {
            sr.flipX = true;
        }
        else if (x < 0)
        {
            sr.flipX = false;
        }
    }
}