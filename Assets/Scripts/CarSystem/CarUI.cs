using TMPro;
using UnityEngine;

namespace UI.Car
{
    public class CarUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI carSpeed;

        private void Start()
        {
            CarSystem.CarController.PlayerCar.speed += SetSpeed;
        }
        private void OnDestroy()
        {
            CarSystem.CarController.PlayerCar.speed -= SetSpeed;
        }
        private void SetSpeed(float speed)
        {
            carSpeed.text = ((int)speed).ToString();
        }
    }

}
