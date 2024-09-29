using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;

    public GameObject loadingPanel;
    public GameObject buttons;
    public Text loadingText;

    public GameObject createRoomPanel;
    public Text enterRoomName;

    public GameObject roomPanel;
    public Text roomName;

    public GameObject errorPanel;
    public Text errorText;

    public GameObject roomListPanel;

    public Room originalRoomButton;
    public GameObject roomButtonContent;
    Dictionary<string,RoomInfo> roomsList = new Dictionary<string,RoomInfo>();

    private List<Room> allRoomButtons = new List<Room>();

    public Text playerNameText;
    private List<Text> allPlayerNames = new List<Text>();
    public GameObject playerNameContent;

    public GameObject startButton;

    public string levelToPlay;

    private void Awake(){
        instance = this;
    }

    //start
    private void Start(){
        CloseMenuUI();
        loadingPanel.SetActive(true);
        loadingText.text = "Connecting network...";

    //connect netowork
        if(!PhotonNetwork.IsConnected){
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void CloseMenuUI(){
        loadingPanel.SetActive(false);
        buttons.SetActive(false);
        createRoomPanel.SetActive(false);
        roomPanel.SetActive(false);
        errorPanel.SetActive(false);
        roomListPanel.SetActive(false);
    }

    public void LobbyMenuDisplay(){
        CloseMenuUI();
        buttons.SetActive(true);
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
        loadingText.text = "Joining lobby...";

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby(){
        roomsList.Clear();
        LobbyMenuDisplay();

        PhotonNetwork.NickName = Random.Range(0,100).ToString();
    }

//ルーム作成ボタン用
    public void OpenCreateRoomPanel(){
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    }

    //ルーム作成用
    public void CreateRoomButton(){
        if(!string.IsNullOrEmpty(enterRoomName.text)){
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 6;
            //ルーム作成
            PhotonNetwork.CreateRoom(enterRoomName.text, options);

            CloseMenuUI();
            loadingText.text = "creating room....";
            loadingPanel.SetActive(true);

        }
    }

    //ルーム作成時コールバック
    public override void OnJoinedRoom(){
        CloseMenuUI();
        roomPanel.SetActive(true);

        roomName.text = PhotonNetwork.CurrentRoom.Name;

        GetAllPlayer();

        CheckRoomMaster();
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        CloseMenuUI();
        loadingText.text = "leaving...";
        loadingPanel.SetActive(true);
    }

    public override void OnLeftRoom(){
        LobbyMenuDisplay();
    }

    public override void OnCreateRoomFailed(short returnCode, string message){
        CloseMenuUI();
        errorText.text = "failed create room" + message;
        errorPanel.SetActive(true);
    }

    public void FindRoom(){
        CloseMenuUI();
        roomListPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        RoomUIInitialization();
        UpdateRoomList(roomList);
    }

    public void UpdateRoomList(List<RoomInfo> roomList){
        for(int i=0; i<roomList.Count; i++){
            RoomInfo info = roomList[i];

            if(info.RemovedFromList){
                    roomsList.Remove(info.Name);
            }
            else{
                    roomsList[info.Name] = info;
            }
        }
        RoomListDisplay(roomsList);
     
    }

    public void RoomListDisplay(Dictionary<string,RoomInfo> cachedRoomList){
        foreach(var roomInfo in cachedRoomList){
            Room newButton = Instantiate(originalRoomButton);
            newButton.RegisterRoomDetails(roomInfo.Value);
            newButton.transform.SetParent(roomButtonContent.transform);

            allRoomButtons.Add(newButton);
        }
    }

    public void RoomUIInitialization(){
        foreach(Room rm in allRoomButtons){
            Destroy(rm.gameObject);
        }
        allRoomButtons.Clear();
    }

    public void JoinRoom(RoomInfo roominfo){
        PhotonNetwork.JoinRoom(roominfo.Name);
        CloseMenuUI();
        loadingText.text="joining room";
        loadingPanel.SetActive(true);
    }

    public void GetAllPlayer(){
        InitializePlayerList();
        PlayerDisplay();
    }

    public void InitializePlayerList(){
        foreach(var rm in allPlayerNames){
            Destroy(rm.gameObject);
        }
        allPlayerNames.Clear();
    }

    public void PlayerDisplay(){
        foreach(var players in PhotonNetwork.PlayerList){
            PlayerTextGeneration(players);
        }
    }

    public void PlayerTextGeneration(Player players){
        Text newPlayerText = Instantiate(playerNameText);
        newPlayerText.text = players.NickName;

        newPlayerText.transform.SetParent(playerNameContent.transform);

        allPlayerNames.Add(newPlayerText);
    }

    public void CheckRoomMaster(){
        if(PhotonNetwork.IsMasterClient){
            startButton.SetActive(true);
        }
        else{
            startButton.SetActive(false);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient){
        if(PhotonNetwork.IsMasterClient){
            startButton.SetActive(true);
        }
    }

    //遷移
    public void PlayGame(){
        PhotonNetwork.LoadLevel(levelToPlay);
    }
}