using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlotItem", menuName = "Item/Plot")]
public class PlotItem : EQItem
{
    public override void Use()
    {
        Debug.Log("Important item");
       // Inventory.instance.RemoveItem(this);
    }
}
