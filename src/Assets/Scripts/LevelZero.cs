using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class with levelzero logic
public class LevelZero : MonoBehaviour
{
    //Text that is displayed in the initial message box
    [SerializeField]
    string initialText;
    //The target that the camera will follow (player)
    [SerializeField]
    Transform target;

    [SerializeField]
    GameObject[] enemys;

    PlayerThirdPersonInput playerThirdPersonInput;

    //Initial State of the scene
    void Start()
    {
        playerThirdPersonInput = FindObjectOfType<PlayerThirdPersonInput>();
        Application.targetFrameRate = 300;
        //DialogCanvas.ShowDialog(initialText);
        GameCamera.SetTarget(target);
        GameCamera.GoToCloseUp(0.1f);
        playerThirdPersonInput.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenu.PauseGame();

        //Camera moves away from the player (bird view) and activates character controls
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && GameCamera.GetState() == GameCamera.CamState.CLOSEUP)
        {
            DialogCanvas.HideDialog();
            GameCamera.GoToStandard(() => {
                playerThirdPersonInput.enabled = true;
                CamGUI.ShowLifes();
                });
        }

        //Camera moves in front of the player and disables character controls
        if (Input.GetKeyDown(KeyCode.Space) && GameCamera.GetState() == GameCamera.CamState.STANDARD)
        {
            
            GameCamera.GoToCloseUp(() => {
                //DialogCanvas.ShowDialog(initialText);
                playerThirdPersonInput.enabled = false;
                CamGUI.HideLifes();
            });
        }

        foreach (var enemy in enemys)
        {
            if (enemy.activeInHierarchy)
                return;
        }
        EndGameMenu.Victory();
    }
}
