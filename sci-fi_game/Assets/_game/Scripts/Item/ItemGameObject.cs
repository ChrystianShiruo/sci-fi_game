using Game.Entities;
using Game.Inventory;
using Game.Item.Data;
using System;
using UnityEngine;

public class ItemGameObject : MonoBehaviour {
    
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _amount = 1;



    private void OnTriggerEnter(Collider other) {

        PlayerInteraction playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
        if(playerInteraction != null) {
            playerInteraction.RegisterInteractable(this);
        }
    }
    private void OnTriggerExit(Collider other) {

        PlayerInteraction playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
        if(playerInteraction != null) {
            playerInteraction.UnregisterInteractable(this);
        }
    }


    public bool Pickup() {
        bool success = InventoryManager.Instance.TryAddItem(_itemData, _amount);
        if(success) {
            Destroy(gameObject);
        }
        return success;
    }
}
