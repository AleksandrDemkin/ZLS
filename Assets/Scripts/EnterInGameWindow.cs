using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _createAccountButton;
    [SerializeField] private Button _restoreAccountButton;
    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _signInCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private Canvas _restoreAccountCanvas;

    private void Start()
    {
        _signInButton.onClick.AddListener(OpenSignInWindow);
        _createAccountButton.onClick.AddListener(OpenCreateAccountWindow);
        _restoreAccountButton.onClick.AddListener(RestoreAccountWindow);
    }

    private void OpenSignInWindow()
    {
        _enterInGameCanvas.enabled = false;
        _signInCanvas.enabled = true;
        _restoreAccountCanvas.enabled = false;
    }

    private void OpenCreateAccountWindow()
    {
        _enterInGameCanvas.enabled = false;
        _createAccountCanvas.enabled = true;
        _restoreAccountCanvas.enabled = false;
    }
    
    private void RestoreAccountWindow()
    {
        _enterInGameCanvas.enabled = false;
        _createAccountCanvas.enabled = false;
        _restoreAccountCanvas.enabled = true;
    }
    
    private void OnDestroy()
    {
        _signInButton.onClick.RemoveAllListeners();
        _createAccountButton.onClick.RemoveAllListeners();
        _restoreAccountButton.onClick.RemoveAllListeners();
    }
}
