using System.Collections;
using UnityEngine;

namespace Game.Entities.Movement {
    public interface IDirectionSource {
        Vector2 Direction { get; }

        void Init();
        void Destroy();
    }
}