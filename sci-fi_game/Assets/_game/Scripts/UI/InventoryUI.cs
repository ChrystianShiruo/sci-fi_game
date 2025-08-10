using Game.GameLoop;
using Game.Input;
using Game.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class InventoryUI : MonoBehaviour {


        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private GameObject _uiSlotPrefab;
        [SerializeField] private GameObject _uiSelectedSlotPrefab;

        private GameObject _selectionInstance;
        private List<InventorySlotUI> _uiSlots;
        private Vector2Int _currentSelection;

        private Vector2Int CurrentSelection {
            get => _currentSelection;
            set {
                _currentSelection = value;
                UpdateSelectionUI();
            }

        }

        private void Awake() {
            _currentSelection = new Vector2Int(-1, -1);// no selection
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
            //InputHandler.Instance.EnableUIInput();
        }
        private void OnDisable() {
            //InputHandler.Instance.EnablePlayerInput();
        }

        public void ShowInventoryPanel(bool show) {
            _inventoryPanel.SetActive(show);
        }

        public void SelectSlot(int x, int y) {
            CurrentSelection = new Vector2Int(x, y);
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
    }
}