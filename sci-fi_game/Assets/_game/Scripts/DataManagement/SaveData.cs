
using System;
using System.Collections.Generic;
namespace Game.DataManagement {

    public class SaveData {

        public List<SerializableInventorySlot> slots = new List<SerializableInventorySlot>();
    }

    [Serializable]
    public class SerializableInventorySlot {
        public string itemId;
        public int amount;
        public int posX;
        public int posY;

        public SerializableInventorySlot(string id, int amount, int x, int y) {
            itemId = id;
            this.amount = amount;
            posX = x;
            posY = y;
        }
    }
}