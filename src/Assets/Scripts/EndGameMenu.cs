using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    static EndGameMenu instance;
    AudioSource audioSource;

    [SerializeField]
    string victoryMsj = "";
    [SerializeField]
    string defeatMsj = "";

    [SerializeField]
    Text title;
    [SerializeField]
    Button reset;
    [SerializeField]
    Button mainMenu;
    static bool finished = false;

    public static bool IsFinished()
    {
        return finished;
    }

    //Singleton Pattern:
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            reset.onClick.AddListener(() => ResetLevel());
            mainMenu.onClick.AddListener(() => MainMenu());
            gameObject.SetActive(false);
            finished = false;
            instance.audioSource.volume = 0;
            SceneManager.sceneLoaded += (s, l) => PauseMenu.UnPauseGame();
        }
        else if(instance != this)
            Destroy(this);
    }

    //Command to pause the game
    public static void Victory()
    {
        Cursor.visible = true;
        PauseMenu.UnPauseGame();
        Cursor.visible = true;
        PlayerController.SetActive(false);
        instance.title.text = instance.victoryMsj;
        instance.gameObject.SetActive(true);
        instance.audioSource.volume = 1;
        Time.timeScale = 0;
        finished = true;
    }

    //Command to unpause the game
    public static void Defeat()
    {
        Cursor.visible = true;
        PauseMenu.UnPauseGame();
        PlayerController.SetActive(false);
        instance.title.text = instance.defeatMsj;
        instance.gameObject.SetActive(true);
        instance.audioSource.volume = 0;
        Time.timeScale = 0;
        finished = true;
    }

    //Command to reset the game
    public static void ResetLevel()
    {
        
        Cursor.visible = true;
        Time.timeScale = 1;
        finished = false;
        instance.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PauseMenu.UnPauseGame();
        Cursor.visible = true;
    }

    //Command to go to MainMenu
    public static void MainMenu()
    {
        Time.timeScale = 1;
        finished = false;
        instance.gameObject.SetActive(false);
        SceneManager.LoadScene(0);

    }
}
