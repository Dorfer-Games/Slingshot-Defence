using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    public class PlayerDataInstaller : Installer
    {
        [SerializeField] bool isTesting;
        [SerializeField] [ShowIf("isTesting")] SaveData testData;

        public override int Order => 2;

        const string saveKey = "saveKey";
        SaveData data;

        public override void Process()
        {
            data = HandlePlayerData();

            Bootstrap.Instance.itemsToInject.Add(data);
            Bootstrap.Instance.EventSave += Save;
        }

        SaveData HandlePlayerData()
        {
#if UNITY_EDITOR
            return isTesting ? testData : Load();
#else
            return Load();
#endif
        }

        void Save()
        {
            SaveExtension.Save(data, saveKey);
        }

        SaveData Load()
        {
            return SaveExtension.Load(saveKey, new SaveData());
        }
    }
}
