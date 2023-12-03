using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;


namespace CarSystem.CarController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class CarController : MonoBehaviour
    {
        [Header("Car Setup")]
        public MeshRenderer meshRenderer;
        public int stopLightsMaterialIndex = 14;

        [Header("Wheel Setup")]
        [SerializeField] Wheels _wheels;
        [SerializeField] List<ParticleSystem> _tireParticles;

        [Header("Motor Setup")]
        public float motorPower;
        public float breakPower;
        public float maxSpeed;

        [Header("Steering Setup")]
        public float steeringDamping;
        public float maxSteering;
        public float steeringSpeed;

        [Header("Road Setup")]

        [SerializeField] SplineContainer spline;
        [SerializeField] float roadBorder;
        [SerializeField] float pathConsistency;

        public bool canMove { get; set; } = false;

        public float speedParam { get; set; }

        public Rigidbody rb { get; set; }

        private Vector3 _splinePosition;
        public Quaternion splineRotation { get; set; }

        public float rotateNumber { get; set; }


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }


        protected void FixedUpdate()
        {

            ApplyGravity();
            Restrict();
            UpdateWheels();
            speedParam = rb.velocity.magnitude * 3.6f;
            UpdateSplinePositionAndRotation();
            HandleCarRotation();
            PreventSlidingOff();
        }

        public void CanMove()
        {
            canMove = true;
        }
        public void CantMove()
        {
            canMove = false;
        }
        public virtual void FinishedSpline()
        {
            rotateNumber = 0;
            canMove = false;
            Deccelerate();
        }
        private void UpdateSplinePositionAndRotation()
        {
            float point = SplineUtility.GetNearestPoint(spline.Spline, transform.position, out float3 nearest, out float t);

            _splinePosition = nearest;
            Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
            Vector3 up = spline.EvaluateTangent(t);

            splineRotation = Quaternion.LookRotation(forward, up);

        }
        private void HandleCarRotation()
        {
            float rotationX = transform.rotation.eulerAngles.x;

            float rotationY = splineRotation.eulerAngles.y;
            float rotationZ = transform.rotation.eulerAngles.z;

            transform.rotation = (Quaternion.Lerp(rb.rotation, Quaternion.Euler(rotationX, rotationY + rotateNumber, rotationZ), Time.fixedDeltaTime * pathConsistency));
        }

        private void PreventSlidingOff()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            localVelocity = new Vector3(0f, localVelocity.y, localVelocity.z);
            rb.velocity = transform.TransformDirection(localVelocity);
        }


        private void Restrict()
        {
            float dst = Vector3.Distance(_splinePosition, transform.position);
            if (dst > roadBorder + 0.3f)
            {
                Vector3 vect = _splinePosition - transform.position;
                vect = vect.normalized;
                vect *= (dst - roadBorder);
                vect = new Vector3(vect.x, 0, vect.z);
                transform.position = Vector3.Lerp(transform.position, transform.position += vect, Time.fixedDeltaTime * (3f + speedParam / 50));
            }
        }
        private void ApplyGravity()
        {
            if (!_wheels.frontLeft.isGrounded || !_wheels.frontRight.isGrounded || !_wheels.backRight.isGrounded || !_wheels.backLeft.isGrounded)
                rb.AddForce(Physics.gravity * 2, ForceMode.Acceleration);
        }

        public void Accelerate()
        {
            if (speedParam < maxSpeed)
            {
                _wheels.backRight.motorTorque = motorPower;
                _wheels.backLeft.motorTorque = motorPower;
            }
            else
            {
                _wheels.backRight.motorTorque = 0;
                _wheels.backLeft.motorTorque = 0;
            }
        }

        public void PlayTireParticles()
        {
            foreach (ParticleSystem particle in _tireParticles)
            {
                particle.Play();
            }
        }
        public void StopTireParticles()
        {
            foreach (ParticleSystem particle in _tireParticles)
            {
                particle.Stop();
            }
        }

        public void OpenStopLights()
        {
            meshRenderer.materials[stopLightsMaterialIndex].EnableKeyword("_EMISSION");
        }
        public void CloseStopLights()
        {
            meshRenderer.materials[stopLightsMaterialIndex].DisableKeyword("_EMISSION");
        }

        public void Deccelerate()
        {
            OpenStopLights();

            PlayTireParticles();

            _wheels.backRight.motorTorque = 0;
            _wheels.backLeft.motorTorque = 0;
            _wheels.frontRight.brakeTorque = breakPower;
            _wheels.frontLeft.brakeTorque = breakPower;
            _wheels.backRight.brakeTorque = breakPower;
            _wheels.backLeft.brakeTorque = breakPower;

        }
        public void ResetPower()
        {
            CloseStopLights();
            StopTireParticles();
            _wheels.backRight.motorTorque = 0;
            _wheels.backLeft.motorTorque = 0;
            _wheels.frontRight.brakeTorque = 0;
            _wheels.frontLeft.brakeTorque = 0;
            _wheels.backRight.brakeTorque = 0;
            _wheels.backLeft.brakeTorque = 0;

        }
        public void HandleSteering(float steeringInput)
        {
            float steering;
            Vector3 splineDirection = transform.InverseTransformPoint(_splinePosition);


            switch (steeringInput)
            {
                case 0:
                    rotateNumber = Mathf.Lerp(rotateNumber, 0, Time.deltaTime * steeringSpeed);
                    break;
                case 1:
                    steering = Vector3.Distance(_splinePosition, transform.position) > roadBorder && splineDirection.x < 0 ? 0 : maxSteering;

                    rotateNumber = Mathf.Lerp(rotateNumber, steering, Time.deltaTime * steeringSpeed);
                    break;
                case -1:

                    steering = Vector3.Distance(_splinePosition, transform.position) > roadBorder && splineDirection.x > 0 ? 0 : -maxSteering;

                    rotateNumber = Mathf.Lerp(rotateNumber, steering, Time.deltaTime * steeringSpeed);
                    break;

            }
        }


        private void UpdateWheels()
        {
            UpdateWheel(_wheels.frontLeft, _wheels.frontLeftGO);
            UpdateWheel(_wheels.frontRight, _wheels.frontRightGO);
            UpdateWheel(_wheels.backLeft, _wheels.backLeftGO);
            UpdateWheel(_wheels.backRight, _wheels.backRightGO);
        }
        private void UpdateWheel(WheelCollider _wheelCollider, GameObject wheel)
        {
            Vector3 position;
            Quaternion rotation;
            _wheelCollider.GetWorldPose(out position, out rotation);
            wheel.transform.position = position;
            wheel.transform.rotation = rotation;
        }


    }

    [System.Serializable]
    public struct Wheels
    {
        public WheelCollider frontLeft;
        public WheelCollider frontRight;
        public WheelCollider backLeft;
        public WheelCollider backRight;

        public GameObject frontLeftGO;
        public GameObject frontRightGO;
        public GameObject backLeftGO;
        public GameObject backRightGO;
    }

}
