using Game.Inventory;
using UnityEngine;

namespace Game.Item.Data {
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Item/WeaponData")]

    public class WeaponData : ItemData {
        public override UseResult Use() {

            if(InventoryManager.Instance.TryEquipWeapon(this)) {
                return new UseResult { InventoryAction = ItemAction.RemoveOne, Success = true };
            }
            return new UseResult { InventoryAction = ItemAction.None, Success = false };

        }
    }
}
