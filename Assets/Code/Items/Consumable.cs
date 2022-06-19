using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "new Consumable", menuName = "Item/Consumable")]
public class Consumable : EQItem
{
    public int heal = 2;

    public override void Use()
    {
        Debug.Log(heal);
        GameObject player = Inventory.instance.player;
        GameObject dialogs = Inventory.instance.dialoguePanel;
        player.GetComponent<ProtagonistBehavior>().Heal(heal);
        dialogs.GetComponent<DialogueBox>().Heal(heal);
        Inventory.instance.RemoveItem(this);
    }
}
