using Game.Entities.Movement;
using UnityEditor.Animations;
using UnityEngine;

namespace Game.Entities {
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour {

        private Animator _animator;
        private MovementController _movementController;
        #region Animator Hashes
        private static readonly int HashX = Animator.StringToHash("X");
        private static readonly int HashY = Animator.StringToHash("Y");
        #endregion
        private void Start() {
            _animator = GetComponent<Animator>();
            _movementController = GetComponentInParent<MovementController>();
        }

        private void FixedUpdate() {
            _animator.SetFloat(HashX, _movementController.MoveDirection.x);
            _animator.SetFloat(HashY, _movementController.MoveDirection.y);
        }

    }
}