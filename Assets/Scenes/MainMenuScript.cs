using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public Button PlayButton;

    public Button ExitButton;

    public AudioSource buttonClicked;






    void Start()
    {

        ExitButton.onClick.AddListener( ExitGame );
        PlayButton.onClick.AddListener( startGame );

    }



    private void ExitGame()
    {
        buttonClicked.Play();
        Application.Quit();

    }


    private void startGame()
    {
        buttonClicked.Play();
        SceneManager.LoadScene("BoardTypeSelectionMenu");   
    }



}

