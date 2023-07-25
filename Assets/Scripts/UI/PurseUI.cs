using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;
        [SerializeField] Purse playerPurse = null;

        private void Start()
        {
            if (playerPurse != null)
            {
                playerPurse.onChange += RefreshUI;
            }
            RefreshUI();
        }

        private void RefreshUI()
        {
            balanceField.text = $"{playerPurse.GetBalance()}";
        }
    }
}