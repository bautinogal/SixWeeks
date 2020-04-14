using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Class that controls dialog display
public class DialogCanvas : MonoBehaviour
{
    static DialogCanvas instance;
    //TODO: find a way to avoid dependency injections
    [SerializeField]
    TextMeshProUGUI content;
    

    GameObject child;

    List<string> texts = new List<string>();
    int pos = 0;

    Action cb;

    //Singleton Pattern:
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            child = transform.GetChild(0).gameObject;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        ShowDialog("Hola Mundo!");
    }

    private void Start()
    {
        HideDialog();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Clear) || 
            Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetMouseButtonDown(0)
            )
        {
            Next();
        }
    }

    //U: Hides dialog box
    public static void HideDialog()
    {
        instance.pos = 0;
        instance.texts.Clear();
        instance.child.SetActive(false);
    }

    //U: Enables dialog box and prints message
    public static void ShowDialog(string[] texts, Action cb = null)
    {
        if (instance.cb != null)
            instance.cb();
        instance.cb = cb;
        instance.pos = 0;
        instance.texts.Clear();
        instance.texts.AddRange(texts);
        instance.content.text = instance.texts[instance.pos];
        instance.child.SetActive(true);
    }

    //U: Enables dialog box and prints message
    public static void ShowDialog(string text, Action cb = null)
    {
        var param = new string[1];
        param[0] = text;
        ShowDialog(param, cb);
    }

    public void Next()
    {
        pos++;
        if(pos < instance.texts.Count)
        {
            content.text = instance.texts[pos];
        }
        else
        {
            HideDialog();
            if(cb != null)
                instance.cb();
            instance.cb = null;
        }
    }
}
