using RPG.Abilities;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Abilities
{
    public class AbilityMenuUI : MonoBehaviour
    {
        [SerializeField] Transform transformRoot;
        [SerializeField] AbilityItemUI abilityItemUI;

        AbilityList abilityList;
        GameObject player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            abilityList = player.GetComponent<AbilityList>();
            BuildAbilityList();
        }

        private void BuildAbilityList()
        {
            foreach (Transform item in transformRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (var ability in player.GetComponent<BaseStats>().GetCharacterClassStats().GetAbilities(player.GetComponent<BaseStats>().GetCharacterClass()))
            {
                if (ability == null) continue;
                AbilityItemUI uiInstance = Instantiate(abilityItemUI, transformRoot);
                uiInstance.Setup(ability);
                uiInstance.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ability.Use(player);
                });
            }
        }
    }
}