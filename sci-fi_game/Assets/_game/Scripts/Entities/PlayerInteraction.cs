using Game.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entities {
    public class PlayerInteraction : MonoBehaviour {

        private readonly List<ItemGameObject> _pickableItems = new List<ItemGameObject>();

        private void OnEnable() {
            if(InputHandler.Instance) {
                InputHandler.Instance.AddCallback(InputHandler.Instance.PlayerActions.Interact, DoInteraction, InputActionPhase.Performed);
            }
        }

        private void OnDisable() {
            if(InputHandler.Instance) {
                InputHandler.Instance.RemoveCallback(InputHandler.Instance.PlayerActions.Interact, DoInteraction, InputActionPhase.Performed);
            }

        }
        private void DoInteraction(InputAction.CallbackContext context) {
            if(_pickableItems.Count == 0) {
                return;
            }

            ItemGameObject closest = _pickableItems.OrderBy
                (item => Vector3.Distance(transform.position, item.transform.position)).FirstOrDefault();
            if(closest == null) {
                return;
            }
            if(closest.Pickup()) {
                _pickableItems.Remove(closest);
            }
        }

        public void RegisterInteractable(ItemGameObject item) {
            if(!_pickableItems.Contains(item)) {
                _pickableItems.Add(item);
                //Add cta to hud
            }
        }

        public void UnregisterInteractable(ItemGameObject item) {
            if(_pickableItems.Contains(item)) {
                _pickableItems.Remove(item);
                //Remove cta from hud if _pickableItems == 0

            }
        }
    }
}