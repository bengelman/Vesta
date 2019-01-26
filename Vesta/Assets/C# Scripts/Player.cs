using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private Vector2 velocity = new Vector2(0, 0);
    private Vector2 affectedVelocity = new Vector2(0, 0);
    public float jumpSpeed = 5;
    public float speed = 10;
    private bool canJump;
    private bool hasJumped;
    public bool isPlayer1;
    public int resources;
    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = true;
        hasJumped = false;
        HP = 1000;
        //isPlayer1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = 0;
        if (hasJumped) {
            canJump = false;
        } else {
            canJump = true;
        }

        if (Input.GetKey(isPlayer1 ? KeyCode.A : KeyCode.LeftArrow))
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(isPlayer1 ? KeyCode.D : KeyCode.RightArrow))
        {
            moveHorizontal = 1;
        }

        if (Input.GetKeyDown(isPlayer1 ? KeyCode.W : KeyCode.UpArrow) && canJump) {
            rb2d.velocity = new Vector2(0, 0);
            velocity = new Vector2(velocity.x, jumpSpeed);
            hasJumped = true;
        }
        if (moveHorizontal != 0 && !hasJumped)
        {
            Debug.Log("Walk");
            GetComponent<SpriteAnim>().PlayAnimation(1);
        }
        else
        {
            GetComponent<SpriteAnim>().PlayAnimation(0);
        }

        if (Input.GetKeyDown(isPlayer1 ? KeyCode.Q : KeyCode.Period))
        {
            DealDamage(50);
        } else if (Input.GetKeyDown(isPlayer1 ? KeyCode.E : KeyCode.Backslash)) {
            DealDamage(200);
        }

        /**** For damage testing
        if (Input.GetKeyDown(KeyCode.J)) {
            TakeDamge(10, "left");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamge(10, "right");
        }
        */

        if (moveHorizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (moveHorizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        velocity = new Vector2(moveHorizontal * speed, Mathf.Min(rb2d.velocity.y + velocity.y, jumpSpeed));
        rb2d.velocity = velocity + affectedVelocity;
        velocity.y = 0;
        affectedVelocity /= 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasJumped = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hasJumped = false;
    }

    public void TakeDamge(float damage, string dmgDir)
    {
        HP -= damage;

        if (dmgDir == "left") {
            affectedVelocity = new Vector2(-4, 1) * Mathf.Log(damage);
        } else {
            affectedVelocity = new Vector2(4, 1) * Mathf.Log(damage);
        }

        if (HP <= 0)
        {

        }
    }

    public void DealDamage(float damage)
    {
        bool direction = GetComponent<SpriteRenderer>().flipX; // true == left, false == right

        RaycastHit2D attack = Physics2D.Raycast((Vector2)transform.position + (direction ? Vector2.left * 1f/2f : Vector2.right), direction ? Vector2.left : Vector2.right, 1f/2f);

        if (attack.collider != null) {
            attack.collider.GetComponent<Player>().TakeDamge(damage, direction ? "left" : "right");
        }
    }

}
