using Game.Inventory;
using UnityEngine;

namespace Game.Item.Data {

    public abstract class ItemData : ScriptableObject {
        public string itemName;
        [TextArea()]
        public string description;
        public Sprite icon;
        public GameObject prefab;
        public int maxStackAmount = 1;
        public int value;
        public abstract UseResult Use();
    }


    public struct UseResult {
        public bool Success;
        public ItemAction InventoryAction;
    }
    public enum ItemAction {
        None,
        RemoveOne
    }
}
