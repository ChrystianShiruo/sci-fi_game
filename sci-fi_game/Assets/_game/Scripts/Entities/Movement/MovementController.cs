using Game.Entities.Data;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Game.Entities.Movement {

    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour {

        public Vector2 MoveDirection { get => _directionSource.Direction; }

        [SerializeField] private MonoBehaviour _directionSourceClass;
        [SerializeField] private MovementData _movementData;   

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
            _rigidbody.linearVelocity = new Vector3(MoveDirection.x * _movementData.speed, _rigidbody.linearVelocity.y,
                MoveDirection.y*_movementData.speed);
        }

    }

}