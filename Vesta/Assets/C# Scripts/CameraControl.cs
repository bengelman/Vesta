using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().pixelRect = new Rect(0, 0, (Screen.width / 44f < Screen.height / 19f) ? Screen.width : Screen.height * (44f / 19f), (Screen.width / 44f < Screen.height / 19f) ? Screen.width * (19f / 44f) : Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
