using Game.Item.Data;

namespace Game.Inventory {
    public class InventorySlot {
        public ItemData itemData;
        public int amount;

        public InventorySlot(ItemData itemData, int amount) {
            this.itemData = itemData;
            this.amount = amount;
        }
    }
}