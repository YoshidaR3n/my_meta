using Photon.Pun;
using UnityEngine;

public class Summon : MonoBehaviourPunCallbacks
{
    //public GameObject player;
    Vector3 pos;
    PhotonMessageInfo info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonButton()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            Debug.Log("function calld.");
            PhotonNetwork.Instantiate("Ball", new Vector3(50f, 10f, -50f), Quaternion.identity, 0);
        }
    }
}
