using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public abstract class FilterStrategy : ScriptableObject
    {
        public abstract IEnumerable<GameObject> Filter(IEnumerable<GameObject> targets);

        internal IEnumerable<GameObject> Filter(object getTargets)
        {
            throw new NotImplementedException();
        }
    }
}