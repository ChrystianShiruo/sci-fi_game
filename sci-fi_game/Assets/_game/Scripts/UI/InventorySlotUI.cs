using Game.Inventory;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI {
    public class InventorySlotUI : MonoBehaviour {

        public Vector2Int Position { get=> _position;}

        [SerializeField] private Vector2Int _position;

        private SpriteRenderer _iconRenderer;
        private TextMeshProUGUI _amountText;
        private InventoryUI _inventoryUIRef;
        private void Start() {
            _iconRenderer = GetComponent<SpriteRenderer>();
            _amountText = GetComponent<TextMeshProUGUI>();
        }


        public void Init(int x, int y, InventoryUI parent) {
            _position = new Vector2Int(x, y);
            _inventoryUIRef = parent;
        }

        public void UpdateVisuals(InventorySlot inventorySlot) {
            _iconRenderer.sprite = inventorySlot.itemData.icon;
            _amountText.text = inventorySlot.amount > 1 ? inventorySlot.amount.ToString() : string.Empty;
        }
    }
}