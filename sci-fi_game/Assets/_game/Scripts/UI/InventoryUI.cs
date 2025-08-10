using Game.GameLoop;
using Game.Input;
using Game.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.UI {
    public class InventoryUI : MonoBehaviour {

        [SerializeField] private Image _dragIcon;
        [Space()]
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private GameObject _uiSlotPrefab;
        [SerializeField] private GameObject _uiSelectedSlotPrefab;

        private InventorySlotUI _draggedSlot;

        private GameObject _selectionInstance;
        private List<InventorySlotUI> _uiSlots;
        private Vector2Int _currentSelection;
        private readonly Vector2Int _defaultSelectionValue = new Vector2Int(-1, -1);// no selection
        private Vector2Int CurrentSelection {
            get => _currentSelection;
            set {
                _currentSelection = value;
                UpdateSelectionUI();
            }

        }

        private void Awake() {
            _currentSelection = _defaultSelectionValue;
            _uiSlots = new List<InventorySlotUI>();
            _selectionInstance = Instantiate(_uiSelectedSlotPrefab, _gridLayoutGroup.transform);
            _inventoryPanel.SetActive(false);
        }
        private void Start() {

            if(InventoryManager.Instance == null) {
                _inventoryPanel.SetActive(false);
                return;
            }
            GameStateManager.OnStateChanged += CheckState;
            InitializeUI();

            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
            UpdateUI();
        }

        private void OnDestroy() {
            if(InventoryManager.Instance != null) {
                InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
            }
            GameStateManager.OnStateChanged -= CheckState;
        }

        private void OnEnable() {
            UpdateUI();
            if(InputHandler.Instance) {
                InputHandler.Instance.AddCallback(InputHandler.Instance.UIActions.DeleteSelection, DeleteSelection, InputActionPhase.Performed);
            }
        }
        private void OnDisable() {
            if(InputHandler.Instance) {
                InputHandler.Instance.RemoveCallback(InputHandler.Instance.UIActions.DeleteSelection, DeleteSelection, InputActionPhase.Performed);
            }
        }

        private void DeleteSelection(InputAction.CallbackContext context) {
            if(CurrentSelection.x < 0) {//no selection
                return;
            }
            InventoryManager.Instance?.TryRemoveItem(CurrentSelection);
            CurrentSelection = _defaultSelectionValue;
        }


        public void ShowInventoryPanel(bool show) {
            _inventoryPanel.SetActive(show);
        }

        public void SelectSlot(Vector2Int position) {
            if(CurrentSelection.x < 0) {
                CurrentSelection = new Vector2Int(position.x, position.y);
                return;
            }
            if(CurrentSelection == position) {//Deselect
                CurrentSelection = _defaultSelectionValue;
                return;
            }
            InventoryManager.Instance.TryMoveItem(CurrentSelection, position);
            CurrentSelection = _defaultSelectionValue;

        }

        private void CheckState(GameStateManager.GameState state) {
            ShowInventoryPanel(state == GameStateManager.GameState.InventoryMenu);
        }

        private void InitializeUI() {

            _selectionInstance.SetActive(false);

            Vector2Int dimensions = InventoryManager.Instance.Dimensions;

            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = dimensions.x;



            for(int y = 0; y < dimensions.y; y++) {
                for(int x = 0; x < dimensions.x; x++) {
                    GameObject slotGO = Instantiate(_uiSlotPrefab, _gridLayoutGroup.transform);
                    slotGO.name = $"Slot [{x},{y}]";

                    InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                    slotUI.Init(x, y, this);
                    _uiSlots.Add(slotUI);
                }

            }
        }

        private void UpdateUI() {
            foreach(InventorySlotUI slotUI in _uiSlots) {
                InventorySlot inventorySlot = InventoryManager.Instance.GetSlot(slotUI.Position);
                slotUI.UpdateVisuals(inventorySlot);
            }
            UpdateSelectionUI();
        }

        private void UpdateSelectionUI() {
            if(CurrentSelection.x < 0) {
                _selectionInstance.SetActive(false);
                return;
            }
            int index = CurrentSelection.y * InventoryManager.Instance.Dimensions.x + CurrentSelection.x;
            if(index < _uiSlots.Count) {
                _selectionInstance.transform.position = _uiSlots[index].transform.position;
                _selectionInstance.SetActive(true);
            }
        }

        public void OnSlotClicked(InventorySlotUI inventorySlotUI, PointerEventData eventData) {
            if(eventData.button == PointerEventData.InputButton.Right) {//use item
                InventoryManager.Instance.TryUseItem(inventorySlotUI.Position);
            } else if(eventData.button == PointerEventData.InputButton.Left) {//select item slot
                SelectSlot(inventorySlotUI.Position);
            }
        }

        #region Drag
        public void OnBeginDrag(InventorySlotUI slot, PointerEventData eventData) {
            if(InventoryManager.Instance.GetSlot(slot.Position)== null) {
                return;
            }
            _draggedSlot = slot;
            _dragIcon.sprite = _draggedSlot.IconImage.sprite;
            _dragIcon.gameObject.SetActive(true);
            _dragIcon.transform.position = eventData.position;

            _selectionInstance.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData) {
            if(_dragIcon.gameObject.activeInHierarchy) {
                _dragIcon.transform.position = eventData.position;
            }
        }

        public void OnEndDrag() {
            _dragIcon.gameObject.SetActive(false);
            _draggedSlot = null;
            CurrentSelection = _defaultSelectionValue;
        }

        public void OnDrop(InventorySlotUI targetSlot) {
            if(_draggedSlot == null) {
                return;
            }

            InventoryManager.Instance?.TryMoveItem(_draggedSlot.Position, targetSlot.Position);

            OnEndDrag();
        }
        #endregion
    }
}