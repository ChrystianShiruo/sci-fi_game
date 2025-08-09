using System.Collections;
using UnityEngine;

namespace Game.Entities.Data {

    [CreateAssetMenu(fileName = "MovementData", menuName = "Data/MovementData")]
    public class MovementData : ScriptableObject {

        public float speed;
        public float jumpForce;
    }
}