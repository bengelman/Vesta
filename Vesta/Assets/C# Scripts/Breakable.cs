using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int durability = 1;
    public bool p1flag = false;
    public bool p2flag = false;
    public TextMesh gameOver;
    public ResourceManager manager;
    public void Break( )
    {
        durability--;
        if (durability <= 0)
        {
            manager.SetTiles(new Vector2[] {transform.position}, false);
            Destroy(gameObject);
            if (p1flag)
            {
                gameOver.text = "Player 2 Wins!";
            }
            else if (p2flag)
            {
                gameOver.text = "Player 1 Wins!";
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
