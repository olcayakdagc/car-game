using Managers;
using Managers.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace CarSystem.CarController
{
    public class PlayerCar : CarController
    {
        private float _firstClickXPos;

        public static event UnityAction<float> speed;

        private bool _isAcclerate;

        private void Start()
        {
            GameEvents.onGameStart += CanMove;
        }
        private void OnDestroy()
        {
            GameEvents.onGameStart -= CanMove;
        }

        private new void FixedUpdate()
        {
            base.FixedUpdate();

            speed?.Invoke(speedParam);
        }
        private void Update()
        {
            if (!canMove) return;
            if (Input.GetMouseButtonDown(0))
            {
                _isAcclerate = true;
                ResetPower();
                _firstClickXPos = Input.mousePosition.x;
            }
            if (Input.GetMouseButton(0))
            {

                Accelerate();


                float input = Input.mousePosition.x - _firstClickXPos;
                if (Mathf.Abs(input) > steeringDamping)
                {
                    int steering = input > 0 ? 1 : -1;

                    HandleSteering(steering);
                }
                else
                {
                    HandleSteering(0);
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                _isAcclerate = false;


                Deccelerate();
            }

            if (!_isAcclerate)
            {
                HandleSteering(0);
            }
        }
     
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<FinishManager>(out FinishManager finishManager))
            {
               FinishedSpline();
                if(finishManager.whichPlace == 1)
                {
                    GameManager.instance.GameWin();
                }
                else
                {
                    GameManager.instance.GameFail();
                }
            }
        }
    }
}

