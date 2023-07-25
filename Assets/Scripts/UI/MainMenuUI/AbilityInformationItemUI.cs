using RPG.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menu
{
    public class AbilityInformationItemUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;

        public void Setup(Ability ability)
        {
            icon.sprite = ability.GetIcon();
            title.text = ability.GetLocalizedName();
            description.text = ability.GetLocalizedDescription();
        }
    }
}