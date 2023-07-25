using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ConfirmButton : MonoBehaviour
    {
        [SerializeField] Button startGame;
        [SerializeField] Button buyCharacter;

        public void StartGame()
        {
            startGame.gameObject.SetActive(true);
            buyCharacter.gameObject.SetActive(false);
        }

        public void BuyCharacter()
        {
            startGame.gameObject.SetActive(false);
            buyCharacter.gameObject.SetActive(true);
        }

        public Button GetBuyCharacter()
        {
            return buyCharacter;
        }
    }
}