using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<EQItem> itemList = new List<EQItem>();

    public GameObject player;
    public GameObject inventoryPanel;
    public GameObject dialoguePanel;
    public GameObject infoPanel;
    public Text healthPoints;
    public Text pointsAmount;
    public Text itemInfo;
    private float itemInfoTime = 5.0f;
    private float timer = 0.0f;

    public static Inventory instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        itemInfo.text = string.Empty;
        UpdatePanelSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePanelSlots()
    {
        int index = 0;
        foreach(Transform child in inventoryPanel.transform)
        {
            InventorySlotController slot = child.GetComponent<InventorySlotController>();
            if (index < itemList.Count)
                slot.item = itemList[index];
            else
                slot.item = null;

            slot.UpdateInfo();
            index++;

        }
    }

    public void AddItem(EQItem item)
    {
        if (itemList.Count < 9)
            itemList.Add(item);
        UpdatePanelSlots();
    }
    public void RemoveItem(EQItem item)
    {
        itemList.Remove(item);
        UpdatePanelSlots();
    }

    public void WriteHealthAndPoints()
    {
        string health = player.GetComponent<ProtagonistBehavior>().GetHealth().ToString();
        string points = player.GetComponent<ProtagonistBehavior>().GetPoints().ToString();
        healthPoints.GetComponent<Text>().text = "HP: " + health;
        pointsAmount.GetComponent<Text>().text = "Points: " + points;
    }

    public void WriteItemInfo(string text)
    {
        itemInfo.GetComponent<Text>().text = text;
    }

    public void TurnOffItemText()
    {
        itemInfo.text = string.Empty;
    }

}
