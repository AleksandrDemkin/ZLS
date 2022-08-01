using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine.UI;
using CatalogItem = PlayFab.ClientModels.CatalogItem;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private TMP_Text _sliderText;
    [SerializeField] private Canvas _accountCanvas;
    [SerializeField] private Canvas _playFabLoginCanvas;
    [SerializeField] private Slider _slider;

    private const string AuthGuidKey = "auth_guid";
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

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            success =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(success); 
                
            }, OnLoginError);
    }
    
    private void OnLoginSuccess(LoginResult result)
    {
        _loginButton.interactable = false;
        _resultText.color = Color.green;
        _resultText.text = _result + _success;
        Debug.Log("Done, huh!");

        SetUserData(result.PlayFabId);
        BuyItem();
        GetInventory();
    }
    
    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Ups, fail: {errorMessage}");
        _loginButton.interactable = true;
        _resultText.color = Color.red;
        _resultText.text = _result + _fail + errorMessage;
    }

    #region SetUserData
    private void SetUserData(string playfabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"daily_reward_time", DateTime.UtcNow.ToString()}
            }
        }, result =>
        {
            Debug.Log("UpdateUserData");
                GetUserData(playfabId, "daily_reward_time");
        }, OnLoginError);

    }
    #endregion
    
    #region GetUserData
    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        
        },result =>
        {
            Debug.Log($"{keyData} : {result.Data[keyData].Value}");
        }, OnLoginError);
    }
    #endregion

    #region BuyItem
    private void BuyItem()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "MainCatalog",
            ItemId =  "bandage_id",
            Price = 100,
            VirtualCurrency = "GD"
        },result =>
        {
            Debug.Log("PurchaseItem");
        }, OnLoginError);
    }
    #endregion

    #region GetInventory
    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest
        {
            
        },result =>
        {
            Debug.Log("GetUserInventory");
            ShowInventory(result.Inventory);
        }, OnLoginError);
    }

    private void ShowInventory(List<ItemInstance> items)
    {
        var firstItem = items.First();
        Debug.Log($"ShowInventory: {firstItem.ItemId}");
        ConsumeBandage(firstItem.ItemInstanceId);
    }

    private void ConsumeBandage(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        },result =>
        {
            Debug.Log("ConsumeItem");
            
        }, OnLoginError);
    }

    #endregion
    
    
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
        StopAllCoroutines();
    }
}
