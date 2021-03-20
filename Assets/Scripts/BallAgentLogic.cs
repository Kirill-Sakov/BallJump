using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class BallAgentLogic : Agent
{
    Rigidbody m_BallRigitBody;

    // Start is called before the first frame update
    void Start()
    {
        m_BallRigitBody = GetComponent<Rigidbody>();
    }

    public Transform target;
    public override void OnEpisodeBegin()
    {
        this.m_BallRigitBody.velocity = Vector3.zero;
        this.m_BallRigitBody.angularVelocity = Vector3.zero;
        this.transform.localPosition = new Vector3(-9, 0.5f, 0);

        target.localPosition = new Vector3(12 + Random.value * 8, Random.value * 3, Random.value * 10 - 5);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        sensor.AddObservation(m_BallRigitBody.velocity);
    }

    public float Speed;
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;

        controlSignal.x = vectorAction[0];

        if (vectorAction[1] == 2)
        {
            controlSignal.z = 1;
        }
        else
        {
            controlSignal.z = -vectorAction[1];
        }


        if (this.transform.localPosition.x < 8.5f)
        {
            m_BallRigitBody.AddForce(controlSignal * Speed);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }
}
