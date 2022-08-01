using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectAndJoinRandomLobby : MonoBehaviour, 
    IConnectionCallbacks,IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private TMP_Text _stateUiText;
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _roomsAmountText;
    
    private LoadBalancingClient _loadBalancingClient;
    
    private const string AI_KEY = "ai";
    private const string PLAYERS_KEY = "rp";
    private const string FLAG_KEY = "fg";
    
    private TypedLobby _sqlLobby = new TypedLobby("sqlLobby", LobbyType.SqlLobby);

    private Dictionary<string, RoomInfo> roomList = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        _loadBalancingClient = new LoadBalancingClient();
        _loadBalancingClient.AddCallbackTarget(this);
        _loadBalancingClient.ConnectUsingSettings(_serverSettings.AppSettings);
    }

    private void Update()
    {
        if (_loadBalancingClient == null)
        {
            return;
        }
        
        _loadBalancingClient.Service();
        var state = _loadBalancingClient.State.ToString();
        _stateUiText.text = $"State: {state}, UserID: {_loadBalancingClient.UserId}";
    }
    
    private void OnDestroy()
    {
        _loadBalancingClient.RemoveCallbackTarget(this);
    }

    public void OnConnected()
    {
        
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 5,
            PlayerTtl = 10000,
            CustomRoomPropertiesForLobby = new []{ AI_KEY }, //sort room in lobby
            CustomRoomProperties = new Hashtable {{PLAYERS_KEY, 0}, {FLAG_KEY, 1}}
            //CustomRoomPropertiesForLobby = new []{ EXP_KEY,  MAP_KEY}, //sort room in lobby
            //CustomRoomProperties = new Hashtable {{EXP_KEY, _minExp}, {MAP_KEY, _mapName}}
            
        };

        var enterRoomParams = new EnterRoomParams
        {
            RoomName = "RoomFlag",
            RoomOptions = roomOptions,
            ExpectedUsers = new [] {"newplayer1", "newplayer2", "newplayer3"},
            Lobby = _sqlLobby
        };
        
        //_loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
        _loadBalancingClient.OpJoinRandomRoom();

        //_loadBalancingClient.OpFindFriends(new[] {"newplayer4"});//add to the friend with id
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }
    
    public void OnJoinRoomFailed(short returnCode, string message)
    {
        
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
        _loadBalancingClient.OpCreateRoom(new EnterRoomParams());
    }

    public void OnLeftRoom()
    {
        
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public void OnLeftLobby()
    {
        
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        
    }
}
