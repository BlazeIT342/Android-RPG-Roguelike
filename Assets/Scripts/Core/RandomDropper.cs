using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class RandomDropper : MonoBehaviour
    {
        //CONFIG DATA
        [Tooltip("How far can the pickups be scattered from the dropper")]
        [SerializeField] DropLibrary[] dropLibraries;
        [SerializeField] float scatterDistance = 2f;

        //CONSTANTS
        const int ATTEMPTS = 30;

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();

            foreach (var dropLibrary in dropLibraries)
            {
                var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
                foreach (var drop in drops)
                {
                    Instantiate(drop.item, GetDropLocation(), Quaternion.identity);
                }
            }         
        }

        private Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}