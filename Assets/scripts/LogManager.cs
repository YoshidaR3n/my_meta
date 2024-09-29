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

    // ���v���C���[�����[������ޏo�������ɌĂ΂��R�[���o�b�N
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        logText.text = otherPlayer.NickName + " leaved.";
    }
}
