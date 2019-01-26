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

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = true;
        hasJumped = false;
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

        if (Input.GetKeyDown("Jump") && canJump) {
            velocity = new Vector2(velocity.x, jumpSpeed);
            hasJumped = true;
        }

        velocity = new Vector2(moveHorizontal * speed, velocity.y);
        rb2d.AddForce(velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasJumped = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hasJumped = false;
    }

}
