using ICities;
using UnityEngine;
using static QuestLog.Mod;

namespace QuestLog
{
    public class UIloader : LoadingExtensionBase
    {
        private LoadMode _mode;

        public override void OnLevelLoaded(LoadMode mode)
        {
            this._mode = mode;

            GameObject gameObject = new GameObject("QuestLogHolder");
            gameObject.AddComponent<TestBehaviour>();
        }
    }
}

