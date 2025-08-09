using Game.Entities.Movement;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

namespace Game.Entities {
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour {

        [SerializeField] private float _animationDamp;
        private Animator _animator;
        private MovementController _movementController;
        #region Animator Hashes
        private static readonly int HashSpeed = Animator.StringToHash("Speed");
        #endregion
        private void Start() {
            _animator = GetComponent<Animator>();
            _movementController = GetComponentInParent<MovementController>();
        }

        private void FixedUpdate() {
            _animator.SetFloat(HashSpeed, (_movementController.MovementDirection).magnitude, _animationDamp, Time.deltaTime);
        }

       
    }
}