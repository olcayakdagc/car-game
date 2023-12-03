using Managers;
using UnityEngine;

namespace CarSystem.CarController
{
    public class EnemyCar : CarController
    {

        private void Start()
        {
            meshRenderer.materials[1].color = Random.ColorHSV();
            GameEvents.onGameStart += CanMove;
        }
        private void OnDestroy()
        {
            GameEvents.onGameStart -= CanMove;
        }
       
     
        private new void FixedUpdate()
        {
            base.FixedUpdate();
            if (!canMove) return;
            
            Accelerate();
        }
    }
}

