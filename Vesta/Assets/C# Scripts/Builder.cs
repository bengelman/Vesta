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
    public SpriteRenderer render;
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
        render.sprite = structures[selected].node.GetComponent<SpriteRenderer>().sprite;
        if (Input.GetKeyDown(player.isPlayer1 ? KeyCode.LeftShift : KeyCode.RightShift))
        {
            if (!resourceManager.AreTilesOccupied(structures[selected].BuildPattern(GetComponent<SpriteRenderer>().flipX, Mathf.RoundToInt(player.transform.position.x), (int)Mathf.RoundToInt(player.transform.position.y))))
            {
                resourceManager.SetTiles(structures[selected].BuildPattern(GetComponent<SpriteRenderer>().flipX, (int)Mathf.RoundToInt(player.transform.position.x), (int)Mathf.RoundToInt(player.transform.position.y)), true);
                structures[selected].Build(playerObj, GetComponent<SpriteRenderer>().flipX, (int)Mathf.RoundToInt(player.transform.position.x), (int)Mathf.RoundToInt
                    (player.transform.position.y));
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
            int placeX = facing ? (int)Mathf.CeilToInt(playerX - 0.5f) : Mathf.CeilToInt(playerX + 1);
            return new Vector2[] {new Vector2(placeX, playerY)};
        }
        public override void Build(GameObject player, bool facing, int playerX, int playerY)
        {
            int placeX = facing ? playerX - 1 : playerX + 1;
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
            return new Vector2[] { new Vector2(playerX, (int)playerY - 1.5f) };
        }
        public override void Build(GameObject player, bool facing, int playerX, int playerY)
        {
            if (!player.GetComponent<Player>().CanDoubleJump()) return;
            player.GetComponent<Player>().DoubleJump();
            player.GetComponent<Player>().KnockBack(new Vector2(0, 7));
            Instantiate(node, new Vector2(playerX, (int)playerY - 1.5f), new Quaternion());
            
        }
    }
}
