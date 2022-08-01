using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

namespace DefaultNamespace
{
    public class SignInWindow : AccountDataWindowBase
    {
        [SerializeField] private Button _signInButton;

        protected override void SubscriptionsElementsUi()
        {
            base.SubscriptionsElementsUi();
            
            _signInButton.onClick.AddListener(SignIn);
        }

        private void SignIn()
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = _username,
                Password = _password
            }, result =>
            {
                Debug.Log($"Done, {_username}");
                EnterInGameScene();
            }, error =>
            {
                Debug.LogError($"Failed: {error.ErrorMessage}");
            });
        }
        
        private void OnDestroy()
        {
            _signInButton.onClick.RemoveAllListeners();
        }
    }
}