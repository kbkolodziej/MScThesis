using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    private ProtagonistBehavior protagonist;
    private float range = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        protagonist = GameObject.FindObjectOfType(typeof(ProtagonistBehavior)) as ProtagonistBehavior;

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0))) // interaction
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
          //  Debug.Log(mousePos2D);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(protagonist.transform.position.x, 
                protagonist.transform.position.y), mousePos2D, 4f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Pickable")
                {
                    EQItem pickable = ScriptableObject.CreateInstance<EQItem>();
                    pickable.itemName = hit.collider.name;
                    SpriteRenderer iconLoader = hit.collider.GetComponent<SpriteRenderer>();
                    pickable.icon = iconLoader.sprite;
                    Inventory.instance.AddItem(pickable);
                    hit.collider.gameObject.SetActive(false);
                    TextHandler.instance.GetComponent<UnityEngine.UI.Text>().text = "Picked " + hit.collider.name + "!";
                    Debug.Log("Picked " + hit.collider.name + "!");
                }
                else if (hit.transform.tag == "Nonpickable")
                    Debug.Log("This is a " + hit.collider.name);
                else if (hit.transform.tag == "Board")
                    Debug.Log("Welcome to Biraffe3. Village is down that way. Right to lake. Hf");
            }
        }

    }
}
