using System;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public int hp;
    public GameObject[] plotItems;
    public Transform[] points;
    private Rigidbody2D oneself;
    private BoxCollider2D myCollider;
    private int destPoint = 0;
    private float chillTime;
    private float timer = 0.0f;
    private int pointsValue = 10;
    public bool isThere = false;
    public bool isTalking = false;
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        oneself = gameObject.GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        chillTime = UnityEngine.Random.Range(5.0f, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (points.Length > 0)
        {
            if (isThere)
                ChillABit();
            else
                GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        Vector3 temp = new Vector3(GetXDirection(), GetYDirection(), 0);
        transform.position += temp;
    }

    void ChillABit()
    {
        timer += Time.deltaTime;
        oneself.velocity = Vector2.zero;
        if (timer > chillTime)
        {
            destPoint = (destPoint + 1) % points.Length;
            timer = 0.0f;
            isThere = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Place" && points.Length > 0 && collision.name == points[destPoint].name)
            isThere = true;
        else if (collision.tag == "Player")
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        else if (collision.tag == "Person")
            myCollider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Person")
            myCollider.enabled = true;
    }

    float GetXDirection()
    {
        if (Math.Round(transform.position.x) > Math.Round(points[destPoint].position.x))
            return -speed;
        else if (Math.Round(transform.position.x) == Math.Round(points[destPoint].position.x))
            return 0;
        else
            return speed;
    }

    float GetYDirection()
    {
        if (Math.Round(transform.position.y) > Math.Round(points[destPoint].position.y))
            return -speed;
        else if (Math.Round(transform.position.y) == Math.Round(points[destPoint].position.y))
            return 0;
        else
            return speed;
    }

    public int GetPointsValue()
    {
        return pointsValue;
    }

    public void Die()
    {
        // death animation
        gameObject.SetActive(false);
    }
}
