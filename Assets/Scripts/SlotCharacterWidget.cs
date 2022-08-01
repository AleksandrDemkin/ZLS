using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _emptySlot;
    [SerializeField] private GameObject _infoCharacterySlot;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _healthLabel;
    [SerializeField] private TMP_Text _dmageLabel;
    [SerializeField] private TMP_Text _levelLabel;
    [SerializeField] private TMP_Text _experienceLabel;
    [SerializeField] private TMP_Text _goldLabel;

    private const string Health = "Health";
    private const string Damage = "Damage";
    private const string Level = "Level";
    private const string Experience = "Experience";
    private const string Gold = "Gold";

    public Button Button => _button;

    public void ShowInfoCharacterSlot(string name, string health, string damage, string level, 
        string experience, string gold)
    {
        _nameLabel.text = name;
        _healthLabel.text = $"{Health}: {health}";
        _dmageLabel.text = $"{Damage}: {damage}";
        _levelLabel.text = $"{Level}: {level}";
        _experienceLabel.text = $"{Experience}: {experience}";
        _goldLabel.text = $"{Gold}: {gold}";
        
        _infoCharacterySlot.SetActive(true);
        _emptySlot.SetActive(false);
    }
    
    public void ShowEmptySlot()
    {
        _infoCharacterySlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}
