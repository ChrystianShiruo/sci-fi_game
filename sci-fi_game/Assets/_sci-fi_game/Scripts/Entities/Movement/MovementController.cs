using Game.Entities.Data;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Game.Entities.Movement {

    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour {

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
            Vector2 moveDirection = _directionSource.Direction;
            _rigidbody.linearVelocity = new Vector3(moveDirection.x * _movementData.speed, _rigidbody.linearVelocity.y,
                moveDirection.y*_movementData.speed);
        }

    }

}