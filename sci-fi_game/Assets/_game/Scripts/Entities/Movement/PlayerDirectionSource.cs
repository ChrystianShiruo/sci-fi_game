
using Game.Entities.Movement;
using Game.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entities.Movement {
    public class PlayerDirectionSource : MonoBehaviour, IDirectionSource {
        public Vector2 Direction { get; private set; }

        public void Awake() {
            Init();
        }
        private void OnDestroy() {
            Destroy();
        }
        public void Init() {
            InputAction moveAction = InputHandler.Instance.PlayerActions.Move;

            InputHandler.Instance.AddCallback(moveAction, UpdateDirection, InputActionPhase.Performed);
            InputHandler.Instance.AddCallback(moveAction, UpdateDirection, InputActionPhase.Canceled);
        }
        public void Destroy() {
            InputAction moveAction = InputHandler.Instance.PlayerActions.Move;
            InputHandler.Instance.RemoveCallback(moveAction, UpdateDirection, InputActionPhase.Performed);
            InputHandler.Instance.RemoveCallback(moveAction, UpdateDirection, InputActionPhase.Canceled);
        }

        private void UpdateDirection(InputAction.CallbackContext context) {
            Direction = (context.ReadValue<Vector2>());
        }

    }

}