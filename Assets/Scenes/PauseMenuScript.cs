using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public Button backToGameButton;

    public Button exitButton;

    public AudioSource buttonClicked;


    void Update()
    {
        backToGameButton.onClick.AddListener(backToGame);
        exitButton.onClick.AddListener(ExitGame);
    }




    private void ExitGame()
    {
        buttonClicked.Play();
        Application.Quit();
    }


    private void backToGame()
    {
        buttonClicked.Play();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
    }

}
