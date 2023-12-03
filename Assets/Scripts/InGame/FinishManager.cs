using CarSystem.CarController;
using UnityEngine;

namespace Managers.InGame
{
    public class FinishManager : MonoBehaviour
    {
        public int whichPlace { get; set; } = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<EnemyCar>(out EnemyCar enemyCar))
            {
                whichPlace++;
                enemyCar.FinishedSpline();
            }
        }
    }
}

