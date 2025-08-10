using Game.DataManagement;
using Game.Input;
using Game.UI;
using System;
using UnityEngine;

namespace Game.GameLoop {
    public class GameStateManager : MonoBehaviour {

        public static Action<GameState> OnStateChanged;

        private GameState _currentState;

        public enum GameState {
            None,
            Gameplay,
            InventoryMenu,
            Paused
        }

        private void Start() {
            if(InputHandler.Instance == null) {
                Debug.LogError("Initialize InputHandler Instance");
                return;
            }

            SetGameState(GameState.Gameplay);

            InputHandler.Instance.AddCallback(InputHandler.Instance.PlayerActions.OpenInventory,
                (_) => SetGameState(GameState.InventoryMenu), UnityEngine.InputSystem.InputActionPhase.Performed);
            InputHandler.Instance.AddCallback(InputHandler.Instance.UIActions.CloseInventory,
                (_) => CloseInventory(), UnityEngine.InputSystem.InputActionPhase.Performed);
            InputHandler.Instance.AddCallback(InputHandler.Instance.UIActions.CloseMenu,
                (_) => SetGameState(GameState.Gameplay), UnityEngine.InputSystem.InputActionPhase.Performed);

            SaveManager.Instance.LoadInventory();
        }
        private void OnDestroy() {
            InputHandler.Instance.RemoveCallback(InputHandler.Instance.PlayerActions.OpenInventory,
                (_) => SetGameState(GameState.InventoryMenu), UnityEngine.InputSystem.InputActionPhase.Performed);
            InputHandler.Instance.RemoveCallback(InputHandler.Instance.UIActions.CloseInventory,
                (_) => CloseInventory(), UnityEngine.InputSystem.InputActionPhase.Performed);
            InputHandler.Instance.RemoveCallback(InputHandler.Instance.UIActions.CloseMenu,
                (_) => SetGameState(GameState.Gameplay), UnityEngine.InputSystem.InputActionPhase.Performed);
        }
        private void CloseInventory() {
            if(_currentState == GameState.InventoryMenu) {
                SetGameState(GameState.Gameplay);
            }
        }

        private void SetGameState(GameState newState) {
            _currentState = newState;
            switch(_currentState) {
                case GameState.None:
                    break;
                case GameState.Gameplay:
                    InputHandler.Instance.EnablePlayerInput();
                    break;
                case GameState.InventoryMenu:
                    InputHandler.Instance.EnableUIInput();
                    break;
                case GameState.Paused:
                    InputHandler.Instance.EnableUIInput();
                    break;
            }
            OnStateChanged?.Invoke(_currentState);
        }

    }
}