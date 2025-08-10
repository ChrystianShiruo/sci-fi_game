using Game.Inventory;
using Game.Item.Data;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Entities {
    public class PlayerEquipment : MonoBehaviour {

        [SerializeField] private GameObject handSlotR;

        private ItemData _equippedItemR;
        private GameObject _equippedItemModelR;

        private void Start() {
            InventoryManager.Instance.OnEquipmentUpdated += UpdateEquipmentModels;
        }

        private void UpdateEquipmentModels() {
            if(InventoryManager.Instance.CurrentWeapon == _equippedItemR) {
                return;
            }
            if(_equippedItemModelR != null) {
                Destroy(_equippedItemModelR);
            }
            if(InventoryManager.Instance.CurrentWeapon != null) {
                _equippedItemModelR = Instantiate( InventoryManager.Instance.CurrentWeapon.prefab, handSlotR.transform);
                _equippedItemModelR.transform.localPosition = Vector3.zero;
            }
        }
    }
}