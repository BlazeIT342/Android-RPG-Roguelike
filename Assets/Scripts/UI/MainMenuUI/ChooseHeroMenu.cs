using RPG.Core;
using RPG.Inventories;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menu
{
    public class ChooseHeroMenu : MonoBehaviour
    {
        [SerializeField] CharactersData charactersData;
        [SerializeField] TextMeshProUGUI titleName;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] Image icon;
        [SerializeField] Transform charactersRoot;
        [SerializeField] CharacterItemUI characterItemUI;
        [SerializeField] Transform abilitiesRoot;
        [SerializeField] AbilityInformationItemUI abilityInformationItemUI;
        [SerializeField] ConfirmButton confirmButton;

        CharactersData.Character choosedCharacter;

        private void OnEnable()
        {
            BuildCharactersList();
            UpdateUI();
        }

        private void BuildCharactersList()
        {
            foreach (Transform item in charactersRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (var character in charactersData.GetCharacters())
            {
                if (character == null) continue;
                if (!character.characterInformation.isCharacter) continue;
                CharacterItemUI uiInstance = Instantiate(characterItemUI, charactersRoot);
                uiInstance.Setup(character, charactersData);
                uiInstance.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    choosedCharacter = character;
                    FindObjectOfType<PlayerSpawner>().SetCharacter(choosedCharacter.characterClass);
                    UpdateUI();
                });
                if (choosedCharacter == null)
                {
                    choosedCharacter = character;
                    FindObjectOfType<PlayerSpawner>().SetCharacter(choosedCharacter.characterClass);
                }
            }
        }

        private void BuildAbilitiesList()
        {
            foreach (Transform item in abilitiesRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (var ability in choosedCharacter.abilities)
            {
                if (ability == null) continue;
                AbilityInformationItemUI uiInstance = Instantiate(abilityInformationItemUI, abilitiesRoot);
                uiInstance.Setup(ability);
            }
        }

        private void UpdateUI()
        {
            if (choosedCharacter == null) return;
            ConfirmButton();
            BuildAbilitiesList();
            titleName.text = charactersData.GetLocalizedName(choosedCharacter.characterClass);
            description.text = charactersData.GetLocalizedDescription(choosedCharacter.characterClass);
            icon.sprite = choosedCharacter.characterInformation.icon;
        }

        public void ConfirmButton()
        {
           
            if (!choosedCharacter.characterInformation.bought)
            {
                confirmButton.BuyCharacter();
                confirmButton.GetBuyCharacter().onClick.AddListener(() =>
                {
                    Purse playerPurse = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Purse>();
                    if (playerPurse.GetBalance() > choosedCharacter.characterInformation.price)
                    {
                        playerPurse.UpdateBalance(-choosedCharacter.characterInformation.price);
                        charactersData.BuyCharacter(choosedCharacter);
                    }
                    else Debug.Log("Пепс");
                    BuildCharactersList();
                    UpdateUI();
                });
            }
            else
            {
                confirmButton.StartGame();
            }
        }
    }
}