using Game.Item.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataManagement {

    //Adapted from an SO database script from an older project
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
    public class ItemDatabase : ScriptableObject {
        [SerializeField, HideInInspector]
        private List<string> _guids = new List<string>();
        [SerializeField, HideInInspector]
        private List<ItemData> _items = new List<ItemData>();

        private Dictionary<string, ItemData> _itemsByGuid;
        private Dictionary<ItemData, string> _guidsByItem;

        public void OnEnable() {
            _itemsByGuid = new Dictionary<string, ItemData>();
            _guidsByItem = new Dictionary<ItemData, string>();
            for(int i = 0; i < _guids.Count; i++) {
                if(_guids[i] != null && _items[i] != null) {
                    _itemsByGuid[_guids[i]] = _items[i];
                    _guidsByItem[_items[i]] = _guids[i];
                }
            }
        }

        public ItemData GetItemByGuid(string guid) {
            if(_itemsByGuid == null) OnEnable();
            _itemsByGuid.TryGetValue(guid, out ItemData item);
            return item;
        }

        public string GetGuidByItem(ItemData item) {
            if(_guidsByItem == null) OnEnable();
            _guidsByItem.TryGetValue(item, out string guid);
            return guid;
        }
    }
}