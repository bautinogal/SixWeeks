using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField]
    GameObject main;
    [SerializeField]
    Button salir;

    private void Start()
    {
        salir.onClick.AddListener(() =>
        {
            main.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
