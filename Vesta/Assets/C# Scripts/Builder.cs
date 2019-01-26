using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject playerObj;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
