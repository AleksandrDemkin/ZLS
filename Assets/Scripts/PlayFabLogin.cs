using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{ 
    [SerializeField] private string _customId = "Player1";
    [SerializeField] private Button _loginButton;
    [SerializeField] private TMP_Text _resultText;
    
    private string _titleId = "18AE5";
    
    private string _result = "Log in result: ";
    private string _success = "Done, huh!";
    private string _fail = "Ups, fail!";
    
    private void Start()
    {
        _loginButton.gameObject.SetActive(true);
        _resultText.gameObject.SetActive(false);
        
        /*if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = _titleId;
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = _customId,
            CreateAccount = true
        };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);*/
        
        _loginButton.onClick.AddListener(OnPlayFabClientAPI);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _loginButton.gameObject.SetActive(false);
        _resultText.gameObject.SetActive(true);
        _resultText.color = Color.green;
        _resultText.text = _result + _success;
        Debug.Log("Done, huh!");
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Ups, fail: {errorMessage}");
        _loginButton.gameObject.SetActive(false);
        _resultText.gameObject.SetActive(true);
        _resultText.color = Color.red;
        _resultText.text = _result + _fail + errorMessage;
    }
    
    private void OnPlayFabClientAPI()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = _titleId;
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = _customId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }
    
    private void OnDestroy()
    {
        _loginButton.onClick.RemoveAllListeners();
    }
}
