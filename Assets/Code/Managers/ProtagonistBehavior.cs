using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class ProtagonistBehavior : MonoBehaviour
{
    public string prevNPC = null;
    public new Camera camera;
    private string[] interactableItems = { "Nonpickable", "Board", "Pickable", "Door", "House", "Person" };
    private Rigidbody2D protagonist;
    public GameObject interactionWith = null;
    private int health = 10;
    private int speed = 10;
    private float logTimer = 0.0f;
    private float logTime = 0.1f;
    private FileStream oFileStream = null;

    private List<string> npcs = new List<string>();
    private int npcCounter = 0;
    private bool shownAchievement = false;
    private int interactionClicks = 0;
    private int uniqueInteractions = 0;
    private List<string> npcStoriesStatus = new List<string>();
    private string currentWeapon;
    private List<string> items;
    private int openedChests;

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
        string logPath = Application.dataPath;
        oFileStream = new FileStream(logPath + "/CollectedLogs.txt", FileMode.Create);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LogUpdate();
        Menu();
        Interaction();
        Inventory.instance.WriteHealthAndPoints();
        logTimer += Time.deltaTime;
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
                    PickableInteraction(interactionWith);
                    break;
                case "Board":
                    BoardInteraction(interactionWith);
                    break;
                case "Nonpickable":
                    NonpickableInteraction(interactionWith);
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
public void LogUpdate()
    {
        if (logTimer > logTime)
        {
            ProtagonistLogs protagonistInfo = new ProtagonistLogs();
            protagonistInfo.x = transform.position.x;
            protagonistInfo.y = transform.position.y;
            protagonistInfo.achievements = achievements;
            protagonistInfo.interactionClicks = interactionClicks;
            protagonistInfo.uniqueInteractions = uniqueInteractions;
            protagonistInfo.npcStoriesStatus = npcStoriesStatus;
            if (((interactionWith != null) && (interactionWith.tag == "Person")) && (prevNPC != interactionWith.name)) {
                npcs.Add(interactionWith.name);
                prevNPC = interactionWith.name;
            }
            protagonistInfo.killed = killCount;
            if ((interactionWith != null) && (interactionWith.tag == "Person")) npcCounter++;
            protagonistInfo.currentWeapon = currentWeapon;
            protagonistInfo.items = items;
            protagonistInfo.hp = health;
            protagonistInfo.exp = points;
            protagonistInfo.openedChests = openedChests;
            protagonistInfo.npcs = npcs;

    // General info
              protagonistInfo.timestamp = new System.DateTimeOffset(System.DateTime.Now).ToUnixTimeMilliseconds();
//            protagonistInfo.xMin = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x - 2 * (GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x - Mathf.Abs(new Vector3(GetComponent<Camera>().transform.position.x, 0, 0).x));
//            protagonistInfo.yMin = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y - 2 * (GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y - Mathf.Abs(new Vector3(0, GetComponent<Camera>().transform.position.y, 0).y));
//            protagonistInfo.xMax = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
//            protagonistInfo.yMax = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;

            string json = JsonUtility.ToJson(protagonistInfo);
            byte[] bytes = Encoding.ASCII.GetBytes(json);
            oFileStream.Write(bytes, 0, bytes.Length);
            logTimer = 0.0f;
        }
    }
    public void PickableInteraction(GameObject interactionWith)
    {
        EQItem pickable;
        if (interactionWith.GetComponent<ItemBehavior>().type == null)
        {
            pickable = ScriptableObject.CreateInstance<EQItem>();
            pickable.itemName = interactionWith.name;
            SpriteRenderer iconLoader = interactionWith.GetComponent<SpriteRenderer>();
            pickable.icon = iconLoader.sprite;
        }
        else
        {
            pickable = interactionWith.GetComponent<ItemBehavior>().type;
        }
        Inventory.instance.AddItem(pickable);

        Debug.Log("Picked " + interactionWith.name + "!");
        interactionWith.gameObject.SetActive(false);
    }

    public void DoorInteraction(GameObject interactionWith, Vector2 positionMain, Vector2 position)
    {
        string openTo = interactionWith.GetComponent<Door>().openTo;
        if (openTo == "Main")
        {
            position = positionMain;
            // Debug.Log("main " + positionMain);
            // Debug.Log("actual position " + position);
        }
        SceneManager.LoadScene(openTo);
    }

    public void HouseInteraction(GameObject interactionWith, Vector2 positionMain, Vector2 position)
    {
        string getTo = interactionWith.GetComponent<House>().houseOf;
        if (!String.IsNullOrEmpty(getTo))
        {
            positionMain = position;
            //  Debug.Log("main " + positionMain);
            //  Debug.Log("actual position " + position);
            SceneManager.LoadScene(interactionWith.GetComponent<House>().houseOf);
        }
        else
            Debug.Log("I cant get there");
    }

    public void BoardInteraction(GameObject interactionWith)
    {
        GameObject infoPanel = Inventory.instance.infoPanel;
        if (!infoPanel.activeSelf)
            infoPanel.SetActive(true);
        infoPanel.GetComponent<InfoPanel>().infoLines = interactionWith.GetComponent<BoardInfo>().info;
    }

    public void NonpickableInteraction(GameObject interactionWith)
    {
        Debug.Log("Jestem w nonpickable!");
        GameObject infoPanel = Inventory.instance.infoPanel;
        if (!infoPanel.activeSelf)
            infoPanel.SetActive(true);
        infoPanel.GetComponent<InfoPanel>().infoLines = interactionWith.GetComponent<Nonpickable>().GetMyInfo();
    }
}
