using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

/// <summary>
/// ペプシマン（プレイヤー）のクラス。
/// StarterAssetsInputs（親：MonoBehaviour）を継承している。
/// MonoBehavior > StarterAssetsInputs > PepsiManの構図
/// </summary>
public class PepsiMan : StarterAssetsInputs{

    /**
    * TODO: パラメータ設定の見直し。
    */
    [Header("Action of PepsiMan")]
    public bool useTumble = false;
    public bool useStop = false;

    private InputAction oKeyAction;

    void Start() {

        oKeyAction = new InputAction("PressO", InputActionType.Button, "<Keyboard>/o");
        oKeyAction.performed += ctx => test();
        oKeyAction.Enable();
        
    }

    void Update() {
        if(useTumble) {
            Tumble();
        }
    }
    
    public override void OnMove(InputValue value) {
        //yは常に1にしていする
        //Debug.Log(value.Get<Vector2>());
        Vector2 movement = value.Get<Vector2>();
        move = new Vector2(movement.x, 1);
        MoveInput(move);
    }


    public void Stop() {
        //動きを止める
        move = Vector2.zero;
        MoveInput(move);
        useStop = true;
    }


    public void Tumble() {
        //動きを止めて、転倒用のアニメーションを再生する
        if (useTumble) {
            Stop();
            //todo:現在は仮でtransformで回転させるだけ
            transform.Rotate(new Vector3(90, 0, 0));
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
    }


    /// <summary>
    /// スライディングの実装
    /// </summary>
    public void Sliding() {

    }

    /// <summary>
    /// Oキーを押したときに呼ばれる(機能テスト用)
    /// </summary>
    private void test() {
        //Tumble = useTumble ? false : true;
        useTumble = useTumble ? false : true;
    }
    
    
    
    

}
