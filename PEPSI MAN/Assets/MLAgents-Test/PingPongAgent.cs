using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class PingPongAgent : Agent {

    public int agentId;
    public GameObject ball;
    Rigidbody ballRb;

    public override void Initialize() {
        ballRb = ball.GetComponent<Rigidbody>();

    }

    public override void CollectObservations(VectorSensor sensor) {
        float dir = (agentId == 0) ? 1.0f : -1.0f;
        sensor.AddObservation(ball.transform.localPosition.x * dir);
        sensor.AddObservation(ball.transform.localPosition.z * dir);
        sensor.AddObservation(ballRb.velocity.x * dir);
        sensor.AddObservation(ballRb.velocity.z * dir);

        sensor.AddObservation(transform.localPosition.x * dir);
    }

    void OnCollisionEnter(Collision other) {
        AddReward(0.1f);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float dir = (agentId == 0) ? 1.0f : -1.0f;
        int action = actions.DiscreteActions[0];

        Vector3 pos = transform.localPosition;
        if(action == 1) pos.x -= 0.2f * dir;
        if(action == 2) pos.x += 0.2f * dir;
        if(pos.x < -4.0f) pos.x = -4.0f;
        if(pos.x > 4.0f) pos.x = 4.0f;

        transform.localPosition = pos;
    }


    public override void Heuristic(in ActionBuffers actionBuffers) {
        var action = actionBuffers.DiscreteActions;
        action[0] = 0;
        if(Input.GetKey(KeyCode.A)) action[0] = 1;
        if(Input.GetKey(KeyCode.D)) action[0] = 2;
    }
}
