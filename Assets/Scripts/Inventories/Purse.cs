using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        int balance = 0;

        public event Action onChange;

        public int GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(int amount)
        {
            balance += amount;
            if (onChange != null)
            {
                onChange();
            }
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (int)state;
        }
    }
}