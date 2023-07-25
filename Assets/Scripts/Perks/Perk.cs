using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Perks
{
    [CreateAssetMenu(fileName = "New Perk", menuName = "RPG Time Killer/Perks/New Perk", order = 0)]
    public class Perk : Item
    {
        [SerializeField] public EventFunction function;

        [System.Serializable]
        public class EventFunction : UnityEvent
        {

        }

        public EventFunction GetFunction()
        {
            return function;
        }     
    }
}