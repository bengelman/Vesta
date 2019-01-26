using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private Vector2 velocity = new Vector2(0, 0);
    public float jumpSpeed = 5;
    public float speed = 10;
    private bool canJump;
    private bool hasJumped;
    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = true;
        hasJumped = false;
        HP = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (hasJumped) {
            canJump = false;
        } else {
            canJump = true;
        }

        if (Input.GetKeyDown("space") && canJump) {
            rb2d.velocity = new Vector2(0, 0);
            velocity = new Vector2(velocity.x, jumpSpeed);
            hasJumped = true;
        }
        if (moveHorizontal != 0)
        {
            Debug.Log("Walk");
            GetComponent<SpriteAnim>().PlayAnimation(1);
        }
        else
        {
            GetComponent<SpriteAnim>().PlayAnimation(0);
        }
        velocity = new Vector2(moveHorizontal * speed, Mathf.Min(rb2d.velocity.y + velocity.y, jumpSpeed));
        rb2d.velocity = velocity;
        velocity.y = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasJumped = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hasJumped = false;
    }

    public void TakeDamge(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {

        }
    }

}
