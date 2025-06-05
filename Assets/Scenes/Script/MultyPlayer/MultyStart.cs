using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MultyStart : NetworkManager
{
    private InputField nameInput;
    private string displayName;

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
    }
    public struct CreateCharacterMessage : NetworkMessage
    {
        public string playerID;
    }
    public override void OnClientConnect()
    {
        nameInput = GameObject.Find("NameInput").GetComponent<InputField>();
        base.OnClientConnect();
        if(nameInput.text.Length > 0){
            displayName = nameInput.text;
        } else {
            displayName = "无名墩子";
        }
        CreateCharacterMessage characterMessage = new CreateCharacterMessage
        {
            playerID = displayName
        };
        NetworkClient.Send(characterMessage);
    }
    void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
    {
        GameObject gameobject = Instantiate(playerPrefab); // 实例化玩家预制体
        gameobject.GetComponent<GhostDunzi>().playerID = message.playerID;
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }
}
