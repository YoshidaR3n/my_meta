
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using System.Threading;

public class FPSController : MonoBehaviourPunCallbacks
{
    float x, z;
    float speed = 0.2f;
    float rotateSpeed = 10f;
    float jumpForce = 10f;

    Transform cameraTransform;

    Vector3 velocity;
    Vector3 pre;

    bool isJump, isJumpWait; //ジャンプフラグの設定。Unityちゃんが飛んでいるか否か。
    Animator anim; //Unityちゃんのジャンプ設定するためのAnimator
    float jumpWaitTimer; //ジャンプ待機時間

    CameraController cameraController;
    // bool cursorLock = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pre = transform.position;
        cameraTransform = Camera.main.transform;

        cameraController = this.GetComponent<CameraController>();

        if (photonView.IsMine) {
            cameraController.OnStartFollowing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown("space")) //スペースキーが押下されたらジャンプする
            {
               anim.Play("JUMPING");//Unityちゃんのジャンプ処理
               GetComponent<Rigidbody>().velocity = transform.up * jumpForce;
            }
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            x = 0;
            z = 0;

            x = Input.GetAxisRaw("Horizontal") * speed;
            z = Input.GetAxisRaw("Vertical") * speed;

            velocity = new Vector3(x, 0, z).normalized;
            rotateDir();

            if (velocity.magnitude > 0) {
                anim.SetBool("RUNNING", true);
            } else {
                anim.SetBool("RUNNING", false);
            }
            transform.position += cameraTransform.forward * z + cameraTransform.right * x;
            Debug.Log(cameraTransform.forward);
        }
    }

    public void rotateDir()
    {
        var delta = transform.position - pre;
        delta.y = 0;
        pre = transform.position;

        if(delta.magnitude > 0.01f){
        transform.forward = Vector3.Slerp(transform.forward, delta, Time.deltaTime * rotateSpeed);
        //transform.rotation = Quaternion.LookRotation(delta); //向きを変更する
        }
        
    }

}