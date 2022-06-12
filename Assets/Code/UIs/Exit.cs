using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public Button ExitButton;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TEST1");
        ExitButton.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        Application.Quit();
    }
}
