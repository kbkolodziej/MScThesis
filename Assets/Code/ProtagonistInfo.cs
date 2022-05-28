using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistInfo : MonoBehaviour
{
    public static ProtagonistInfo protagonist;
    private Vector2 positionOnMain;

    public GameObject player;

    public Vector2 PositionOnMain { get => positionOnMain; set => positionOnMain = value; }

    // Start is called before the first frame update

    private void Awake()
    {
        protagonist = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
