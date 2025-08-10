using Game.Inventory;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI {
    public class InventorySlotUI : MonoBehaviour {

        public Vector2Int Position { get=> _position;}

        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Button _button;

        private Vector2Int _position;
        private InventoryUI _inventoryUIRef;
        
        public void Init(int x, int y, InventoryUI parent) {
            _position = new Vector2Int(x, y);
            _inventoryUIRef = parent;
            _button.onClick.AddListener(Select);
        }

        private void Select() {
            _inventoryUIRef.SelectSlot(Position.x, Position.y);
        }

        public void UpdateVisuals(InventorySlot inventorySlot) {
            if(inventorySlot == null) {
                _iconImage.enabled = false;
                _iconImage.sprite = null;
                _amountText.text = string.Empty;
                return;
            }
            _iconImage.enabled = true;
            _iconImage.sprite = inventorySlot.itemData.icon;
            _amountText.text = inventorySlot.amount > 1 ? inventorySlot.amount.ToString() : string.Empty;
        }


    }
}