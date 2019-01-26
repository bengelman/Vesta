using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject playerObj;
    private Player player;
    public ResourceManager resourceManager;
    public GameObject block, anvil;
    private Structure[] structures;
    public int selected;
    // Start is called before the first frame update
    void Start()
    {
        structures = new Structure[]
            {
                new Block(block), new Anvil(anvil)
            };
        player = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(player.isPlayer1 ? KeyCode.LeftAlt : KeyCode.RightAlt))
        {
            selected++;
        }
        selected %= structures.Length;
        if(Input.GetKeyDown(player.isPlayer1 ? KeyCode.LeftShift : KeyCode.RightShift))
        {
            if (!resourceManager.AreTilesOccupied(structures[selected].BuildPattern(GetComponent<SpriteRenderer>().flipX, (int)player.transform.position.x, (int)player.transform.position.y)))
            {
                resourceManager.SetTiles(structures[selected].BuildPattern(GetComponent<SpriteRenderer>().flipX, (int)player.transform.position.x, (int)player.transform.position.y), true);
                structures[selected].Build(playerObj, GetComponent<SpriteRenderer>().flipX, (int)player.transform.position.x, (int)player.transform.position.y);
            }
        }
    }
    private abstract class Structure
    {
        public Structure(GameObject node)
        {
            this.node = node;
        }
        public GameObject node;
        public abstract Vector2[] BuildPattern(bool facing, int playerX, int playerY);
        public abstract void Build(GameObject player, bool facing, int playerX, int playerY);
    }
    private class Block : Structure
    {
        public Block(GameObject node) : base(node)
        {

        }

        public override Vector2[] BuildPattern(bool facing, int playerX, int playerY)
        {
            int placeX = facing ? (int)(playerX - 0.9) : Mathf.CeilToInt(playerX + 1);
            return new Vector2[] {new Vector2(placeX, playerY)};
        }
        public override void Build(GameObject player, bool facing, int playerX, int playerY)
        {
            int placeX = facing ? (int)(playerX - 0.9) : Mathf.CeilToInt(playerX + 1);
            Instantiate(node, new Vector2(placeX, playerY), new Quaternion());
        }
    }
    private class Anvil : Structure
    {
        public Anvil(GameObject node) : base(node)
        {

        }
        public override Vector2[] BuildPattern(bool facing, int playerX, int playerY)
        {
            return new Vector2[] { new Vector2(playerX, playerY - 1) };
        }
        public override void Build(GameObject player, bool facing, int playerX, int playerY)
        {
            Instantiate(node, new Vector2(playerX, playerY - 1), new Quaternion());
            
        }
    }
}
