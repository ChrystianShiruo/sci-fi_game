using Game.Inventory;
using UnityEngine;

namespace Game.Item.Data {
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "Data/Item/ConsumableData")]

    public class ConsumableData : ItemData {
        public override UseResult Use() {
            //consume

            return new UseResult { InventoryAction = ItemAction.RemoveOne, Success = true };
        }
    }
}
