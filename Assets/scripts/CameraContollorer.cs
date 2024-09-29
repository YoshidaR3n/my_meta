using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 currentPos;//現在のカメラ位置
    Vector3 pastPos;//過去のカメラ位置

    Vector3 diff;//移動距離

    Vector3 pos; //カメラとプレイヤーの距離
    float playerAngle,camAngle,angle;//カメラとプレイヤーのなす角

    private float zoom;//ホイールがどれくらい回転しているか
    Vector3 offset =  Vector3.zero;
    Vector3 cameraOffset = Vector3.zero;
    Vector3 centerOffset = Vector3.zero;

    [SerializeField]
    private bool followOnStart = false;
    Transform cameraTransform;
    bool isFollowing;

    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("The height we want the camera to be above the target")]
    [SerializeField]
    private float height = 3.0f;

    private void Start()
    {
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }
    private void LateUpdate()
    {
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }
        if (isFollowing)
        {
            Follow();
            Debug.Log("aaaaaaaa");
        }
    }
    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        //最初のプレイヤーの位置の取得
        pastPos = this.transform.position;
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

        cameraTransform.LookAt(this.transform.position + centerOffset);

        isFollowing = true;
    }

    public void Follow()
    {
        cameraTransform = Camera.main.transform;

        //------カメラの移動------

        //プレイヤーの現在地の取得
        currentPos = this.transform.position;

        diff = currentPos - pastPos;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + diff, 1.0f);//カメラをプレイヤーの移動差分だけうごかすよ

        pastPos = currentPos;


        //------カメラの回転------
        if (!Input.GetMouseButton(0))
        {
            // マウスの移動量を取得
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            pos = this.transform.position - cameraTransform.position;

            // X方向に一定量移動していれば横回転
            if (Mathf.Abs(mx) > 0.01f)
            {
                // 回転軸はワールド座標のY軸
                cameraTransform.RotateAround(this.transform.position, Vector3.up, mx);
            }

            // Y方向に一定量移動していれば縦回転
            if (Mathf.Abs(my) > 0.01f)
            {
                // 回転軸はカメラ自身のX軸
                cameraTransform.RotateAround(this.transform.position, cameraTransform.right, -my);
            }
        }
        else
        {
            //左クリックしてるとき、カメラの位置を調整する機能
            
            float my = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(my) > 0.01f)
            {
                var newmy = (float)(my * -0.3);
                cameraTransform.position = cameraTransform.position + new Vector3(0f, newmy, 0f);
            }
        }


            /*
            if (Input.GetMouseButtonDown(1))
            {
                playerAngle = new Vector3(this.transform.x, 0, this.transform.z);
                angle = Vector3.Angle(this.transform.forward, cameraTransform.forward);
                cameraTransform.RotateAround(this.transform.position, Vector3.up, angle);
                //cameraTransform.LookAt(this.transform.position);
            }
            */

            zoom = Input.GetAxis("Mouse ScrollWheel");

        if (zoom > 0)
        {
            offset = pos.normalized * 0.1f;
        }
        else if (zoom < 0)
        {
            offset = -pos.normalized * 0.1f;
        }

        cameraTransform.position = cameraTransform.position + offset;
        offset = pos.normalized * 0.0f;
    }
}