using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    //Text that is displayed in the initial message box
    [SerializeField]
    string initialText;

    Enemy[] enemys;

    //Initial State of the scene
    void Start()
    {
        Application.targetFrameRate = 300;
        enemys = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenu.PauseGame();

        // Check if there are enemys alive
        foreach (var enemy in enemys)
        {
            if (enemy.gameObject.activeInHierarchy)
                return;
        }

        // If there are no enemys left end game
        Cursor.visible = false;
        EndGameMenu.Victory();
    }
}
