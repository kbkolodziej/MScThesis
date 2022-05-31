using UnityEngine;
using System.Collections;

[System.Serializable]
public class InfoLoggerRocking
{
    // Protagonist
    public float x;
    public float y;
    public int deathCount;
    public int shootsCounter;
    public int hitCounter;
    public int money;
    public int collectedMoney;
    public int collectedHealth;
    public int health;

    // General
    public long timestamp;
    public string idOfSound;
    public string timestampOfSound;
    public float xMin;
    public float yMin;
    public float xMax;
    public float yMax;

}
