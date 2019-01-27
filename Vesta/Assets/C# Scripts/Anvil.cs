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
        Destroy(this.gameObject);
        if (collision.collider.GetComponent<Player>() != null) {
            collision.collider.GetComponent<Player>().TakeDamge(100, !collision.collider.GetComponent<SpriteRenderer>().flipX ? "left" : "right");
            
        } else if (collision.collider.GetComponent<Breakable>() != null) {
            collision.collider.GetComponent<Breakable>().Break();
        }
    }
}
