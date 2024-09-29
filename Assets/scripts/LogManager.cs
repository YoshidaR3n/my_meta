using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviourPunCallbacks
{
    public Text logText;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        logText.text = newPlayer.NickName  + " joined.";
    }

    // 他プレイヤーがルームから退出した時に呼ばれるコールバック
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        logText.text = otherPlayer.NickName + " leaved.";
    }
}
