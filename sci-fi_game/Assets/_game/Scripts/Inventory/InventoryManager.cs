using Game.DataManagement;
using Game.Item.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Inventory {
    public class InventoryManager : MonoBehaviour {

        public static InventoryManager Instance { get; private set; }
        public Vector2Int Dimensions { get => new Vector2Int(_width, _height); }

        public WeaponData CurrentWeapon { get => _weaponSlot.itemData as WeaponData; }
        public InventorySlot WeaponSlot { get => _weaponSlot; }

        public Action OnInventoryUpdated;
        public Action OnEquipmentUpdated;


        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private InventorySlot[,] _inventoryGrid;
        private InventorySlot _weaponSlot;
        private void Awake() {

            if(Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            _inventoryGrid = new InventorySlot[_width, _height];
            _weaponSlot = new InventorySlot(null, 1);
        }

        public InventorySlot GetSlot(Vector2Int position) {
            return _inventoryGrid[position.x, position.y];
        }

        //Adapted from old project script
        public bool TryAddItem(ItemData itemData, int amount) {
            if(itemData == null) {
                return false;
            }
            if(itemData.maxStackAmount > 1) {
                for(int y = 0; y < _height; y++) {
                    for(int x = 0; x < _width; x++) {
                        InventorySlot slot = _inventoryGrid[x, y];
                        if(slot != null && slot.itemData == itemData && slot.amount < itemData.maxStackAmount) {
                            int amountCanTake = itemData.maxStackAmount - slot.amount;
                            int amountToAdd = Mathf.Min(amount, amountCanTake);
                            slot.amount += amountToAdd;
                            amount -= amountToAdd;

                            if(amount <= 0) {
                                OnInventoryUpdated?.Invoke();
                                return true;
                            }
                        }
                    }
                }
            }

            if(amount > 0) {
                for(int y = 0; y < _height; y++) {
                    for(int x = 0; x < _width; x++) {
                        if(_inventoryGrid[x, y] == null) {
                            int amountToAdd = Mathf.Min(amount, itemData.maxStackAmount);
                            _inventoryGrid[x, y] = new InventorySlot(itemData, amountToAdd);
                            amount -= amountToAdd;

                            if(amount <= 0) {
                                OnInventoryUpdated?.Invoke();
                                return true;
                            }
                        }
                    }
                }
            }

            OnInventoryUpdated?.Invoke();
            return amount <= 0;
        }

        public bool TryRemoveItem(Vector2Int itemPos) {
            if(_inventoryGrid[itemPos.x, itemPos.y] == null) {
                return false;
            }
            return TryRemoveItem(itemPos, _inventoryGrid[itemPos.x, itemPos.y].amount);
        }
        public bool TryRemoveItem(Vector2Int itemPos, int amount) {
            if(!IsPositionValid(itemPos)) {
                return false;
            }
            if(_inventoryGrid[itemPos.x, itemPos.y] == null) {
                return false;
            }
            _inventoryGrid[itemPos.x, itemPos.y].amount -= amount;
            if(_inventoryGrid[itemPos.x, itemPos.y].amount <= 0) {
                _inventoryGrid[itemPos.x, itemPos.y] = null;
            }

            OnInventoryUpdated?.Invoke();
            return true;
        }
        public bool TryMoveItem(Vector2Int posA, Vector2Int posB) {
            if(!IsPositionValid(posA) || !IsPositionValid(posB)) {
                return false;
            }

            InventorySlot slotA = _inventoryGrid[posA.x, posA.y];
            _inventoryGrid[posA.x, posA.y] = _inventoryGrid[posB.x, posB.y];
            _inventoryGrid[posB.x, posB.y] = slotA;

            OnInventoryUpdated?.Invoke();
            return true;
        }
        public bool TryUseItem(Vector2Int itemPos) {
            if(_inventoryGrid[itemPos.x, itemPos.y] == null) {
                return false;
            }
            UseResult itemUseResult = _inventoryGrid[itemPos.x, itemPos.y].itemData.Use();

            switch(itemUseResult.InventoryAction) {
                case ItemAction.None:
                    break;
                case ItemAction.RemoveOne:
                    TryRemoveItem(itemPos, 1);
                    break;
                default:
                    OnInventoryUpdated?.Invoke();
                    break;
            }
            return itemUseResult.Success;
        }
        public bool SplitStack(Vector2Int itemPos, int amount) {
            throw new NotImplementedException();
        }
        public bool JoinStacks(Vector2Int itemPos) {
            throw new NotImplementedException();
        }

        private bool IsPositionValid(Vector2Int pos) {
            return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
        }

        public bool TryEquipWeapon(WeaponData weaponData) {
            //Vector2Int pos = new Vector2Int(-1, -1);
            //for(int y = 0; y < _height; y++) {
            //    for(int x = 0; x < _width; x++) {
            //        if(_inventoryGrid[x, y].itemData == weaponData) {
            //            pos = new Vector2Int(x, y);
            //        }
            //    }
            //}

            ItemData currentWeapon = null;
            if(_weaponSlot != null) {
                currentWeapon = _weaponSlot.itemData;
            }
            _weaponSlot.itemData = weaponData;
            if(currentWeapon != null) {
                TryAddItem(currentWeapon, 1);
            }
            OnEquipmentUpdated?.Invoke();
            return true;
        }

        public SaveData GetSaveData(ItemDatabase itemDatabase) {
            SaveData saveData = new SaveData();
            string guid;
            for(int y = 0; y < _height; y++) {
                for(int x = 0; x < _width; x++) {
                    if(_inventoryGrid[x, y] != null) {
                        guid = itemDatabase.GetGuidByItem(_inventoryGrid[x, y].itemData);
                        int amount = _inventoryGrid[x, y].amount;
                        saveData.slots.Add(new SerializableInventorySlot(guid, amount, x, y));
                    }

                }
            }
            if(CurrentWeapon != null) {
                guid = itemDatabase.GetGuidByItem(CurrentWeapon);
                saveData.equipment_handR = (new SerializableInventorySlot(guid, 1, -1, -1));
            } else {
                saveData.equipment_handR = null;
            }
            return saveData;
        }
        public void LoadSaveData(SaveData saveData, ItemDatabase itemDatabase) {
            _inventoryGrid = new InventorySlot[_width, _height];
            foreach(SerializableInventorySlot slot in saveData.slots) {
                ItemData itemData = itemDatabase.GetItemByGuid(slot.itemId);

                if(itemData != null) {
                    _inventoryGrid[slot.posX, slot.posY] = new InventorySlot(itemData, slot.amount);
                } else {
                    Debug.LogError($" Unable to load item with GUID: {slot.itemId}");
                }
            }
            if(!String.IsNullOrEmpty(saveData.equipment_handR.itemId)) {
                ItemData equipData = itemDatabase.GetItemByGuid(saveData.equipment_handR.itemId);
                _weaponSlot = new InventorySlot(equipData, 1);
                OnEquipmentUpdated?.Invoke();
            }
            OnInventoryUpdated?.Invoke();
        }


    }
}