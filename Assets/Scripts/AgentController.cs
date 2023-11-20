using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public float speed = 2f;

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.W))
        {

        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.S))
        {

        }
    }
}
