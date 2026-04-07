using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SivaPhotonController : MonoBehaviourPunCallbacks
{
    GameObject panels;
    Button[] roomButtons;
    Text[] playerNameTexts;
    InputField newRoomName;
    InputField newRoomSize;
    InputField joinedRoomName;
    InputField joinedRoomSize;
    InputField joinedRoomPlayerCount;

    HashSet<RoomInfo> allRooms; //set of all rooms in the lobby

    void Start()
    {
        panels = transform.Find("Panels").gameObject;
        newRoomName = transform.Find("Panels/ConnectPanel/NewRoomName").GetComponent<InputField>();
        newRoomSize = transform.Find("Panels/ConnectPanel/NewRoomSize").GetComponent<InputField>();
        joinedRoomName = transform.Find("Panels/DisconnectPanel/JoinedRoomName").GetComponent<InputField>();
        joinedRoomSize = transform.Find("Panels/DisconnectPanel/JoinedRoomSize").GetComponent<InputField>();
        joinedRoomPlayerCount = transform.Find("Panels/DisconnectPanel/JoinedRoomPlayerCount").GetComponent<InputField>();
        roomButtons = transform.Find("Panels/ConnectPanel/RoomsText/RoomButtons").GetComponentsInChildren<Button>();
        playerNameTexts = transform.Find("Panels/DisconnectPanel/PlayerList").GetComponentsInChildren<Text>();
        PhotonNetwork.ConnectUsingSettings();
        showPanel("WaitPanel");
    }

    void Update()
    {
        //Update the Disconnect Panel with info on current room
        if (PhotonNetwork.InRoom && panels.activeSelf) 
        {
            joinedRoomName.text = PhotonNetwork.CurrentRoom.Name;
            joinedRoomSize.text = "" + PhotonNetwork.CurrentRoom.MaxPlayers;
            joinedRoomPlayerCount.text = "" + PhotonNetwork.CurrentRoom.PlayerCount;

            int i = 0;
            foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
            {
                
                if (i < playerNameTexts.Length)
                {
                    playerNameTexts[i].text = p.NickName;
                    if (p == PhotonNetwork.LocalPlayer)
                    {
                        playerNameTexts[i].text = "*" + playerNameTexts[i].text + "*";
                    }
                    i++;
                }
            }
            while (i < playerNameTexts.Length)
            {
                playerNameTexts[i].text = "";
                i++;
            }
        }
    }

    /**************************************************************************
     *                     BUTTON PROCESSING                                  *
     **************************************************************************/                     
    public void show()
    {
        panels.SetActive(!panels.activeSelf);
    }

    public void setPlayerName(InputField f)
    {
        PhotonNetwork.NickName = f.text;
    }

    public void createRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse(newRoomSize.text);
        PhotonNetwork.CreateRoom(newRoomName.text, options);
        showPanel("WaitPanel");
    }

    public void joinRoom(Text t)
    {
        PhotonNetwork.JoinRoom(t.text);
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        showPanel("WaitPanel");
    }

    void showPanel(string name)
    {
        for (int i = 0; i < panels.transform.childCount; i++)
        {
            GameObject panel = panels.transform.GetChild(i).gameObject;
            panel.SetActive(panel.name == name);
        }
    }


    /**************************************************************************
     *                     PHOTON CALLBACKS                                   *
     **************************************************************************/
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        showPanel("WaitPanel");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //print("Room Updates:" + string.Join(",", roomList));
        //NOTE this roomList is just the delta on total rooms.
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
                allRooms.Remove(info);
            else
                allRooms.Add(info);
        }

        //set all the buttons active or deactive
        int i = 0;
        foreach (RoomInfo info in allRooms)
        {
            if (i < roomButtons.Length)
            {
                roomButtons[i].GetComponentInChildren<Text>().text = info.Name;
                roomButtons[i].gameObject.SetActive(true);
                i++;
            }
        }
        while (i < roomButtons.Length)
        {
            roomButtons[i].gameObject.SetActive(false);
            i++;
        }
    }

    public override void OnJoinedLobby()
    {
        //hide all room buttons
        allRooms = new HashSet<RoomInfo>();
        panels.SetActive(true);
        showPanel("ConnectPanel");
    }

    public override void OnJoinedRoom()
    {
        showPanel("DisconnectPanel");
        panels.SetActive(false);
        foreach(Player p in PhotonNetwork.CurrentRoom.Players.Values) {
            if (p.NickName == "") {
                p.NickName = "Player " + p.ActorNumber;
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print(message);
    }
}
