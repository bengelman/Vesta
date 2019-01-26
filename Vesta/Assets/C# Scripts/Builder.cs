using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject playerObj;
    private Player player;
    public ResourceManager resourceManager;
    public GameObject block;
    private Structure[] structures = new Structure[]
    {
        new Block()
    };
    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(player.isPlayer1 ? KeyCode.LeftShift : KeyCode.RightShift))
        {
            int placeY = (int)player.transform.position.y;
            int placeX = GetComponent<SpriteRenderer>().flipX ? (int)(player.transform.position.x - 0.9) : Mathf.CeilToInt(player.transform.position.x + 1);
            if (!resourceManager.AreTilesOccupied(new Vector2[]{new Vector2(placeX, placeY)}))
            {
                resourceManager.SetTiles(new Vector2[] { new Vector2(placeX, placeY) }, true);
                Instantiate(block, new Vector2(placeX, placeY), new Quaternion());
            }
        }
    }
    private class Structure
    {

    }
    private class Block : Structure
    {

    }
}
