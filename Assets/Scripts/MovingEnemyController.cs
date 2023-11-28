using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemyController : MonoBehaviour
{
    public float speed = 5f;
    private float zPos;
    private Vector3 startingPos;

    private float boundLeft = -9f;
    private float boundRight = 9f;

    private void Start()
    {
        zPos = transform.position.z;
        startingPos = new Vector3(9f, 0.75f, zPos);

        transform.position = startingPos;
    }

    void FixedUpdate()
    {
        if(transform.localPosition.x >= boundLeft && transform.localPosition.x <= boundRight)
        {
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            return;
        }
    }
}
