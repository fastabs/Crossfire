using System.IO;
using UnityEngine;

namespace Crossfire.Workspace
{
    public interface ISaveEntryRepository
    {
        bool IsSaveFileExists { get; }
        void Save(SaveEntry saveEntry);
        void DeleteSave();
        void LoadSave();
        bool TryGetLoadedSave(out SaveEntry saveEntry);
    }

    public sealed class SaveEntryRepository : ISaveEntryRepository
    {
        private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, "Save.save");

        private SaveEntry _loadedSave;

        public bool IsSaveFileExists => File.Exists(SaveFilePath);

        public void Save(SaveEntry saveEntry)
        {
            var json = JsonUtility.ToJson(saveEntry);
            File.WriteAllText(SaveFilePath, json);
            _loadedSave = saveEntry;
        }

        public void DeleteSave()
        {
            if (IsSaveFileExists)
                File.Delete(SaveFilePath);

            _loadedSave = null;
        }

        public void LoadSave()
        {
            if (!IsSaveFileExists)
            {
                _loadedSave = null;
                return;
            }

            var json = File.ReadAllText(SaveFilePath);
            _loadedSave = JsonUtility.FromJson<SaveEntry>(json);
        }

        public bool TryGetLoadedSave(out SaveEntry saveEntry)
        {
            if (_loadedSave == null)
            {
                saveEntry = null;
                return false;
            }

            saveEntry = _loadedSave;
            return true;
        }
    }
}
