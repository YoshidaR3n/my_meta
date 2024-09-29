using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class JoinRoom:MonoBehaviourPunCallbacks
{
    float randX, randZ;

    private void Start()
    {
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
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

        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }
}