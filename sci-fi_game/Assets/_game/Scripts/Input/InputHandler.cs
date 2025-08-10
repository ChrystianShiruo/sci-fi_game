using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

//**DISCLAIMER: This is an updated version of a InputHandler from a older personal project.
//The updates were adding callbacks to generic dictionaries instead of adding each Mapped InputAction individually, and handling "toggle" input such as Movement through Action triggers of
//started and canceled, older version handled it by checking it on Update()

namespace Game.Input {

    [DefaultExecutionOrder(-1)]
    public class InputHandler : MonoBehaviour {

        public static InputHandler Instance { get; private set; }

        public InputActions.PlayerActions PlayerActions { get => _inputActions.Player; }
        public InputActions.UIActions UIActions { get => _inputActions.UI; }

        private InputActions _inputActions;

        private readonly Dictionary<InputAction, List<Action<InputAction.CallbackContext>>> _startedActionSubscribers = new();
        private readonly Dictionary<InputAction, List<Action<InputAction.CallbackContext>>> _canceledActionSubscribers = new();
        private readonly Dictionary<InputAction, List<Action<InputAction.CallbackContext>>> _performedActionSubscribers = new();

        private void Awake() {
            if(Instance != null) {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _inputActions = new InputActions();
        }

        private void OnEnable() {

            foreach(InputAction action in _inputActions.Player.Get().actions) { SubscribeAction(action); }
            foreach(InputAction action in _inputActions.UI.Get().actions) { SubscribeAction(action); }
            _inputActions.Enable();
        }

        private void OnDisable() {
            foreach(var action in _inputActions.Player.Get().actions) { UnsubscribeAction(action); }
            foreach(var action in _inputActions.UI.Get().actions) { UnsubscribeAction(action); }
            _inputActions.Disable();
        }

        private void OnDestroy() {
            _inputActions.Dispose();
        }

        public void EnablePlayerInput() {
            _inputActions.Disable();
            _inputActions.Player.Enable();
        }

        public void EnableUIInput() {
            _inputActions.Disable();
            _inputActions.UI.Enable();
        }

        public void AddCallback(InputAction inputAction, Action<InputAction.CallbackContext> callback, InputActionPhase inputActionPhase) {
            if(GetPhaseDictionary(inputActionPhase).TryAdd(inputAction, new List<Action<InputAction.CallbackContext>>())) {
                GetPhaseDictionary(inputActionPhase)[inputAction].Add(callback);
            }
        }
        public void RemoveCallback(InputAction inputAction, Action<InputAction.CallbackContext> callback, InputActionPhase inputActionPhase) {
            if(GetPhaseDictionary(inputActionPhase).TryGetValue(inputAction, out var callbacks)) {
                callbacks.Remove(callback);
            }
        }

        private Dictionary<InputAction, List<Action<InputAction.CallbackContext>>> GetPhaseDictionary(InputActionPhase phase) {
            return phase switch {
                InputActionPhase.Started => _startedActionSubscribers,
                InputActionPhase.Canceled => _canceledActionSubscribers,
                InputActionPhase.Performed => _performedActionSubscribers,
                _ => null
            };
        }

        private void SubscribeAction(InputAction action) {
            action.started += HandleActionStartedCallbacks;
            action.performed += HandleActionPerformedCallbacks;
            action.canceled += HandleActionCanceledCallbacks;
        }

        private void UnsubscribeAction(InputAction action) {
            action.started -= HandleActionStartedCallbacks;
            action.performed -= HandleActionPerformedCallbacks;
            action.canceled -= HandleActionCanceledCallbacks;
        }

        private void HandleActionStartedCallbacks(CallbackContext context) {
            if(_startedActionSubscribers.TryGetValue(context.action, out var callbacks)) {
                foreach(var callback in callbacks) callback(context);
            }
        }

        private void HandleActionCanceledCallbacks(CallbackContext context) {
            if(_canceledActionSubscribers.TryGetValue(context.action, out var callbacks)) {
                foreach(var callback in callbacks) callback(context);
            }
        }

        private void HandleActionPerformedCallbacks(CallbackContext context) {
            if(_performedActionSubscribers.TryGetValue(context.action, out var callbacks)) {
                foreach(var callback in callbacks) callback(context);
            }
        }


    }
}