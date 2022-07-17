using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] private Canvas _accountInfoCanvas;
        [SerializeField] private TMP_Text _titleAccount;
        [SerializeField] private TMP_Text _titleUsername;
        [SerializeField] private TMP_Text _titleCustomIdInfo;
        [SerializeField] private TMP_Text _titleCreated;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
            
            _closeButton.onClick.AddListener(DisableAccountInfoCanvas);
        }

        private void OnGetAccount(GetAccountInfoResult result)
        {
            _titleAccount.text = $"User {result.AccountInfo.PlayFabId} has logged in)";
            _titleUsername.text = $"User {result.AccountInfo.Username} has logged in)";
            _titleCustomIdInfo.text = $"User {result.AccountInfo.CustomIdInfo} has logged in)";
            _titleCreated.text = $"User {result.AccountInfo.Created} has logged in)";
        }
        
        
        private void OnError(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Error: {errorMessage}");
        }

        private void DisableAccountInfoCanvas()
        {
            _accountInfoCanvas.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}