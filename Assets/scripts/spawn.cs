using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviourPunCallbacks
{
    float randX, randZ;

    void Start() {
        randX = Random.Range(0, 100);
        randZ = Random.Range(-100, 0);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("unitychan", new Vector3(randX, 10.0f, randZ), Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate("unitychan", new Vector3(randX, 10.0f, randZ), Quaternion.identity, 0);
        }
    }
}
