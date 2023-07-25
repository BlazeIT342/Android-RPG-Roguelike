using RPG.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Abilities
{
    public class AbilityItemUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Image cooldownOverlay;
        Ability ability;
        CooldownStore cooldownStore;

        private void Start()
        {
            cooldownStore = GameObject.FindGameObjectWithTag("Player").GetComponent<CooldownStore>();
        }

        private void Update()
        {
            if (ability != null)
            {
                cooldownOverlay.fillAmount = cooldownStore.GetFractionRemaining(ability);
            }
        }

        public void Setup(Ability ability)
        {
            this.ability = ability;
            icon.sprite = ability.GetIcon();
        }
    }
}