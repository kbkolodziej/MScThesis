using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Vector3 offset;
    public GameObject protagonist;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - protagonist.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowThePlayer();
    }

    void FollowThePlayer()
    {
        transform.position = protagonist.transform.position + offset;
    }

}
