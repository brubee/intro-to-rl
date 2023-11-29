using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class MyAgent : Agent
{
    Rigidbody rb;
    public float m_speed = 5f;
    public float a_speed = 5f;
    private bool isGrounded = true;

    private Vector3 startingPos = new Vector3(0f, 1f, -96f);

    private float boundXLeft = -6.5f;
    private float boundXRight = 6.5f;
    private float boundY = 2f;

    public int episodeCounter = 0;
    private bool secondRound = false;

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
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * m_speed * Time.fixedDeltaTime);

        if (transform.position.y <= -2f)
        {
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingPos;
        isGrounded = true;
        episodeCounter++;

        if(episodeCounter >= 200)
        {
            SceneManager.LoadScene("Scene2");
        }

        if(SceneManager.GetActiveScene().name == "Scene2" && episodeCounter >= 100)
        {
            SceneManager.LoadScene("Scene1");
            secondRound = true;
        }

        if(SceneManager.GetActiveScene().name == "Scene1" && secondRound == true && episodeCounter >= 50)
        {
            SceneManager.LoadScene("Scene2");
        }

        if(SceneManager.GetActiveScene().name == "Scene2" && secondRound == true && episodeCounter >= 50)
        {
            SceneManager.LoadScene("Scene1");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // done with RayPerceptionSensor
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
                break;
            case (int)ACTIONS.LEFT:
                if (transform.localPosition.x >= boundXLeft)
                    transform.Translate(-Vector3.right * a_speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.RIGHT:
                if (transform.localPosition.x <= boundXRight)
                    transform.Translate(Vector3.right * a_speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.JUMP:
                if (isGrounded && transform.localPosition.y < boundY)
                {
                    rb.AddForce(Vector3.up, ForceMode.Impulse);
                }
                AddReward(-0.005f);
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
            AddReward(10.0f);
            EndEpisode();
        }
    }
}
