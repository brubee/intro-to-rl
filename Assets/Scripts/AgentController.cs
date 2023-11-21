using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SIDE { Left, Mid, Right}

public class AgentController : MonoBehaviour
{
    Rigidbody rb;
    CharacterController cc;

    private SIDE m_side;
    public float newXPos = 0f;
    public float XValue = 6f;

    public float speed = 5f;
    public float jumpHeight = 10f;
    private Vector3 playerVelocity;

    private Vector3 startingPos = new Vector3(0f, 1f, -96f);
    private float gravity = -9.81f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        m_side = SIDE.Mid;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(m_side == SIDE.Mid)
            {
                Debug.Log("from MID to LEFT");
                newXPos -= XValue;
                m_side = SIDE.Left;
            }
            else if(m_side == SIDE.Right)
            {
                Debug.Log("from RIGHT to MID");
                newXPos = 0;
                m_side = SIDE.Mid;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(m_side == SIDE.Mid)
            {
                Debug.Log("from MID to RIGHT");
                newXPos += XValue;
                m_side = SIDE.Right;
            }
            else if(m_side == SIDE.Left)
            {
                Debug.Log("from LEFT to MID");
                newXPos = 0;
                m_side = SIDE.Mid;
            }
        }

        Vector3 moveDir = new Vector3(newXPos - transform.position.x, 0, speed * Time.deltaTime);
        cc.Move(moveDir);

        if (Input.GetKeyDown(KeyCode.W) && cc.isGrounded)
        {
            Debug.Log("jump");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }
}
