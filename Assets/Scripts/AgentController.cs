using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE { Left, Mid, Right}

public class AgentController : MonoBehaviour
{
    Rigidbody rb;

    private SIDE m_side;
    private float newXPos = 0f;
    private float XValue = 6f;

    public float speed = 2f;
    bool isGrounded = true;

    private Vector3 startingPos = new Vector3(0f, 1f, -96f);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= 2;
        m_side = SIDE.Mid;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            Debug.Log("jump");
            rb.AddForce(Vector3.up * 11, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if(m_side == SIDE.Mid)
            {
                Debug.Log("from MID to LEFT");
                newXPos = -XValue;
                m_side = SIDE.Left;
                transform.Translate(new Vector3(newXPos, 0, 0));
            }
            else if(m_side == SIDE.Right)
            {
                Debug.Log("from RIGHT to MID");
                newXPos = 0;
                m_side = SIDE.Mid;
                transform.Translate(new Vector3(newXPos, 0, 0));
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if(m_side == SIDE.Mid)
            {
                Debug.Log("from MID to RIGHT");
                newXPos = XValue;
                m_side = SIDE.Right;
                transform.Translate(new Vector3(newXPos, 0, 0));
            }
            else if(m_side == SIDE.Left)
            {
                Debug.Log("from LEFT to MID");
                newXPos = 0;
                m_side = SIDE.Mid;
                transform.Translate(new Vector3(newXPos, 0, 0));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shrub") || collision.gameObject.CompareTag("Log") || collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Branch") || collision.gameObject.CompareTag("Sanic"))
        {
            Debug.Log("Hit an obstacle!");
            speed = 0f;
            isGrounded = false;
        }
        else if(collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }
}
