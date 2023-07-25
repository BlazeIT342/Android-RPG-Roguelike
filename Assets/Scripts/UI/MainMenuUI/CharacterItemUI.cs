using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menu
{
    public class CharacterItemUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI titleName;
        [SerializeField] Image abilityPrefab;
        [SerializeField] Transform abilitiesRoot;
        [SerializeField] Image locked;

        public void Setup(CharactersData.Character character, CharactersData charactersData)
        {
            foreach (Transform item in abilitiesRoot)
            {
                Destroy(item.gameObject);
            }
            icon.sprite = character.characterInformation.icon;
            titleName.text = charactersData.GetLocalizedName(character.characterClass);
            if (!character.characterInformation.bought)
            {
                locked.gameObject.SetActive(true);
            }
            foreach (var ability in character.abilities)
            {
                Image uiInstance = Instantiate(abilityPrefab, abilitiesRoot);
                uiInstance.sprite = ability.GetIcon();
            }
        }
    }
}