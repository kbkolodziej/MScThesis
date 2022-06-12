using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines = new string[] { };
    public float textSpeed;
    public GameObject player;
    private string dialogue;
    private StringReader reader;
    private int index;
    public GameObject continueButton;
    private bool answer = false;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        // StartDialogue();
        gameObject.SetActive(false);
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject interactWith = player.GetComponent<ProtagonistBehavior>().interactionWith;
        if (interactWith != null && interactWith.tag == "Person")
        {
            ReadDialogue(interactWith.name);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (textComponent.text == lines[index])
                NextLine();
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                interactWith.GetComponent<NPCBehavior>().isTalking = false;
            }
        }
    }

    void ReadDialogue(string name)
    {
        continueButton.SetActive(true);
        dialogue = ReadingDialogues.ReadDialogue(name);
        reader = new StringReader(dialogue);
        string line = "";
        List<string> newDialogue = new List<string>();
        bool found = false;
     
        while (!found)
        {
            line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                found = true;
                break;
            }
            newDialogue.Add(line);
        }
        lines = newDialogue.ToArray();
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); // change to wait for input
        }

    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            if(index <= 1) textComponent.text = string.Empty;
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            gameObject.SetActive(false);
            continueButton.SetActive(false);

        }
    }
    public void OnClick()
    {
        NextLine();
    }
}