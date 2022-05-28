using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EQItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string itemInfo;
    // Start is called before the first frame update
    public virtual void Use()
    {
        
    }

}
