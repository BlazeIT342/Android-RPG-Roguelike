using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;

        private void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
        }
    }
}