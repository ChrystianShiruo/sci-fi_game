using Game.Inventory;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI {
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler {

        public Vector2Int Position { get => _position; }

        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;

        private Vector2Int _position;
        private InventoryUI _inventoryUIRef;

        public void Init(int x, int y, InventoryUI parent) {
            _position = new Vector2Int(x, y);
            _inventoryUIRef = parent;
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

        public void OnPointerClick(PointerEventData eventData) {
            _inventoryUIRef.OnSlotClicked(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            //hide icon
            //create icon copy to drag
        }

        public void OnEndDrag(PointerEventData eventData) {
            //show icon

            //call trymoveitem through InventoryUI
        }
    }
}