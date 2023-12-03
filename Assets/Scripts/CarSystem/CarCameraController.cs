using Managers;
using UnityEngine;


namespace CarSystem.Camera
{
    public class CarCameraController : MonoBehaviour
    {
        [SerializeField] CarController.CarController followCar;
        [SerializeField] Transform emptyFollower;


        [SerializeField] Vector3 offset;
        [SerializeField] Vector3 lookOffset;
        [SerializeField] float speed;

        private bool _isFollow = true;

        private void Awake()
        {
            GameEvents.onFail += StopFollow;
            GameEvents.onWin += StopFollow;
        }
        private void OnDestroy()
        {
            GameEvents.onFail -= StopFollow;
            GameEvents.onWin -= StopFollow;
        }
        private void StopFollow()
        {
            _isFollow = false;
        }
        private void FixedUpdate()
        {
            if (!_isFollow) return;

            emptyFollower.position = followCar.transform.position;

            emptyFollower.rotation = Quaternion.Euler(followCar.transform.eulerAngles.x, followCar.splineRotation.eulerAngles.y, followCar.transform.eulerAngles.z);

            transform.position = Vector3.Lerp(transform.position,
                emptyFollower.transform.position + emptyFollower.transform.TransformVector(offset)  * (-5), speed * Time.fixedDeltaTime);
            transform.LookAt(followCar.transform.position + lookOffset);
        }
    }
}

