using Game.Entities.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;


namespace Game.Entities.Movement {

    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour {

        public MovementData MovementData { get => _movementData; }
        public Vector2 MovementDirection { get => _directionSource.Direction; }

        [SerializeField] private MonoBehaviour _directionSourceClass;
        [SerializeField] private MovementData _movementData;
        [SerializeField] private float _rotationSpeed;

        private IDirectionSource _directionSource;
        private Rigidbody _rigidbody;

        protected virtual void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            //_directionSource = GetComponent<IDirectionSource>();

            _directionSource = _directionSourceClass as IDirectionSource;
            if(_directionSource == null) {
                Debug.LogError("! Assigned directionSourceClass doies not implement IDirectionSource !");
                this.enabled = false;
            }
        }

        private void FixedUpdate() {

            _rigidbody.linearVelocity = new Vector3(MovementDirection.x * MovementData.speed, _rigidbody.linearVelocity.y,
                MovementDirection.y * MovementData.speed);

            if(_rigidbody.linearVelocity.magnitude > 0.1f) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(/*transform.position +*/ new Vector3(MovementDirection.x, 0, MovementDirection.y)), _rotationSpeed);
            }
        }


    }

}