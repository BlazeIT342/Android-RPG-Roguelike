using RPG.Perks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Perks
{
    public class PerkItemUI : MonoBehaviour
    {
        [SerializeField] Image icon;

        public void Setup(Perk perk)
        {
            icon.sprite = perk.GetIcon();
        }
    }
}