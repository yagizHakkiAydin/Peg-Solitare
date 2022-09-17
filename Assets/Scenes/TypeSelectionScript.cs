using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypeSelectionScript : MonoBehaviour
{
    public Button typeOneButton;
    public Button typeTwoButton;
    public Button typeThreeButton;
    public Button typeFourButton;
    public Button typeFiveButton;
    public Button typeSixButton;
    public Button exitButton;

    public AudioSource buttonClicked;



    public static int type;





    void Start()
    {

        typeOneButton.onClick.AddListener(setTypeToOne);
        typeTwoButton.onClick.AddListener(setTypeToTwo);
        typeThreeButton.onClick.AddListener(setTypeToThree);
        typeFourButton.onClick.AddListener(setTypeToFour);
        typeFiveButton.onClick.AddListener(setTypeToFive);
        typeSixButton.onClick.AddListener(setTypeToSix);
        exitButton.onClick.AddListener(ExitGame);


       

    }


    void setTypeToOne(  )
    {
        type = 1;
        buttonClicked.Play();
        SceneManager.LoadScene("GameMode");
    }

    void setTypeToTwo()
    {
        type = 2;
        buttonClicked.Play();
        SceneManager.LoadScene("GameMode");
    }

    void setTypeToThree()
    {
        type = 3;
        buttonClicked.Play();
        SceneManager.LoadScene("GameMode");
    }

    void setTypeToFour()
    {
        type = 4;
        buttonClicked.Play();
        SceneManager.LoadScene("GameMode");
    }

    void setTypeToFive()
    {
        type = 5;
        buttonClicked.Play();
        SceneManager.LoadScene("GameMode");
    }

    void setTypeToSix()
    {
        type = 6;
        buttonClicked.Play();
        SceneManager.LoadScene("GameMode");
    }

    void ExitGame()
    {
        buttonClicked.Play();
        Application.Quit();
    }



}
