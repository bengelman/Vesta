using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public Button button;
    public Image render;
    public Sprite black;
    public bool quit = false;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        if (quit)
        {
            Application.Quit();
        }
        render.sprite = black;
        SceneManager.LoadScene("MainGame");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
