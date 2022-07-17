using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AccountDataWindowBase : MonoBehaviour
    {
        [SerializeField] private InputField _userNameField;
        [SerializeField] private InputField _passwordField;

        protected string _username;
        protected string _password;

        private void Start()
        {
            SubscriptionsElementsUi();
        }

        protected virtual void SubscriptionsElementsUi()
        {
            _userNameField.onValueChanged.AddListener(UpdateUserName);
            _passwordField.onValueChanged.AddListener(UpdatePassword);
        }

        private void UpdateUserName(string username)
        {
            _username = username;
        }
        
        private void UpdatePassword(string password)
        {
            _password = password;
        }

        protected void EnterInGameScene()
        {
            SceneManager.LoadScene(1);
        }

        private void OnDestroy()
        {
            _userNameField.onValueChanged.RemoveAllListeners();
            _passwordField.onValueChanged.RemoveAllListeners();
        }
    }
}