using Managers;

namespace InGame.Particle
{
    public class ConfettiManager : ParticleManager
    {
        private void Start()
        {
            GameEvents.onWin += Play;
        }
        private void OnDestroy()
        {
            GameEvents.onWin -= Play;
        }
    }

}
