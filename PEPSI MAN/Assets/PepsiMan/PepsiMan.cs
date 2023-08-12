using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/// <summary>
/// ペプシマン（プレイヤー）のクラス。
/// StarterAssetsInputs（親：MonoBehaviour）を継承している。
/// MonoBehavior > StarterAssetsInputs > PepsiManの構図
/// </summary>
/// 

namespace StarterAssets {
    public class PepsiMan : Agent{
        
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("Status Flags")]
        public bool stoping = false;
        public bool tumbling = false;

        private InputAction oKeyAction;

        private CharacterController controller;

        private Rigidbody _rBody;

        void Start() {

            /*実装テスト用*/
            oKeyAction = new InputAction("PressO", InputActionType.Button, "<Keyboard>/o");
            oKeyAction.performed += ctx => Sliding();
            oKeyAction.Enable();

            controller = GetComponent<CharacterController>();
        }

        public override void Initialize() {
            _rBody = GetComponent<Rigidbody>();
        }

        void Update() {
            
        }

        #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED


            public void OnLook(InputValue value)
            {
                if(cursorInputForLook)
                {
                    LookInput(value.Get<Vector2>());
                }
            }

            public void OnJump(InputValue value)
            {
                JumpInput(value.isPressed);
            }

            public void OnSprint(InputValue value)
            {
                SprintInput(value.isPressed);
            }
    #endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        } 

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public override void OnEpisodeBegin() {
            _rBody.angularVelocity = Vector3.zero;
            _rBody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(-322.31f, 69.69f, -66.67f);
        }

        public override void OnActionReceived(ActionBuffers actions) {

            //選択した行動を取得する
            float x_movement = actions.ContinuousActions[0];
            int discreteActions = actions.DiscreteActions[0];

            //行動の実行
            move = new Vector2(x_movement, 1);
            MoveInput(move);
            //ジャンプする場合
            if (discreteActions == 1) {
                JumpInput(true);
            }
            if(discreteActions == 2) {
                SprintInput(true);
            }

            if(transform.localPosition.y < 0) {
                //エピソードを終了する
                EndEpisode();
            }
        }


        public override void Heuristic(in ActionBuffers actionsOut) {
            var ContinuousActionsOut = actionsOut.ContinuousActions;
            var DiscreteActionsOut = actionsOut.DiscreteActions;

            DiscreteActionsOut[0] = 0;
            if(Input.GetKey(KeyCode.Space)) {
                DiscreteActionsOut[0] = 1;
            }
            if(Input.GetKey(KeyCode.LeftShift)) {
                DiscreteActionsOut[0] = 2;
            }
            ContinuousActionsOut[0] = Input.GetAxis("Horizontal");
        }


        void OnTriggerEnter(Collider other) {
            //衝突したタグがtargetの場合
            if (other.gameObject.CompareTag("target")) {
                //報酬を与えてエピソードを終了する
                SetReward(1.0f);
            }

            //衝突したタグがobstacleの場合
            if (other.gameObject.CompareTag("obstacle")) {
                Tumble();
                //報酬を与えてエピソードを終了する
                SetReward(-1.0f);
                EndEpisode();
            }
        }

        
        public void OnMove(InputValue value) {
            //yは常に1にしていする
            //Debug.Log(value.Get<Vector2>());
            //Vector2 movement = value.Get<Vector2>();
            //move = new Vector2(movement.x, 1);
            //MoveInput(move);
        }


        public void Stop() {
            //動きを止める
            move = Vector2.zero;
            MoveInput(move);
            stoping = true;
        }


        public void Tumble() {
            //動きを止めて、転倒用のアニメーションを再生する
            Stop();
            //todo:現在は仮でtransformで回転させるだけ
            transform.Rotate(new Vector3(90, 0, 0));
            controller.height = 0.5f;
        }


        /// <summary>
        /// スライディングの実装
        /// 
        /// </summary>
        public void Sliding() {
            Stop();
            transform.Rotate(new Vector3(-90, 0, 0));
            controller.height = 0.5f;
        }

    }
}
