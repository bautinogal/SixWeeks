using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to test first person mechanics
public class FirstPersonLevel : MonoBehaviour
{
    //Text that is displayed in the initial message box
    [SerializeField]
    string initialText;

    [SerializeField]
    GameObject[] enemys;

    //Initial State of the scene
    void Start()
    {
        Application.targetFrameRate = 300;
        //DialogCanvas.ShowDialog(initialText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenu.PauseGame();
        /*
        //Camera moves away from the player (bird view) and activates character controls
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && GameCamera.GetState() == GameCamera.CamState.CLOSEUP)
        {
            DialogCanvas.HideDialog();
            GameCamera.GoToStandard(() => {
                CamGUI.ShowLifes();
            });
        }

        //Camera moves in front of the player and disables character controls
        if (Input.GetKeyDown(KeyCode.Space) && GameCamera.GetState() == GameCamera.CamState.STANDARD)
        {

            GameCamera.GoToCloseUp(() => {
                DialogCanvas.ShowDialog(initialText);
                CamGUI.HideLifes();
            });
        }*/

        foreach (var enemy in enemys)
        {
            if (enemy.activeInHierarchy)
                return;
        }
        Cursor.visible = false;
        EndGameMenu.Victory();
    }
}