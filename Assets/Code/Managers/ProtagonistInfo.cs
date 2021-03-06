using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ProtagonistLogs
{
    // Protagonist
    public float x;
    public float y;
    public long timestamp;
    public List<string> achievements;
    public List<string> npcs;
    public int npcCounter;
    public int interactionClicks;
    public int uniqueInteractions;
    public List<string> npcStoriesStatus;
    public int exp;
    public int hp;
    public List<string> items;
    public int killed;
    public string currentWeapon;
    public int openedChests;
}
