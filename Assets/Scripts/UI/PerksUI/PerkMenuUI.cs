using RPG.Control;
using RPG.Perks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Perks
{
    public class PerkMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject perksMenu;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] Image icon;
        [SerializeField] Transform choiceRoot;
        [SerializeField] PerkItemUI perkItemUI;
        [SerializeField] Button confirmButton;

        Perk choosedPerk;
        PerkList perkList;

        private void Start()
        {
            perkList = GameObject.FindGameObjectWithTag("Player").GetComponent<PerkList>();
            if (perkList == null) return;
            BuildPerkList();
            UpdateUI();
        }

        private void BuildPerkList()
        {
            choosedPerk = null;
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (var perk in SelectRandomPerks())
            {
                if (perk == null) continue;
                PerkItemUI uiInstance = Instantiate(perkItemUI, choiceRoot);
                uiInstance.Setup(perk);
                uiInstance.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    choosedPerk = perk;
                    UpdateUI();
                });
                if (choosedPerk == null) choosedPerk = perk;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (choosedPerk == null) return;
            title.text = choosedPerk.GetLocalizedName();
            description.text = choosedPerk.GetLocalizedDescription();
            icon.sprite = choosedPerk.GetIcon();
        }

        public void ConfirmButton()
        {
            perkList.GetPerkProgression().ChoosePerk(choosedPerk);
            if (choosedPerk != null)
            {
                perkList.AddPerkToList(choosedPerk);
            }
            perkList.gameObject.GetComponent<PlayerController>().FinishLevelUp();
            choosedPerk = null;
            perksMenu.SetActive(false);
            
        }

        public void ShowMenu()
        {
            BuildPerkList();
            perksMenu.SetActive(true);
        }

        private List<Perk> SelectRandomPerks()
        {
            List<Perk> perksList = new List<Perk>();
            foreach (Perk perk in perkList.GetPerkProgression().GetPerks())
            {
                if (perk == null) continue;
                perksList.Add(perk);
            }
            print(perksList.Count);
            int allowableCount = 4;
            if (perksList.Count < 4) allowableCount = perksList.Count;

            List<Perk> selectedObjects = new List<Perk>();

            // Копируем список объектов для безопасности
            List<Perk> tempList = new List<Perk>(perksList);

            // Выбираем 4 случайных объекта
            for (int i = 0; i < allowableCount; i++)
            {
                int randomIndex = Random.Range(0, tempList.Count);
                Perk selectedObject = tempList[randomIndex];
                selectedObjects.Add(selectedObject);
                tempList.RemoveAt(randomIndex);
            }

            // Перемешиваем порядок выбранных объектов
            for (int i = 0; i < selectedObjects.Count; i++)
            {
                Perk temp = selectedObjects[i];
                int randomIndex = Random.Range(i, selectedObjects.Count);
                selectedObjects[i] = selectedObjects[randomIndex];
                selectedObjects[randomIndex] = temp;
            }

            return selectedObjects;
        }
    }
}