using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ProtagonistBehavior : MonoBehaviour
{
    private string[] interactableItems = { "Nonpickable", "Board", "Pickable", "Door", "House", "Person" };
    private Rigidbody2D protagonist;
    public GameObject interactionWith = null;
    private int health = 10;
    private int speed = 10;

    private bool shownAchievement = false;

    private int points = 0;
    private bool hasWeapon = false;
    private int killCount = 0;
    private List<string> achievements = new List<string>();

    private string equipped = "Bron zalozona i gotowa do uzycia!";
    private string unequipped = "Bron sciagnieta.";

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = Vector2.zero;
        protagonist = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Menu();
        Interaction();
        Inventory.instance.WriteHealthAndPoints();
    }

    void Movement()
    {
        if (!checkIfTalking())
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(moveX, moveY, 0.0f);
            protagonist.velocity = movement * speed;
        }
    }

    void Menu()
    {
        if((Input.GetKeyDown(KeyCode.Tab))|| (Input.GetKeyDown(KeyCode.JoystickButton1) && interactionWith))
        {
            
            GameObject panel = Inventory.instance.inventoryPanel;
            if (!panel.activeSelf)
                panel.SetActive(true);
            else
            {
                Inventory.instance.TurnOffItemText();
                panel.SetActive(false);
            }
        }
    }

    private void Interaction()
    {
        // peaceful interaction
        if ((Input.GetKeyDown(KeyCode.E) && interactionWith) || (Input.GetKeyDown(KeyCode.JoystickButton0) && interactionWith))
        {
            switch (interactionWith.tag)
            {
                case "Pickable":
                    GetComponent<InteractionMethods>().PickableInteraction(interactionWith);
                    break;
                case "Board":
                    GetComponent<InteractionMethods>().BoardInteraction(interactionWith);
                    break;
                case "Nonpickable":
                    GetComponent<InteractionMethods>().NonpickableInteraction(interactionWith);
                    break;
                case "Person":
                    GameObject dialoguePanel = Inventory.instance.dialoguePanel;
                    if (!dialoguePanel.activeSelf)
                        dialoguePanel.SetActive(true);
                    break;
                case "Door":
                    string openTo = interactionWith.GetComponent<Door>().openTo;
                    SceneManager.LoadScene(openTo);
                    break;
                case "House":
                    string getTo = interactionWith.GetComponent<House>().houseOf;
                    if (!String.IsNullOrEmpty(getTo))
                    {
                        SceneManager.LoadScene(interactionWith.GetComponent<House>().houseOf);
                    }
                    else
                        Debug.Log("I cant get there");
                    break;
            }

        }
        // not so peaceful
        else if ((Input.GetKeyDown(KeyCode.Q) && interactionWith && hasWeapon) || (Input.GetKeyDown(KeyCode.JoystickButton2) && interactionWith && hasWeapon))
        {
            if(interactionWith.tag == "Person")
            {
                IncreasePoints(interactionWith.GetComponent<NPCBehavior>().GetPointsValue());
                killCount += 1;
                interactionWith.GetComponent<NPCBehavior>().Die();
                GiveAchievement();
            }
        }
    }

    public void Heal(int value)
    {
        health += value;
        points += 10;
    }

    public void IncreasePoints(int value)
    {
        points += value;
    }

    private void GiveAchievement()
    {
        if (killCount == 1 && !shownAchievement)
        {
            achievements.Add("Murderer");
            GetComponent<AchievementManager>().AchievementInteraction("Murderer");
        }
        shownAchievement = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log(other.tag);
        if (Array.IndexOf(interactableItems, other.tag) != -1)
            interactionWith = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Array.IndexOf(interactableItems, other.tag) != -1)
        {
            if (interactionWith == other.gameObject)
                interactionWith = null;
        }
    }

    private bool checkIfTalking()
    {
        GameObject panel = Inventory.instance.dialoguePanel;
        GameObject infoPanel = Inventory.instance.infoPanel;
        if (panel.activeSelf)
            return true;
        else if (infoPanel.activeSelf && !infoPanel.GetComponent<InfoPanel>().isAchievementInfo)
            return true;
        else return false;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetPoints()
    {
        return points;
    }

    public void HasWeaponNow()
    {
        if (hasWeapon)
        {
            hasWeapon = false;
            Inventory.instance.WriteItemInfo(unequipped);
        }
        else
        {
            hasWeapon = true;
            Inventory.instance.WriteItemInfo(equipped);
        }

    }


}
