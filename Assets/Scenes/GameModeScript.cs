using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameModeScript : MonoBehaviour
{


    public Button playerModeButton;
    public Button computerModeButton;


    public static string mode;


    void Start()
    {
        playerModeButton.onClick.AddListener(selectPlayerMode);
        computerModeButton.onClick.AddListener(selectComputerMode);
    }



    void selectPlayerMode()
    {
        mode = "PM";
        SceneManager.LoadScene("GameScene");
    }

    void selectComputerMode()
    {
        mode = "CM";
        SceneManager.LoadScene("GameScene");
    }


}
