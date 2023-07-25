using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerManaBar : MonoBehaviour
    {
        Slider playerManaSlider;
        Mana mana;

        private void Start()
        {
            playerManaSlider = GetComponent<Slider>();
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }

        private void Update()
        {
            playerManaSlider.maxValue = mana.GetMaxMana();
            playerManaSlider.value = mana.GetMana();
        }
    }
}