using Game.Item.Data;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Inventory {
    public class InventoryManager : MonoBehaviour {

        public static InventoryManager Instance { get; private set; }
        public Vector2Int Dimensions { get => new Vector2Int(_width, _height); }

        public Action OnInventoryUpdated;


        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private InventorySlot[,] _inventoryGrid;

        private void Awake() {

            if(Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            _inventoryGrid = new InventorySlot[_width, _height];
        }

        public InventorySlot GetSlot(Vector2Int position) {
            return _inventoryGrid[position.x, position.y];
        }

        //Adapted from old project script
        public bool TryAddItem(ItemData itemData, int amount) {
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

            //InventorySlot slotA = _inventoryGrid[posA.x, posA.y] == null ? null :
            //    new InventorySlot(_inventoryGrid[posA.x, posA.y].itemData, _inventoryGrid[posA.x, posA.y].amount);
            InventorySlot slotA = _inventoryGrid[posA.x, posA.y];
            _inventoryGrid[posA.x, posA.y] = _inventoryGrid[posB.x, posB.y];
            _inventoryGrid[posB.x, posB.y] = slotA;

            OnInventoryUpdated?.Invoke();
            return true;
        }
        public bool TryUseItem(Vector2Int itemPos) {
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
    }
}