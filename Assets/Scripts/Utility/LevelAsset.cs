using UnityEngine;

namespace Managers.LevelSystem
{
    [CreateAssetMenu(menuName = "LevelSystem/Level Asset", fileName = "LevelAsset")]
    public class LevelAsset : ScriptableObject
    {
        public SceneField asset;
    }
}
