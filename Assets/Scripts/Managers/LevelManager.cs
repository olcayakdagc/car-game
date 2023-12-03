using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.LevelSystem
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public int levelCount { get { return levelAsset.Length; } }
        [SerializeField] LevelAsset[] levelAsset;
        bool spamCheck = true;
        [SerializeField] bool isWork;
        private void Start()
        {
            var level = DataHandler.instance.level % levelAsset.Length;

            if (!isWork)
                StartCoroutine(LoadFirst(DataHandler.instance.level % levelAsset.Length));
        }
        public void Restart()
        {
            if (spamCheck)
            {
                spamCheck = false;
                GameEvents.onRestart?.Invoke();
                var level = DataHandler.instance.level % levelAsset.Length;

                StartCoroutine(Load(level, SceneManager.GetSceneAt(1).name));
            }

        }
        public void NextLevel()
        {
            if (spamCheck)
            {
                DataHandler.instance.Increaselevel();
                spamCheck = false;
                var level = DataHandler.instance.level % levelAsset.Length;

                StartCoroutine(Load(level, SceneManager.GetSceneAt(1).name));
            }

        }
        IEnumerator Load(int whereScene, string unlodName)
        {

            GameEvents.onLoad?.Invoke();
            GameManager.instance.WaitState();

            var asyncUnload = SceneManager.UnloadSceneAsync(unlodName);
            yield return new WaitUntil(() => asyncUnload.isDone);

            var name = levelAsset[whereScene].name;
            var asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            yield return new WaitUntil(() => asyncLoad.isDone);
            spamCheck = true;
            yield return new WaitForSeconds(1f);
            GameManager.instance.StartState();

            GameEvents.onLoadEnd?.Invoke();

        }
        IEnumerator LoadFirst(int whereScene)
        {
            GameEvents.onLoad?.Invoke();
            yield return new WaitForSeconds(0.2f);

            var name = levelAsset[whereScene].name;
            var asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            yield return new WaitUntil(() => asyncLoad.isDone);

            yield return new WaitForSeconds(1f);
            GameManager.instance.StartState();
            GameEvents.onLoadEnd?.Invoke();

        }
    }

}
