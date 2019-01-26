using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int player1Resources;
    public int player2Resources;
    public int player1Control;
    public int player2Control;
    public  int mapX;
    public  int mapY;
    public bool[,] tiles;
    public Vector2[] occupiedTiles;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new bool[mapX,mapY];
        foreach (Vector2 vec in occupiedTiles)
        {
            tiles[(int)vec.x,(int)vec.y] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool AreTilesOccupied(Vector2[] checkTiles)
    {
        foreach (Vector2 vec in occupiedTiles)
        {
            if (tiles[(int)vec.x,(int)vec.y])
                return false;
        }
        return false;
    }
    public void SetTiles(Vector2[] checkTiles, bool value)
    {
        foreach (Vector2 vec in checkTiles)
        {
            tiles[(int)vec.x,(int)vec.y] = value;
        }
    }
}
