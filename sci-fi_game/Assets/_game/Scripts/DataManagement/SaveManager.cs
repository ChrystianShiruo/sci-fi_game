using Game.Inventory;
using System.Collections;
using System.IO;
using UnityEngine;

//Adapted from an older project
namespace Game.DataManagement {
    public class SaveManager : MonoBehaviour {

        public static SaveManager Instance { get; private set; }

        [SerializeField] private ItemDatabase _itemDatabase;

        private string _savePath;

        private void Awake() {
            if(Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        }

        public void SaveInventory() {
            SaveData saveData = InventoryManager.Instance.GetSaveData(_itemDatabase);

            string json = JsonUtility.ToJson(saveData, true);

            File.WriteAllText(_savePath, json);
            Debug.Log("Inventory saved to: " + _savePath);
        }

        public void LoadInventory() {
            if(File.Exists(_savePath)) {
                string json = File.ReadAllText(_savePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                InventoryManager.Instance.LoadSaveData(saveData, _itemDatabase);
                Debug.Log("Inventory loaded.");
            } else {
                Debug.Log("No save file found. Starting with an empty inventory.");
            }
        }
    }
}
