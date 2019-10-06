
using UnityEngine;

namespace Skoggy.LD45.Cameras
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform Target;
        public float Height = 5f;
        public float Distance = 5f;
        public float Damping = 10f;

        void Start()
        {
        }

        void Update()
        {
            if(Target == null) return;

            var targetPosition = 
                Target.position +
                (Vector3.back * Distance) +
                (Vector3.up * Height);

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Damping * Time.deltaTime);

            var direction = (Target.position - transform.position).normalized;

            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }
}