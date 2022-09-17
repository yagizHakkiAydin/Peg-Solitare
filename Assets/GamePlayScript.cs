using System.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GamePlayScript : MonoBehaviour
{

    public GameObject[] groundObjectsArray;

    public GameObject[] ballObjectsArray;



    public AudioSource moveSound;

    public AudioSource buttonClickSound;




    public Transform mainCameraTransform;



    public Material defaultBallMaterial;

    public Material clickedBallMaterial;


    private string gameMode;

    private int boardType;



    public Button pauseMenuButton;

    public Button resumeButton;

    public Button exitButton;

    public Button saveButton;

    public TextMeshProUGUI gameOverMessage;



    private int lastSelectedBallIndex;





    





    void Start()
    {
        gameMode = GameModeScript.mode;

        saveButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        gameOverMessage.enabled = false;



        pauseMenuButton.onClick.AddListener(EnablePauseMenu);
        resumeButton.onClick.AddListener(resumeGame);
        saveButton.onClick.AddListener(SaveGame);
        exitButton.onClick.AddListener(Application.Quit);




        boardType = TypeSelectionScript.type;


        lastSelectedBallIndex = 81;
        setCamera();
        InitializeBoard();



    }





    //This function allows user to play game,it is called until game has ended
    void Update()
    {

         if( gameMode == "PM")
         {
            PlayPlayer();
         }
         else
         {
            playAuto();
         }
         
         if(hasGameEnded() == true)
         {
              StartCoroutine(ExitGame()); 
         }

    }



    //This function sets selected ball and selected ground piece
    void PlayPlayer()
    {
        for (int ballIndex = 0; ballIndex < 81; ballIndex++)
        {
            play();

            if (ballObjectsArray[ballIndex] != null)
            {

                if ((ballObjectsArray[ballIndex].tag == "Selected") && (ballIndex != lastSelectedBallIndex))
                {
                    ballObjectsArray[ballIndex].GetComponent<MeshRenderer>().material = clickedBallMaterial;
                    lastSelectedBallIndex = ballIndex;
                    ClearOtherSelectedBalls(ballIndex);
                }
            }

            if (groundObjectsArray[ballIndex] != null)
            {
                if (IsGroundPieceEmpty(ballIndex))
                {
                    groundObjectsArray[ballIndex].tag = "Empty";
                }
                else
                {
                    groundObjectsArray[ballIndex].tag = "notEmpty";
                }
            }
        }
    }


    //This function disables pause menu
    void resumeGame()
    {
        saveButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }



    //This function is used to exit when game is ended
    //Prints "Game Over" on screen
    //Waits for 4 seconds
    //Terminates application
    IEnumerator ExitGame()
    {
        gameOverMessage.enabled = true;
        yield return new WaitForSecondsRealtime(4);
        Application.Quit();
    }



    //This function enables pause menu
    void EnablePauseMenu()
    {
        if (resumeButton.IsActive() && exitButton.IsActive())
        {
            saveButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
        }
        else
        {
            saveButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
    }



    //This function allows player to make one move
    private void play()
    {
        
        int selectedBallIndex = GetSelectedBallIndex();

        int selectedGroundPieceIndex = GetSelectedGroundPieceIndex();




        if ((selectedBallIndex != 81) && (selectedGroundPieceIndex != 81))
        {
            int selectedBallXPosition = (int)ballObjectsArray[selectedBallIndex].transform.position.x;  //Vertical position of selected ball
            int selectedBallZPosition = (int)ballObjectsArray[selectedBallIndex].transform.position.z;  //Horizontal position of selected ball
            int selectedGroundPieceXPosition = (int)groundObjectsArray[selectedGroundPieceIndex].transform.position.x;  //Vertical position of selected ground piece
            int selectedGroundPieceZPosition = (int)groundObjectsArray[selectedGroundPieceIndex].transform.position.z;  //Horizontal position of selected ground piece



            if ( boardType != 6) 
            {
                if( (selectedBallZPosition==selectedGroundPieceZPosition) && (selectedBallXPosition==selectedGroundPieceXPosition-4) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex-9)) )//to down
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex - 9);
                }
                else if ((selectedBallZPosition == selectedGroundPieceZPosition) && (selectedBallXPosition == selectedGroundPieceXPosition + 4) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex + 9)))  //to up
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex + 9);
                }
                else if ((selectedBallZPosition == selectedGroundPieceZPosition-4) && (selectedBallXPosition == selectedGroundPieceXPosition) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex -1)))  //to left
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex - 1);
                }
                else if ((selectedBallZPosition == selectedGroundPieceZPosition + 4) && (selectedBallXPosition == selectedGroundPieceXPosition) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex + 1)))  //to right
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex + 1);
                }
            }
            else
            {
                if ((selectedBallZPosition == selectedGroundPieceZPosition + 4) && (selectedBallXPosition == selectedGroundPieceXPosition + 4) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex + 10)))//to upperLeft
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex + 10);
                }
                else if ((selectedBallZPosition == selectedGroundPieceZPosition - 4) && (selectedBallXPosition == selectedGroundPieceXPosition + 4)  && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex + 8)))  //to upperRight
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex +8 );
                }
                else if ((selectedBallZPosition == selectedGroundPieceZPosition + 4) && (selectedBallXPosition == selectedGroundPieceXPosition - 4) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex - 8)))//to upperLeft
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex - 8);
                }
                else if ((selectedBallZPosition == selectedGroundPieceZPosition - 4) && (selectedBallXPosition == selectedGroundPieceXPosition - 4) && IsGroundPieceEmpty(selectedGroundPieceIndex) && !(IsGroundPieceEmpty(selectedGroundPieceIndex - 10)))  //to DownLeft
                {
                    ballObjectsArray[selectedBallIndex].transform.position = groundObjectsArray[selectedGroundPieceIndex].transform.position;
                    ballObjectsArray[selectedBallIndex].transform.position += new Vector3(0, 1, 0);
                    moveSound.Play();
                    DestroyBallAtGivenGroundPiecePosition(selectedGroundPieceIndex - 10);
                }
            }
        }




    }


    //This function checks if there is a ball on asked ground piece
    private bool IsGroundPieceEmpty( int groundPieceIndex )
    {

         if( GetBallIndexOnGroundPiece(groundPieceIndex) == -1)
         {
             return true; 
         }
        
         return false;
    }



    //This function returns index value of the clicked ball
    private int GetSelectedBallIndex()
    {
        int result = 81;

        for (int index = 0; index < 81; index++)
        {
            if (ballObjectsArray[index] != null)
            {
                if ((ballObjectsArray[index].tag == "Selected"))
                {
                    result = index;
                    break;
                }
            }
        }

        return result;
    }




    //This function returns index value of the clicked ground piece
    private int GetSelectedGroundPieceIndex()
    {
        int result = 81;

        for (int index = 0; index < 81; index++)
        {
            if (groundObjectsArray[index] != null)
            {
                if ((groundObjectsArray[index].tag == "Selected"))
                {
                    result = index;
                    break;
                }
            }
        }

        return result;
    }



    //This function is for avoiding selecting two balls
    private void ClearOtherSelectedBalls(int ballIndex)
    {
        for (int index = 0; index < 81; index++)
        {
            if (ballObjectsArray[index] != null)
            {
                if (index != ballIndex)
                {
                    ballObjectsArray[index].tag = "notSelected";
                    ballObjectsArray[index].GetComponent<MeshRenderer>().material = defaultBallMaterial;
                }
            }
        }

    }




    //This function takes index for array of ground objects as parameter
    //Returns the index value of the ball object which is on the asked ground piece
    //if ground piece is empty,returns -1

    //This function is needed,because balls move on ground objects and ground object and ball on it can have different indexes
    //so,ball on asked ground piece must be detected by position values
    private int GetBallIndexOnGroundPiece(int indexOfGroundPiece)
    {


        for (int ballIndex = 0; ballIndex < 81; ballIndex++)
        {
            if (ballObjectsArray[ballIndex] != null)
            {
                if (ballObjectsArray[ballIndex].transform.position == groundObjectsArray[indexOfGroundPiece].transform.position + new Vector3(0, 1, 0))
                {
                    return ballIndex;
                }
            }
            
        }

        return -1;
    }






    //This function initializes the game board
    //At the beginning,there is 9x9 square board and 81 balls on ground pieces
    //This function shapes board by destroying necessary objects
    private void InitializeBoard()
    {
        System.Exception InvalidTypeException = new System.Exception();

        if ((boardType < 1) || (boardType > 6))
        {
            throw InvalidTypeException;
        }


        List<int> boardIndexes = getBoardIndexes(boardType);

        for (int i = 0; i < boardIndexes.Count; i++)
        {
            Destroy(groundObjectsArray[boardIndexes[i]]);
            Destroy(ballObjectsArray[boardIndexes[i]]);
        }


        if (boardType == 1)
        {
            Destroy(ballObjectsArray[21]);
        }
        else if (boardType == 2)
        {
            Destroy(ballObjectsArray[40]);
        }
        else if (boardType == 3)
        {
            Destroy(ballObjectsArray[39]);
        }
        else if (boardType == 4)
        {
            Destroy(ballObjectsArray[30]);
        }
        else if (boardType == 5)
        {
            Destroy(ballObjectsArray[40]);
        }
        else
        {
            Destroy(ballObjectsArray[4]);
        }

    }



    //This function sets camera position after initialization of the board
    private void setCamera()
    {
        if (boardType == 1)
        {
            mainCameraTransform.position += new Vector3(-2.0f, 0.0f, -2.0f);
        }
        else if (boardType == 2)
        {
            mainCameraTransform.position += new Vector3(0, 0, 0);
        }
        else if (boardType == 3)
        {
            mainCameraTransform.position += new Vector3(0, 0, 0);
        }
        else if (boardType == 4)
        {
            mainCameraTransform.position += new Vector3( -2.0f , 0.0f , -2.0f );
        }
        else if (boardType == 5)
        {
            mainCameraTransform.position += new Vector3(0, 0, 0);
        }
        else
        {
            mainCameraTransform.position += new Vector3(0, 0, 0);
        }
    }





    //This function returns indexes of the ground objects to be destroyed.
    //These indexes is used to shape 9x9 square board
    private List<int> getBoardIndexes(int boardType)
    {

        List<int> boardIndexes;


        if (boardType == 1)
        {
            boardIndexes = new List<int> { 0, 1, 5, 6, 7, 8, 9, 15, 16, 17, 25, 26, 34, 35, 43, 44, 45, 51, 52, 53, 54, 55, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
        }
        else if (boardType == 2)
        {
            boardIndexes = new List<int> { 0, 1, 2, 6, 7, 8, 9, 10, 11, 15, 16, 17, 18, 19, 20, 24, 25, 26, 54, 55, 56, 60, 61, 62, 63, 64, 65, 69, 70, 71, 72, 73, 74, 78, 79, 80 };
        }
        else if (boardType == 3)
        {
            boardIndexes = new List<int> { 0, 1, 5, 6, 7, 8, 9, 10, 14, 15, 16, 17, 18, 19, 23, 24, 25, 26, 63, 64, 68, 69, 70, 71, 72, 73, 77, 78, 79, 80 };
        }
        else if (boardType == 4)
        {
            boardIndexes = new List<int> { 0, 1, 5, 6, 7, 8, 9, 10, 14, 15, 16, 17, 25, 26, 34, 35, 43, 44, 45, 46, 50, 51, 52, 53, 54, 55, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
        }
        else if (boardType == 5)
        {
            boardIndexes = new List<int> { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 15, 16, 17, 18, 19, 25, 26, 27, 35, 45, 53, 54, 55, 61, 62, 63, 64, 65, 69, 70, 71, 72, 73, 74, 75, 77, 78, 79, 80 };
        }
        else
        {
            boardIndexes = new List<int> { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 13, 15, 16, 17, 18, 19, 21, 23, 25, 26, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
        }

        return boardIndexes;

    }




    //This function gets index for ground objects array and destroys the ball on that ground object
    private void DestroyBallAtGivenGroundPiecePosition( int groundPieceIndex )
    {
        for (int index = 0; index < 81; index++)
        {
            if (ballObjectsArray[index] != null)
            {
                if( (ballObjectsArray[index].transform.position.x == groundObjectsArray[groundPieceIndex].transform.position.x)  &&  (ballObjectsArray[index].transform.position.z == groundObjectsArray[groundPieceIndex].transform.position.z) )
                {
                    Destroy(ballObjectsArray[index]);
                    break;
                }
            }
        }
        
    }


    //This function checks if game has ended
    public bool hasGameEnded()
    {
        if( boardType == 6 )
        {
            //This loop checks if there is availible  moves on the line which is slope = 1
            for (int index = 0; index < 61; index++)
            {
                if ((groundObjectsArray[index] != null) && (groundObjectsArray[index + 10] != null) && (groundObjectsArray[index + 20] != null))//alt
                {
                    if (IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 10) && !IsGroundPieceEmpty(index + 20))
                    {
                        return false;
                    }
                    if (!IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 10) && IsGroundPieceEmpty(index + 20))
                    {
                        return false;
                    }
                }

            }


            //This loop checks if there is availible  moves on the line which is slope = -1
            for (int index = 0; index < 65; index++)
            {
                if ((groundObjectsArray[index] != null) && (groundObjectsArray[index + 8] != null) && (groundObjectsArray[index + 16] != null))//alt
                {
                    if (IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 8) && !IsGroundPieceEmpty(index + 16))
                    {
                        return false;
                    }
                    if (!IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 8) && IsGroundPieceEmpty(index + 16))
                    {
                        return false;
                    }
                }

            }
        }
        else
        {
            //This loop checks if there is availible vertical moves
            for (int index = 0; index < 63; index++)
            {
                if ((groundObjectsArray[index] != null) && (groundObjectsArray[index + 9] != null) && (groundObjectsArray[index + 18] != null))//alt
                {
                    if (IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 9) && !IsGroundPieceEmpty(index + 18))
                    {
                        return false;
                    }
                    if (!IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 9) && IsGroundPieceEmpty(index + 18))
                    {
                        return false;
                    }
                }

            }


            //This loop checks if there is availible horizontal moves
            for (int index = 0; index < 79; index++)
            {
                if ((groundObjectsArray[index] != null) && (groundObjectsArray[index + 1] != null) && (groundObjectsArray[index + 2] != null))//alt
                {
                    if (IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 1) && !IsGroundPieceEmpty(index + 2))
                    {
                        return false;
                    }
                    if (!IsGroundPieceEmpty(index) && !IsGroundPieceEmpty(index + 1) && IsGroundPieceEmpty(index + 2))
                    {
                        return false;
                    }
                }

            }
        }




        return true;
    }




    //This function makes one automatic move when it's called
    private void playAuto()
    {

        //Move is randomly decided
        System.Random randomObject = new System.Random();



        if (boardType != 6)
        {


            int[][] directionIndexes = new int[4][];  //This array keeps four arrays and these four arrays has values to be added to corresponding ground piece's index to decide direction of move


            int[] upArr = { -9, -18 };
            int[] downArr = { +9, +18 };
            int[] leftArr = { -1, -2 };
            int[] rightArr = { 1, 2 };

            directionIndexes[0] = upArr;
            directionIndexes[1] = downArr;
            directionIndexes[2] = leftArr;
            directionIndexes[3] = rightArr;




            
            int index = randomObject.Next(0, 80);
            int randomDirection = randomObject.Next(0, 4);  //Index of ground object
            int secondPieceIndex = index + directionIndexes[randomDirection][0];  //Index of ground object at middle,ball of this object is destroyed
            int thirdPieceIndex = index + directionIndexes[randomDirection][1];  //Index of ground object at the end




            if ((index >= 0) && (index < 81) && (secondPieceIndex >= 0) && (secondPieceIndex < 81) && (thirdPieceIndex >= 0) && (thirdPieceIndex < 81))
                if (groundObjectsArray[index] != null && groundObjectsArray[secondPieceIndex] != null && groundObjectsArray[thirdPieceIndex] != null)
                    if (IsGroundPieceEmpty(thirdPieceIndex) && !IsGroundPieceEmpty(secondPieceIndex) && !IsGroundPieceEmpty(index))
                    {
                        ballObjectsArray[GetBallIndexOnGroundPiece(index)].transform.position = groundObjectsArray[thirdPieceIndex].transform.position + new Vector3(0, 1, 0);
                        DestroyBallAtGivenGroundPiecePosition(secondPieceIndex);
                        System.Threading.Thread.Sleep(1000);
                        moveSound.Play();
                    }
        }
        else
        {

            int[][] directionIndexes = new int[4][];
            int[] arr1 = { -10, -20 };
            int[] arr2 = { +10, +20 };
            int[] arr3 = { -8, -16 };
            int[] arr4 = { 8, 16 };

            directionIndexes[0] = arr1;
            directionIndexes[1] = arr2;
            directionIndexes[2] = arr3;
            directionIndexes[3] = arr4;





            int index = randomObject.Next(0, 80);
            int randomDirection = randomObject.Next(0, 4);
            int secondPieceIndex = index + directionIndexes[randomDirection][0];//ortadaki
            int thirdPieceIndex = index + directionIndexes[randomDirection][1];//sondaki boþ kutu




            if ((index >= 0) && (index < 81) && (secondPieceIndex >= 0) && (secondPieceIndex < 81) && (thirdPieceIndex >= 0) && (thirdPieceIndex < 81))
                if (groundObjectsArray[index] != null && groundObjectsArray[secondPieceIndex] != null && groundObjectsArray[thirdPieceIndex] != null)
                    if (IsGroundPieceEmpty(thirdPieceIndex) && !IsGroundPieceEmpty(secondPieceIndex) && !IsGroundPieceEmpty(index))
                    {
                        ballObjectsArray[GetBallIndexOnGroundPiece(index)].transform.position = groundObjectsArray[thirdPieceIndex].transform.position + new Vector3(0, 1, 0);
                        DestroyBallAtGivenGroundPiecePosition(secondPieceIndex);
                        System.Threading.Thread.Sleep(1000);
                        moveSound.Play();

                    }

        
        }
    }

    //This function creates "save.txt" in game folder and saves the game
    void SaveGame()
    {
        
        string type = boardType.ToString();


        string mode = gameMode;

        string boardStr = "";


        for( int index=0;index<81;index++)
        {
            if( groundObjectsArray[index] == null)
            {
                boardStr += "U";//undefined
            }
            else if( IsGroundPieceEmpty(index) == true)
            {
                boardStr += "E";//empty
            }
            else
            {
                boardStr += "P";//pegged
            }

            
        }

        using  (var streamWriterObj = new StreamWriter("save.txt") )
        {

            streamWriterObj.WriteLine(boardType);
            streamWriterObj.WriteLine(gameMode);
            streamWriterObj.WriteLine(boardStr);

        }


        
        


    }







}
