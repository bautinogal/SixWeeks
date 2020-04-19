using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    Button button;
    [SerializeField]
    string[] texts;
        
    public void OnStart(Action cb)
    {
        FindObjectOfType<MainMenu>().gameObject.SetActive(false);
        gameObject.SetActive(true);
        button = GetComponentInChildren<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => cb());
    }
}
