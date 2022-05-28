using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "new Consumable", menuName = "Item/Consumable")]
public class Consumable : EQItem
{
    public int heal = 0;

    public override void Use()
    {
        GameObject player = Inventory.instance.player;
        player.GetComponent<ProtagonistBehavior>().Heal(heal);
        Inventory.instance.RemoveItem(this);
    }
}
