using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new WeaponItem", menuName = "Item/Weapon")]
public class WeaponItem : EQItem
{
    public override void Use()
    {
        GameObject player = Inventory.instance.player;
        player.GetComponent<ProtagonistBehavior>().HasWeaponNow();
    }
}
