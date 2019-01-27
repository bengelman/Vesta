using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hazard = Physics2D.Raycast((Vector2)transform.position + (Vector2.down * 0.5f), Vector2.down, 0.5f);
        if (hazard.collider.GetComponent<Player>() != null) {
            hazard.collider.GetComponent<Player>().TakeDamge(100, !hazard.collider.GetComponent<Player>().GetComponent<SpriteRenderer>().flipX ? "left" : "right");
            Destroy(this.gameObject);
        } else if (hazard.collider.GetComponent<Breakable>() != null) {
            hazard.collider.GetComponent<Breakable>().Break();
            Destroy(this.gameObject);
        }
    }
}
