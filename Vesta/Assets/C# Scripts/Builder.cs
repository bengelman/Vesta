using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject playerObj;
    private Player player;
    public ResourceManager resourceManager;
    public GameObject block, anvil, bridge;
    private Structure[] structures;
    public SpriteRenderer render;
    public TextMesh text;
    public int selected;
    // Start is called before the first frame update
    void Start()
    {
        structures = new Structure[]
            {
                new Block(block), new Anvil(anvil), new Bridge(bridge)
            };
        player = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < structures.Length; i++)
            structures[i].available += Time.deltaTime;
        text.text = "" + (int)(structures[selected].available / structures[selected].cost);
        if (Input.GetKeyDown(player.isPlayer1 ? KeyCode.S : KeyCode.Keypad5))
        {
            selected++;
            
        }
        selected %= structures.Length;
        render.sprite = structures[selected].node.GetComponent<SpriteRenderer>().sprite;

        if (Input.GetKeyDown(player.isPlayer1 ? KeyCode.LeftShift : KeyCode.RightShift) && structures[selected].available >= structures[selected].cost)
        {
            if (!resourceManager.AreTilesOccupied(structures[selected].BuildPattern(GetComponent<SpriteRenderer>().flipX, Mathf.RoundToInt(player.transform.position.x), (int)Mathf.RoundToInt(player.transform.position.y))))
            {
                
                resourceManager.SetTiles(structures[selected].BuildPattern(GetComponent<SpriteRenderer>().flipX, (int)Mathf.RoundToInt(player.transform.position.x), (int)Mathf.RoundToInt(player.transform.position.y)), true);
                if(structures[selected].Build(playerObj, GetComponent<SpriteRenderer>().flipX, (int)Mathf.RoundToInt(player.transform.position.x), (int)Mathf.RoundToInt(player.transform.position.y))) structures[selected].available -= structures[selected].cost;
            }
        }
    }
    private abstract class Structure
    {
        public int cost;
        public float available;
        public Structure(GameObject node, int cost, float available)
        {
            this.cost = cost;
            this.node = node;
            this.available = available;
        }
        public GameObject node;
        public abstract Vector2[] BuildPattern(bool facing, int playerX, int playerY);
        public abstract bool Build(GameObject player, bool facing, int playerX, int playerY);
    }
    private class Block : Structure
    {
        public Block(GameObject node) : base(node, 2, 20)
        {

        }

        public override Vector2[] BuildPattern(bool facing, int playerX, int playerY)
        {
            int placeX = facing ? playerX - 1 : playerX + 1;
            return new Vector2[] {new Vector2(placeX, playerY)};
        }
        public override bool Build(GameObject player, bool facing, int playerX, int playerY)
        {
            int placeX = facing ? playerX - 1 : playerX + 1;
            Instantiate(node, new Vector2(placeX, playerY), new Quaternion());
            return true;
        }
    }
    private class Anvil : Structure
    {
        public Anvil(GameObject node) : base(node, 10, 20)
        {

        }
        public override Vector2[] BuildPattern(bool facing, int playerX, int playerY)
        {
            return new Vector2[] { new Vector2(playerX, (int)playerY - 1.5f) };
        }
        public override bool Build(GameObject player, bool facing, int playerX, int playerY)
        {
            if (!player.GetComponent<Player>().CanDoubleJump()) return false;
            player.GetComponent<Player>().DoubleJump();
            player.GetComponent<Player>().KnockBack(new Vector2(0, 7));
            player.GetComponent<SpriteAnim>().PlayTemp(5, 1);
            Instantiate(node, new Vector2(playerX, (int)playerY - 1.5f), new Quaternion());
            return true;
        }
    }
    private class Bridge : Structure
    {
        public Bridge(GameObject node) : base(node, 5, 20)
        {

        }
        public override Vector2[] BuildPattern(bool facing, int playerX, int playerY)
        {
            return new Vector2[] { new Vector2(playerX, (int)(playerY - 1.5f))};
        }
        public override bool Build(GameObject player, bool facing, int playerX, int playerY)
        {
            player.GetComponent<SpriteAnim>().PlayTemp(5, 1);
            Instantiate(node, new Vector2(playerX, (int)(playerY - 1.5f)), new Quaternion());
            return true;

        }
    }
}
