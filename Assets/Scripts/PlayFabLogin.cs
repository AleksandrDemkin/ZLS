using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private TMP_Text _sliderText;
    [SerializeField] private Canvas _accountCanvas;
    [SerializeField] private Canvas _playFabLoginCanvas;
    [SerializeField] private Slider _slider;

    private const string AuthGudKey = "auth_guid";
    private string _titleId = "18AE5";

    private string _result = "Log in result: ";
    private string _success = "Done, huh!";
    private string _fail = "Ups, fail!";
    private string _percent = "%";
    private float _curValue;
    private float _maxValue;

    private void Start()
    {
        _curValue = _slider.value;
        _maxValue = _slider.maxValue;
        
        DisableAccountCanvas();
        
        _loginButton.onClick.AddListener(OnPlayFabClientAPI);
        _closeButton.onClick.AddListener(DisablePlayFabLoginCanvas);

        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        StartCoroutine(SliderValueChange());
    }

    private void OnPlayFabClientAPI()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = _titleId;
        }

        var needCreation = PlayerPrefs.HasKey(AuthGudKey);
        var id = PlayerPrefs.GetString(AuthGudKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            success =>
            {
                PlayerPrefs.SetString(AuthGudKey, id);
                OnLoginSuccess(success); 
                
            }, OnLoginError);
    }
    
    private void OnLoginSuccess(LoginResult result)
    {
        _loginButton.interactable = false;
        _resultText.color = Color.green;
        _resultText.text = _result + _success;
        Debug.Log("Done, huh!");
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Ups, fail: {errorMessage}");
        _loginButton.interactable = true;
        _resultText.color = Color.red;
        _resultText.text = _result + _fail + errorMessage;
    }

    private void DisablePlayFabLoginCanvas()
    {
        _playFabLoginCanvas.gameObject.SetActive(false);
        EnableAccountCanvas();
    }

    private void EnableAccountCanvas()
    {
        _accountCanvas.gameObject.SetActive(true);
    }

    private void DisableAccountCanvas()
    {
        _accountCanvas.gameObject.SetActive(false);
    }

    private IEnumerator SliderValueChange()
    {
        while (_curValue < _maxValue)
        {
            ++_curValue;
            SliderValueDisplay(_curValue);
            yield return _curValue;
        }
    }

    private void SliderValueDisplay(float _curValue)
    {
        _sliderText.text = _curValue.ToString(CultureInfo.InvariantCulture) + _percent;
    }

    private void OnDestroy()
    {
        _loginButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();
    }
}
