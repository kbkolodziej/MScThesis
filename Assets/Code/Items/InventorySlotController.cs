using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour//, IPointerClickHandler
{
    public EQItem item;

    private void Start()
    {
        UpdateInfo();
    }
    public void Use()
    {
        if (item)
        {
            item.Use();
        }
    }
    public void UpdateInfo()
    {
        Text displayText = transform.Find("Text").GetComponent<Text>();
        Image displayImage = transform.Find("Image").GetComponent<Image>();
        if (item)
        {
            displayText.text = item.itemName;
            displayImage.sprite = item.icon;
            displayImage.color = Color.white;
        }
        else
        {
            displayText.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.clear;
        }
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //        Use();
    //    else if (eventData.button == PointerEventData.InputButton.Middle)
    //        Debug.Log("Middle click");
    //    else if (eventData.button == PointerEventData.InputButton.Right)
    //    {
    //        if(item != null) Inventory.instance.WriteItemInfo(item.itemInfo);
    //    }
            
    //}
}
