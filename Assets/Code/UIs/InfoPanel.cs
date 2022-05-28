using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public string[] infoLines = new string[] { };
    public bool isAchievementInfo = false;

    private int index;
    private float time = 2.0f;
    private float timer = 0.0f;
    private string[] interactableItems = { "Nonpickable", "Board", "Pickable"};
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        // StartInfo();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if ((interactWith != null && Array.IndexOf(interactableItems, interactWith.tag) != -1) || isAchievementInfo)
        {
            if (textComponent.text == infoLines[index])
            {
                timer += Time.deltaTime;
                if (timer > time)
                {
                    NextLine();
                    timer = 0.0f;
                }
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = infoLines[index];
            }
        }
    }

    public void StartInfo()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in infoLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < infoLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            timer += Time.deltaTime;
            if (timer > time)
            {
                if (isAchievementInfo)
                    TurnOffAchievementSettings();
                gameObject.SetActive(false);
            }
        }
    }

    void TurnOffAchievementSettings()
    {
        textComponent.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
        textComponent.alignment = TextAlignmentOptions.Left;
        isAchievementInfo = false;
    }
}
