using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

namespace DefaultNamespace
{
    public class RestoreAccount  : AccountDataWindowBase
    {
        [SerializeField] private InputField _emailField;
        [SerializeField] private Button _restoreButton;
        [SerializeField] private TMP_Text _resultText;
        
        private string _email;
        private string _seccess = "Done, restore account request has been sent to the ";
        private string _failed = "Failed: ";
        protected override void SubscriptionsElementsUi()
        {
            _emailField.onValueChanged.AddListener(UpdateEmail);
            _restoreButton.onClick.AddListener(Restore);
        }
        
        private void UpdateEmail(string email)
        {
            _email = email;
        }

        private void Restore()
        {
            PlayFabClientAPI.SendAccountRecoveryEmail(new SendAccountRecoveryEmailRequest
            {
                Email = _email
            },result =>
            {
                _resultText.text = _seccess + _email;
                Debug.Log($"Done, restore account request has been sent to the {_email}");
            }, error =>
            {
                _resultText.text = _failed + error.ErrorMessage;
                Debug.LogError($"Failed: {error.ErrorMessage}");
            });
        }
        
        private void OnDestroy()
        {
            _emailField.onValueChanged.RemoveAllListeners();
            _restoreButton.onClick.RemoveAllListeners();
        }
    }
}