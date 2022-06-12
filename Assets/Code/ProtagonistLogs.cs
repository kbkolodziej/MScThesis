using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ProtagonistLogs
{
    // Protagonist
    public float x;
    public float y;
    public int areaNumber;
    public List<string> achievements;
    public int interactionClicks;
    public int uniqueInteractions;
    public List<string> npcStoriesStatus;
    public List<string> npcs;
    public int killed;
    public string currentNPC;
    public string currentWeapon;
    public List<string> items;
    public int hp;
    public int exp;
    public int openedChests;

    // General
    public long timestamp;
    public float xMin;
    public float yMin;
    public float xMax;
    public float yMax;

}
