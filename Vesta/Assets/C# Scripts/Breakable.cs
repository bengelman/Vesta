using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            manager.SetTiles(new Vector2[] { transform.position }, false);

            if (p1flag)
            {
                gameOver.text = "Player 2 Wins!";
                Invoke("Restart", 2);
            }
            else if (p2flag)
            {
                gameOver.text = "Player 1 Wins!";
                Invoke("Restart", 2);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    public void Restart()
    {
        Debug.Log("Loading menu");
        SceneManager.LoadScene("MainMenu");
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
