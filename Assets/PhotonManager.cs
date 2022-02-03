using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public Text roomName;
    public Text playerName;
    public GameObject beforeConnection;
    public GameObject afterConnection;
    public string name;
    public static PhotonManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
      DontDestroyOnLoad(this); 
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = playerName.text;
        name = PhotonNetwork.NickName;
    }



    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Photon server Connected");
        beforeConnection.SetActive(false);
        afterConnection.SetActive(true);
    }


    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {

        };

        PhotonNetwork.CreateRoom(roomName.text);

    }

    public void JoinRom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        SceneManager.LoadScene("Room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room joined");

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PhotonNetwork.JoinRoom(roomName.text);
        Debug.Log("Room Created");
        SceneManager.LoadScene("Room");
    }

    public void DisplayNames()
    {
       foreach(var i in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(i.Value);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Joining Failed");
    }
}
