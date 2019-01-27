using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private Vector2 velocity = new Vector2(0, 0);
    public float jumpSpeed = 5;
    public float speed = 10;
    public float maxHP = 1000;
    public float HP;
    public int resources;
    private bool canJump;
    private bool hasJumped;
    public bool isPlayer1;
    public bool isFrozen;
    public bool knockedBack;
    public bool canFall;
    public bool canDash;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = true;
        hasJumped = false;
        HP = maxHP;
        isFrozen = false;
        knockedBack = false;
        canFall = true;
    }

    // Update is called once per frame
    void Update()
    {
        slowmotime -= Time.deltaTime;
        if (slowmotime <= 0)
        {
            Time.timeScale = 1.0f;
        }
        if (!canFall)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }

        if (isFrozen) return;

        float moveHorizontal = 0;
        if (hasJumped) {
            canJump = false;
        } else {
            canJump = true;
        }
        
        if (Input.GetKey(isPlayer1 ? KeyCode.A : KeyCode.Keypad4)) // movement
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(isPlayer1 ? KeyCode.D : KeyCode.Keypad6)) {
            moveHorizontal = 1;
        }
        if (Input.GetKeyDown(isPlayer1 ? KeyCode.W : KeyCode.Keypad8) && canJump) { // jump
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

        if (Input.GetKeyDown(isPlayer1 ? KeyCode.Q : KeyCode.Keypad7)) // attacks
        {
            GetComponent<SpriteAnim>().PlayTemp(2, 1);
            DealDamage(50);
        } else if (Input.GetKeyDown(isPlayer1 ? KeyCode.E : KeyCode.Keypad9)) {
            if ((Input.GetKey(isPlayer1 ? KeyCode.A : KeyCode.Keypad4) || Input.GetKey(isPlayer1 ? KeyCode.D : KeyCode.Keypad6)) && canDash) {
                GetComponent<SpriteAnim>().PlayTemp(4, 6);
                rb2d.AddForce(new Vector2(200 * (GetComponent<SpriteRenderer>().flipX ? -1 : 1), 0));
                canFall = false;
                isFrozen = true;
                canDash = false;
                Invoke("SideSpecialDamage", 0.5f);
            } else {
                GetComponent<SpriteAnim>().PlayTemp(4, 6);
                isFrozen = true;
                Invoke("SpecialDamage", 0.5f);
            }
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

        if (transform.position.y <= 0.05 || transform.position.x < -1 || transform.position.x > 44 || transform.position.y > 25) { // offstage damage
            gameObject.SetActive(false);
            Invoke("Respawn", 3);
            HP = maxHP;
            
            //TakeDamge(maxHP, GetComponent<SpriteRenderer>().flipX ? "left" : "right");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasJumped = false;
        knockedBack = false;
        canDash = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        knockedBack = false;
        canDash = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hasJumped = false;
        canDash = true;
    }

    public void TakeDamge(float damage, string dmgDir, bool doesKnockback = true)
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

        if (HP <= 0 && damage >= 100)
        {
            Time.timeScale = 0.1f;
            slowmotime = 1.5f;
            //isFrozen = true;
            //gameObject.SetActive(false);
            //Invoke("Respawn", 3);
            //HP = maxHP;
        }

        if (doesKnockback) {
            KnockBack(force);
        }
    }
    float slowmotime = 0;
    private void SpecialDamage()
    {
        GetComponent<SpriteAnim>().PlayTemp(3, 1);
        DealDamage(200);
        isFrozen = false;
    }

    private void SideSpecialDamage()
    {
        GetComponent<SpriteAnim>().PlayTemp(3, 1);
        DealDamage(100);
        isFrozen = false;
        canFall = true;
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
        rb2d.velocity = new Vector2(0, 0.115f);
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
        HP = maxHP;
        transform.position = isPlayer1 ? new Vector2(2, 10) : new Vector2(42, 10);
        isFrozen = false;
    }

    private void Unfreeze()
    {
        isFrozen = false;
    }
    public bool CanDoubleJump()
    {
        return canJump;
    }
    public void DoubleJump()
    {
        hasJumped = true;
    }

}
