using System;
using Skoggy.LD45.Game.Consumables;
using UnityEngine;

namespace Skoggy.LD45.Game.Players
{
    public class Player : MonoBehaviour
    {
        public float Speed = 1f;
        public float JumpForce = 5f;
        public float Damping = 0.1f;
        public float GrowAnimationSpeed = 0.2f;
        public Transform Model;
        public SphereCollider Collider;
        public LayerMask GroundLayer;

        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private float _weight = 0.1f;
        private bool _grounded;

        public float Size => _weight;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _weight = 0.1f;
            UpdateWeight();
        }
        
        void OnCollisionEnter(Collision collision)
        {
            var consumable = collision.gameObject.GetComponent<Consumable>();
            if(consumable == null) return;

            // Maybe make shit stuck in the blob?
            var weight = consumable.Consume();
            AddWeight(weight);
        }

        private void AddWeight(float weight)
        {
            _weight += weight;
        }
        
        private void UpdateWeight()
        {
            var current = Model.localScale;
            var target = Vector3.one * _weight*2f;

            var scale = Vector3.Lerp(current, target, GrowAnimationSpeed);

            Model.localScale = scale;
            Collider.radius = _weight;
        }

        void Update()
        {
            _grounded = CheckGround();
            var input = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical")
            );

            var cam = ObjectLocator.Camera;

            if(cam == null) return;

            input = cam.transform.TransformVector(input);
            input.y = 0f;

            _movement = input.normalized * Mathf.Clamp01(input.magnitude);

            if(Input.GetButtonDown("Jump"))
            {
                TryJump();
            }

            UpdateWeight();
        }

        private static Collider[] _colliderCache = new Collider[12];
        private bool CheckGround()
        {
            var hits = Physics.OverlapSphereNonAlloc(
                transform.position + Vector3.down * Collider.radius, 
                0.3f, 
                _colliderCache,
                GroundLayer, 
                QueryTriggerInteraction.Ignore);

            return hits > 0;
        }

        private void TryJump()
        {
            if(!_grounded) return;

            _rigidbody.AddForce(Vector3.up * JumpForce * (_weight*2f), ForceMode.Impulse);
        }

        void FixedUpdate()
        {
            _rigidbody.AddForce(_movement * Speed);
            if(!_grounded)
            {
                _rigidbody.AddForce(Vector3.down * 20f);
            }
            var flatVelocity = _rigidbody.velocity;
            flatVelocity.y = 0f;

            _rigidbody.velocity += -flatVelocity * Damping;
        }
    }
}