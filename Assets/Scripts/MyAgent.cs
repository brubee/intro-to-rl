using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MyAgent : Agent
{
    Rigidbody rb;
    public float speed = 5f;
    private bool isGrounded = true;

    private Vector3 startingPos = new Vector3(0f, 1f, -96f);

    private float boundXLeft = -6f;
    private float boundXRight = 6f;

    private enum ACTIONS
    {
        NOTHING = 0,
        LEFT = 1,
        RIGHT = 2,
        JUMP = 3
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= 2;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingPos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // this should be done by the RayPerceptionSensor, but if it doesn't work how I want it to, I'll come back to it :')
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == -1)
        {
            actions[0] = (int)ACTIONS.LEFT;
        }
        else if (horizontal == 1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
        }
        else
        {
            actions[0] = (int)ACTIONS.NOTHING;
        }

        if (vertical == 1)
        {
            actions[0] = (int)ACTIONS.JUMP;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

        switch (actionTaken)
        {
            case (int)ACTIONS.NOTHING:
                transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.LEFT:
                if (transform.localPosition.x >= boundXLeft)
                    transform.Translate(-Vector3.right * speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.RIGHT:
                if (transform.localPosition.x <= boundXRight)
                    transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.JUMP:
                if (isGrounded)
                {
                    rb.AddForce(Vector3.up * 11, ForceMode.Impulse);
                    isGrounded = false;
                }
                break;
        }

        AddReward(0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            AddReward(1.0f);
            // and then I also want to create at least one more different level, which would be loaded in from here, once the agent gets to the goal
            EndEpisode();
        }
    }
}
