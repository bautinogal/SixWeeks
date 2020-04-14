using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject creditsCanvas;
    [SerializeField]
    Button fps;
    [SerializeField]
    GameObject controlesCanvas;
    [SerializeField]
    Button controles;
    [SerializeField]
    Button creditos;
    [SerializeField]
    Button salir;
    [SerializeField]
    Intro intro;


    private void Start()
    {
        fps.onClick.AddListener(() => intro.OnStart(() => SceneManager.LoadScene(3)));
        controles.onClick.AddListener(() =>
        {
            controlesCanvas.SetActive(true);
            gameObject.SetActive(false);
        });
        creditos.onClick.AddListener(() =>
        {
            creditsCanvas.SetActive(true);
            gameObject.SetActive(false);
        });
        salir.onClick.AddListener(() => Application.Quit());
    }

}
