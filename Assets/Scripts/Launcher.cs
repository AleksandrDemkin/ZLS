using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _roomName = "RoomName";
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private TMP_Text _resultText;
    
    private string _result = "Log in result: ";
    private string _loginRoom = "Log in the room!";
    private string _loginLobby = "Log in the lobby!";
    private string _logoutServer = "Log out! PhotonNetwork is disconnected!";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //_logoutButton.gameObject.SetActive(false);
        _logoutButton.interactable = false;
        _resultText.text = _result;
        _resultText.color = Color.green;
    }

    private void Start()
    {
        _loginButton.onClick.AddListener(Connect);
        _logoutButton.onClick.AddListener(Disconnect);
    }

    private void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
            //PhotonNetwork.GameVersion = _gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        _loginButton.interactable = false;
        _logoutButton.interactable = true;
        //_resultText.gameObject.SetActive(true);
        //_loginButton.gameObject.SetActive(false);
        //_logoutButton.gameObject.SetActive(true);
        Debug.Log("OnConnectedToMaster() was called by PUN");
        _resultText.text = _result + _loginLobby;
        _resultText.color = Color.green;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"OnJoinedLobby: {PhotonNetwork.InLobby}");
        //_resultText.text = _result + _loginLobby;
        PhotonNetwork.JoinOrCreateRoom(_roomName,
            new RoomOptions {MaxPlayers = 2, IsVisible = true}, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        //_resultText.text = _result + _loginRoom;
        Debug.Log($"OnJoinedRoom: {PhotonNetwork.InRoom}");
    }

    private void Disconnect()
    {
        //_resultText.gameObject.SetActive(true);
        //_loginButton.gameObject.SetActive(true);
        //_logoutButton.gameObject.SetActive(false);
        _loginButton.interactable = true;
        _logoutButton.interactable = false;
        PhotonNetwork.Disconnect();
        _resultText.text = _logoutServer;
        _resultText.color = Color.red;
        Debug.Log(_logoutServer);
    }
    
    private void OnDestroy()
    {
        _loginButton.onClick.RemoveAllListeners();
        _logoutButton.onClick.RemoveAllListeners();
    }
}
