using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

namespace DefaultNamespace
{
    public class CreateAccountWindow: AccountDataWindowBase
    {
        [SerializeField] private InputField _emailField;
        [SerializeField] private Button _createAccountButton;

        private string _email;

        protected override void SubscriptionsElementsUi()
        {
            base.SubscriptionsElementsUi();
            
            _emailField.onValueChanged.AddListener(UpdateEmail);
            _createAccountButton.onClick.AddListener(CreateAccount);
        }

        private void UpdateEmail(string email)
        {
            _email = email;
        }

        private void CreateAccount()
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Username = _username,
                Password = _password,
                Email = _email
            }, result =>
            {
                Debug.Log($"Dome, {_username}");
                EnterInGameScene();
            }, error =>
            {
                Debug.LogError($"Failed: {error.ErrorMessage}");
            });
        }
        
        private void OnDestroy()
        {
            _emailField.onValueChanged.RemoveAllListeners();
            _createAccountButton.onClick.RemoveAllListeners();
        }
    }
}