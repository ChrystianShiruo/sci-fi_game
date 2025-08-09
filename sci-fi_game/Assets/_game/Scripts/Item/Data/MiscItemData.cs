using UnityEngine;

namespace Game.Item.Data {
    [CreateAssetMenu(fileName = "MiscData", menuName = "Data/Item/MiscItemData")]

    public class MiscItemData : ItemData {
        
        public override UseResult Use() {
            return new UseResult { InventoryAction = ItemAction.None, Success = false };
        }
    }
}
