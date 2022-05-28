using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InteractionMethods : Component
{
    public void PickableInteraction(GameObject interactionWith)
    {
        EQItem pickable;
        if (interactionWith.GetComponent<ItemBehavior>().type == null)
        {
            pickable = ScriptableObject.CreateInstance<EQItem>();
            pickable.itemName = interactionWith.name;
            SpriteRenderer iconLoader = interactionWith.GetComponent<SpriteRenderer>();
            pickable.icon = iconLoader.sprite;
        } else
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
        GameObject infoPanel = Inventory.instance.infoPanel;
        if (!infoPanel.activeSelf)
            infoPanel.SetActive(true);
        infoPanel.GetComponent<InfoPanel>().infoLines = interactionWith.GetComponent<Nonpickable>().GetMyInfo();
    }
}
