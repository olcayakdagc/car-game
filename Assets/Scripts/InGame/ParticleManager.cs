using UnityEngine;


namespace InGame.Particle
{
    public abstract class ParticleManager : MonoBehaviour
    {
        [SerializeField] ParticleSystem prefab;

        public void Play()
        {
            prefab.Play();
        }
        public void Stop() 
        {
            prefab.Stop();
        }
    }
}

