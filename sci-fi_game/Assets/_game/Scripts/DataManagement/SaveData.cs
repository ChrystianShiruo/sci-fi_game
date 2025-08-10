
using System;
using System.Collections.Generic;
namespace Game.DataManagement {

    public class SaveData {

        public List<SerializableInventorySlot> slots = new List<SerializableInventorySlot>();
        public SerializableInventorySlot equipment_handR;
    }

    [Serializable]
    public class SerializableInventorySlot {
        public string itemId = null;
        public int amount = 0;
        public int posX = -1;
        public int posY = -1;

        public SerializableInventorySlot(string id, int amount, int x, int y) {
            itemId = id;
            this.amount = amount;
            posX = x;
            posY = y;
        }
    }
}