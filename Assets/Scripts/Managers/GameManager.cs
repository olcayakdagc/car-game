
namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameStates gameState { get; private set; } = GameStates.Wait;

        public void GameStart()
        {
            if (gameState == GameStates.Start)
            {
                GameEvents.onGameStart?.Invoke();
                gameState = GameStates.GamePlay;
            }
        }
        public void GameWin()
        {
            if (!(gameState == GameStates.GamePlay)) return;
            gameState = GameStates.Win;
            GameEvents.onWin?.Invoke();

        }
        public void GameFail()
        {
            if (!(gameState == GameStates.GamePlay)) return;

            gameState = GameStates.Lose;

            GameEvents.onFail?.Invoke();    

        }
        public void WaitState()
        {
            gameState = GameStates.Wait;
        }
        public void StartState()
        {
            gameState = GameStates.Start;
        }
    }
}


