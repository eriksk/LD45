using UnityEngine;

namespace Skoggy.LD45.Effects
{
    public class InstancedEntity
    {
        public Vector3 Position;
        public Quaternion Rotation = Quaternion.identity;
        public Vector3 Scale = Vector3.one;

        public InstancedEntity()
        {
        }

        public virtual Matrix4x4 Matrix => Matrix4x4.TRS(
            Position,
            Rotation,
            Scale
        );
    }
}