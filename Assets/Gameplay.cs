using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public Text playerName;
    // Start is called before the first frame update
    void Start()
    {
        playerName.text = PhotonManager.Instance.name;

        PhotonManager.Instance.DisplayNames();
    }

 }
