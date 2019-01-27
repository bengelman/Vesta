using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private Vector2 velocity = new Vector2(0, 0);
    public float jumpSpeed = 5;
    public float speed = 10;
    public const float maxHP = 1000;
    public float HP;
    public int resources;
    private bool canJump;
    private bool hasJumped;
    public bool isPlayer1;
    public bool isFrozen;
    public bool knockedBack;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = true;
        hasJumped = false;
        HP = maxHP;
        isFrozen = false;
        knockedBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen) return;

        float moveHorizontal = 0;
        if (hasJumped) {
            canJump = false;
        } else {
            canJump = true;
        }
        
        if (Input.GetKey(isPlayer1 ? KeyCode.A : KeyCode.LeftArrow)) // movement
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(isPlayer1 ? KeyCode.D : KeyCode.RightArrow)) {
            moveHorizontal = 1;
        }
        if (Input.GetKeyDown(isPlayer1 ? KeyCode.W : KeyCode.UpArrow) && canJump) { // jump
            rb2d.velocity = new Vector2(0, 0);
            velocity = new Vector2(velocity.x, jumpSpeed);
            hasJumped = true;
        }

        if (moveHorizontal != 0 && !hasJumped) { // animations
            GetComponent<SpriteAnim>().PlayAnimation(1);
        } else {
            GetComponent<SpriteAnim>().PlayAnimation(0);
        }
        if (moveHorizontal < 0) // direction animation
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (moveHorizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetKeyDown(isPlayer1 ? KeyCode.Q : KeyCode.Period)) // attacks
        {
            GetComponent<SpriteAnim>().PlayTemp(2, 1);
            DealDamage(50);
        } else if (Input.GetKeyDown(isPlayer1 ? KeyCode.E : KeyCode.Slash)) {
            GetComponent<SpriteAnim>().PlayTemp(4, 6);
            isFrozen = true;
            Invoke("SpecialDamage", 1f/2f);
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

        velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y + velocity.y); // movement
        if (Mathf.Abs(rb2d.velocity.x) > Mathf.Abs(velocity.x) && knockedBack) velocity.x = rb2d.velocity.x;
        rb2d.velocity = velocity;
        velocity.y = 0; // don't change

        if (transform.position.y <= 0 || transform.position.x < -1 || transform.position.x > 30) { // offstage damage
            TakeDamge(maxHP, GetComponent<SpriteRenderer>().flipX ? "left" : "right");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasJumped = false;
        knockedBack = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        knockedBack = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hasJumped = false;
    }

    public void TakeDamge(float damage, string dmgDir)
    {
        HP -= damage;
        isFrozen = true;
        Invoke("Unfreeze", 0.25f);
        Vector2 force = new Vector2(0, Mathf.Sqrt(damage * (maxHP - HP) / 100));

        if (dmgDir == "left") {
            force += new Vector2(-(damage * (maxHP - HP) / 100), 0);
        } else {
            force += new Vector2((damage * (maxHP - HP) / 100), 0);
        }

        if (HP <= 0)
        {
            gameObject.SetActive(false);
            Invoke("Respawn", 3);
            HP = maxHP;
        }

        KnockBack(force);
    }

    private void SpecialDamage()
    {
        GetComponent<SpriteAnim>().PlayTemp(3, 1);
        DealDamage(200);
        isFrozen = false;
    }

    public void DealDamage(float damage)
    {
        bool direction = GetComponent<SpriteRenderer>().flipX; // true == left, false == right

        RaycastHit2D attack = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, -0.25f) + (direction ? Vector2.left * 1f / 2f : Vector2.right), direction ? Vector2.left : Vector2.right, 1f / 2f);

        if (attack.collider != null) {
            if (attack.collider.GetComponent<Player>() != null)
                attack.collider.GetComponent<Player>().TakeDamge(damage, direction ? "left" : "right");
            if (attack.collider.GetComponent<Breakable>() != null)
            {
                attack.collider.GetComponent<Breakable>().Break();
            }
        }
    }

    public void KnockBack(Vector2 strength)
    {
        rb2d.AddForce(new Vector2(strength.x, 0));
        rb2d.velocity = new Vector2(rb2d.velocity.x, strength.y);
        knockedBack = true;
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        canJump = true;
        hasJumped = false;
        velocity = new Vector2(0, 0);
        transform.position = isPlayer1 ? new Vector2(2, 10) : new Vector2(25, 10);
        isFrozen = false;
    }

    private void Unfreeze()
    {
        isFrozen = false;
    }
    public bool CanDoubleJump()
    {
        return !hasJumped;
    }
    public void DoubleJump()
    {
        hasJumped = true;
    }
}
