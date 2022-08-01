using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Button _closeButton;
        [SerializeField] private GameObject _newCreateCharacterPanel;
        [SerializeField] private Button _createCharacterButton;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private List<SlotCharacterWidget> _slots;
        
        private const string Health = "Health";
        private const string Damage = "Damage";
        private const string Level = "Level";
        private const string Experience = "Experience";
        private const string Gold = "Gold";

        private string _characterName;

        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string,
            CatalogItem>();

        private void Start()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
            
            GetCharacters();
            _closeButton.onClick.AddListener(DisableAccountInfoCanvas);

            foreach (var slot in _slots)
            {
                slot.Button.onClick.AddListener(OpenCreateNewCharacter);
            }
            
            _createCharacterButton.onClick.AddListener(CreateCharacterWithItem);
            _inputField.onValueChanged.AddListener(OnNameChanged);
        }

        private void CreateCharacterWithItem()
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName = _characterName,
                ItemId = "character_item"
            }, result =>
            {
                UpdateCharacterStatistics(result.CharacterId);
            }, OnError);
        }

        private void UpdateCharacterStatistics(string characterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    {Health, 100},
                    {Damage, 20},
                    {Level, 1},
                    {Experience, 0},
                    {Gold, 0}
                }
            }, result =>
            {
                Debug.Log("UpdateCharacterStatistics done");
                CloseCreateNewCharacter();
                GetCharacters();
            },OnError);
        }

        private void GetCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    Debug.Log($"Characters count: {result.Characters.Count}");
                    ShowCharactersInSlots(result.Characters);
                }, OnError);
        }

        private void ShowCharactersInSlots(List<CharacterResult> characters)
        {
            if (characters.Count == 0)
            {
                foreach (var slot in _slots)
                {
                    slot.ShowEmptySlot();
                }
            }
            else if (characters.Count > 0 && characters.Count <= _slots.Count)
            {
                PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = characters.First().CharacterId
                }, result =>
                {
                    var health = result.CharacterStatistics[Health].ToString();
                    var damage = result.CharacterStatistics[Damage].ToString();
                    var level = result.CharacterStatistics[Level].ToString();
                    var experience = result.CharacterStatistics[Experience].ToString();
                    var gold = result.CharacterStatistics[Gold].ToString();
                    
                    _slots.First().ShowInfoCharacterSlot(characters.First().CharacterName, health,damage, level, 
                        experience, gold);
                }, OnError);
            }
            else
            {
                Debug.LogError("Add character slots.");
            }
        }

        private void OpenCreateNewCharacter()
        {
            _newCreateCharacterPanel.SetActive(true);
        }
        
        private void CloseCreateNewCharacter()
        {
            _newCreateCharacterPanel.SetActive(false);
        }

        private void OnNameChanged(string changedName)
        {
            _characterName = changedName;
        }

        private void OnGetAccount(GetAccountInfoResult result)
        {
            _titleAccount.text = $"User {result.AccountInfo.PlayFabId} has logged in)";
        }
        
        private void OnGetCatalog(GetCatalogItemsResult result)
        {
            ShowCatalog(result.Catalog);
            Debug.Log("OnGetCatalog completed");
        }

        private void ShowCatalog(List<CatalogItem> items)
        {
            foreach (var item in items)
            {
                _catalog.Add(item.ItemId, item);
                Debug.Log($"Items: {item.ItemId}");
                Debug.Log($"Items: {item.Description}");
            }
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