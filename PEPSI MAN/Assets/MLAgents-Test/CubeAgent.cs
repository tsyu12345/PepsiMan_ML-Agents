using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CubeAgent: Agent {

    [SerializeField] private Transform target;
    private Rigidbody _rBody;


    /**
    * シーン開始時に呼び出される。初期化処理を行う。
    */
    public override void Initialize() {
        _rBody = GetComponent<Rigidbody>();
    }

    /**
    * 各エピソード開始時に呼び出される.
    * エージェントの初期位置の設定や、ターゲットの位置のリセットを行う。
    */
    public override void OnEpisodeBegin() {
        if (this.transform.localPosition.y < 0) {
            // If the Agent fell, zero its momentum
            _rBody.angularVelocity = Vector3.zero;
            _rBody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        }

        // Move the target to a new spot// Targetの位置のリセット
        target.localPosition = new Vector3(Random.value*8-4, 0.5f, Random.value*8-4);

    }


    

    /**
    * 行動の実行時に呼び出される。
    * 行動の設定と、報酬の設定を行う。
    * @param actions 行動の設定を格納するActionBuffers
    */
    public override void OnActionReceived(ActionBuffers actions) {
        
       Vector3 dirToGo = Vector3.zero;
       Vector3 rotateDir = Vector3.zero;

        int action = actions.DiscreteActions[0];
        if(action == 1) dirToGo = transform.forward * 1f; //前進
        if(action == 2) dirToGo = transform.forward * -1f; //後退
        if(action == 3) rotateDir = transform.up * -1f; //左回転
        if(action == 4) rotateDir = transform.up * 1f; //右回転

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        _rBody.AddForce(dirToGo * 0.5f, ForceMode.VelocityChange);


        // Rewards
        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f) {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        if (transform.localPosition.y < 0) {
            EndEpisode();
        }
    }

    /**
    * ヒューリスティックモード時に呼び出される。
    * キーボードの入力を受け付け、行動を設定する。
    */
    public override void Heuristic(in ActionBuffers actionsOut) {
        var actions = actionsOut.DiscreteActions;
        actions[0] = 0;
        if (Input.GetKey(KeyCode.W)) actions[0] = 1;
        if (Input.GetKey(KeyCode.S)) actions[0] = 2;
        if (Input.GetKey(KeyCode.A)) actions[0] = 3;
        if (Input.GetKey(KeyCode.D)) actions[0] = 4;
    }

}

