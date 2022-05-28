using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    public static TextHandler instance;
    private bool somethingToSay = false;
    private string message;
    private float displayTimer = 3.0f;
    private float timer = 0f;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        instance.GetComponent<UnityEngine.UI.Text>().text = message;
    }

    public void WriteMessage(string msg)
    {
        message = msg;
        timer = Time.time + displayTimer;
        if(Time.time > timer)
        {
            message = ""; // ?
        }
    }
}
