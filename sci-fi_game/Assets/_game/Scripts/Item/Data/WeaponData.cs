using Game.Inventory;
using UnityEngine;

namespace Game.Item.Data {
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Item/WeaponData")]

    public class WeaponData : ItemData {
        public override UseResult Use() {
            //equip
            throw new System.NotImplementedException();
        }
    }
}
