
using System;
using UnityEngine;

namespace Skoggy.LD45.Effects
{
    public class InstancedParticleSystem : InstancedEffectSystem<PlayerEffectParticle>
    {
        public void WalkSmoke(Vector3 position, Vector3 forward, Vector3 velocity)
        {
            var p = Items.Pop();
            if(p == null) return;
            
            p.Reset();

            var randomOffset = new Vector3(
                UnityEngine.Random.Range(-0.5f, 0.5f),
                UnityEngine.Random.Range(-0.5f, 0.5f),
                UnityEngine.Random.Range(-0.5f, 0.5f)
            ) * 0.2f;

            p.Position = position + randomOffset;
            p.Rotation = Quaternion.Euler(randomOffset);
            p.StartScale = UnityEngine.Random.Range(1f, 4f)*0.1f;
            p.Scale = Vector3.zero;
            p.EndScale = 0f;
            p.Current = 0f;
            p.Duration = UnityEngine.Random.Range(0.5f, 1f);
            p.Elastic = false;
            p.RotateForwardVelocity = false;
            p.Gravity = new Vector3();

            velocity.y = 0f;
            var direction = velocity * Vector3.Dot(forward, velocity.normalized) * 0.1f;
            p.Velocity = (Vector3.up * 0.2f) + direction;
        }

        public void CoinTrail(Vector3 position, Vector3 velocity)
        {
            var p = Items.Pop();
            if(p == null) return;
            
            p.Reset();

            var randomOffset = new Vector3(
                UnityEngine.Random.Range(-0.5f, 0.5f),
                UnityEngine.Random.Range(-0.5f, 0.5f),
                UnityEngine.Random.Range(-0.5f, 0.5f)
            ) * 0.2f;

            p.Position = position + randomOffset;
            p.Rotation = Quaternion.Euler(randomOffset);
            p.StartScale = UnityEngine.Random.Range(0.3f, 0.5f);
            p.Scale = Vector3.zero;
            p.EndScale = 0f;
            p.Current = 0f;
            p.Duration = UnityEngine.Random.Range(0.3f, 0.5f);
            p.Elastic = true;
            p.RotateForwardVelocity = true;
            p.Velocity = velocity * 0.3f;
            p.Damping = 1f;
            p.Gravity = new Vector3(0f, -5f, 0f);
        }

        public void SpawnPlayer(Vector3 position)
        {
            var count = 36;

            for(var i = 0; i < count; i++)
            {
                var p = Items.Pop();
                if(p == null) return;
                p.Reset();

                p.Duration = 1f;
                p.Delay = 0f;

                var angle = (i / (float)count) * 6.28f;

                p.Position = position + new Vector3(
                    Mathf.Cos(angle),
                    0f,
                    Mathf.Sin(angle)
                ) * 1f;
                
                p.StartScale = 1f;
                p.EndScale = 0f;
                p.Scale = Vector3.zero;
                p.Current = 0f;
                p.RotateForwardVelocity = true;
                p.Elastic = true;
                p.Gravity = new Vector3(0f, -10f, 0f);
                p.Velocity = Vector3.up * 5f;
                p.Damping = 0f;
            }
        }
        
        public void PlayerDropLand(Vector3 position)
        {
            var count = 36;

            for(var i = 0; i < count; i++)
            {
                var p = Items.Pop();
                if(p == null) return;
                p.Reset();

                p.Duration = 1f;
                p.Delay = 0f;

                var angle = (i / (float)count) * 6.28f;

                var direction = new Vector3(
                    Mathf.Cos(angle),
                    0f,
                    Mathf.Sin(angle)
                );

                p.Position = position + direction * 1f;
                
                p.StartScale = 1f;
                p.EndScale = 0f;
                p.Scale = Vector3.zero;
                p.Current = 0f;
                p.RotateForwardVelocity = true;
                p.Elastic = true;
                p.Gravity = new Vector3(0f, 5f, 0f);
                p.Velocity = direction * 5f;
                p.Damping = 5f;
            }
        }

        public void GroundPuff(Vector3 position)
        {
            var count = UnityEngine.Random.Range(20, 32);

            for(var i = 0; i < count; i++)
            {
                var p = Items.Pop();
                if(p == null) return;
                p.Reset();

                var angle = UnityEngine.Random.Range(-180f, 180f);
                var speed = UnityEngine.Random.Range(3f, 5f);
                p.Duration = UnityEngine.Random.Range(0.3f, 0.5f);

                p.Position = position;
                
                p.StartScale = UnityEngine.Random.Range(1f, 2f);
                p.EndScale = 0f;
                p.Scale = Vector3.zero;
                p.Current = 0f;
                p.RotateForwardVelocity = true;
                p.Elastic = true;
                p.Gravity = new Vector3(0f, 1f, 0f);
                p.Velocity = (Quaternion.Euler(
                    0,
                    angle,
                    0f
                ) * Vector3.forward) * speed;
                p.Damping = 5f;
            }
        }

        public void AddToBasket(Vector3 position)
        {
            Explosion(position, Vector3.up * 0.1f);
        }

        public void Explosion(Vector3 position, Vector3 velocity)
        {
            var count = UnityEngine.Random.Range(8, 12);

            for(var i = 0; i < count; i++)
            {
                var p = Items.Pop();
                if(p == null) return;
                p.Reset();

                var speed = UnityEngine.Random.Range(3f, 8f);

                p.Duration = UnityEngine.Random.Range(0.2f, 0.4f);
                p.Position = position + new Vector3(
                    UnityEngine.Random.Range(-0.3f, 0.3f),
                    UnityEngine.Random.Range(-0.3f, 0.3f),
                    UnityEngine.Random.Range(-0.3f, 0.3f)
                );
                
                p.StartScale = UnityEngine.Random.Range(0.5f, 2f) * 0.1f;
                p.EndScale = 0f;
                p.Scale = Vector3.zero;
                p.Current = 0f;
                p.RotateForwardVelocity = true;
                p.Elastic = true;
                p.Gravity = new Vector3(0f, -5f, 0f);
                p.Velocity = (Vector3.up + velocity.normalized) * speed;
                p.Damping = 2f;
            }
        }

        public void QuickFlash(Vector3 position)
        {
            var count = UnityEngine.Random.Range(2, 5);

            for(var i = 0; i < count; i++)
            {
                var p = Items.Pop();
                if(p == null) return;
                p.Reset();

                p.Duration = UnityEngine.Random.Range(0.1f, 0.2f);
                var range = 0.3f;
                p.Position = position + new Vector3(
                    UnityEngine.Random.Range(-range, range),
                    UnityEngine.Random.Range(-range, range),
                    UnityEngine.Random.Range(-range, range)
                );
                
                p.StartScale = UnityEngine.Random.Range(8f, 10f);
                p.EndScale = 0f;
                p.Scale = Vector3.zero;
                p.Current = 0f;
                p.RotateForwardVelocity = false;
                p.Elastic = false;
                p.Gravity = new Vector3(0f, 5f, 0f);
                p.Velocity = Vector3.zero;
                p.Damping = 0f;
            }
        }

        protected override void Update()
        {
            var delta = Time.deltaTime;

            for(var i = 0; i < Items.Count; i++)
            {
                var p = Items[i];
                if(p.Delay > 0f)
                {
                    p.Delay -= delta;
                    p.Scale = Vector3.zero; // Fake hide it
                    continue;
                }

                p.Current += delta;

                if(p.Current >= p.Duration)
                {
                    Items.Push(i--);
                    continue;
                }

                var progress = Mathf.Clamp01(p.Current / p.Duration);

                p.Velocity += p.Gravity * delta;
                
                if(p.Damping > 0f)
                {
                    p.Velocity = Vector3.Lerp(p.Velocity, Vector3.zero, p.Damping * delta);
                }

                p.Position += p.Velocity * delta;

                if(p.RotateForwardVelocity)
                {
                    p.Rotation = Quaternion.LookRotation(p.Velocity, Vector3.up);
                }

                if(p.Elastic)
                {
                    float elasticScaleMag = 1f;
                    var shape = Vector3.one + 
                                    new Vector3(
                                        0f,
                                        0f,
                                        p.Velocity.magnitude
                                    ) * elasticScaleMag;
                    p.Scale = Vector3.Lerp(shape * p.StartScale, shape * p.EndScale, progress);
                }
                else
                {
                    p.Scale = Vector3.Lerp(Vector3.one * p.StartScale, Vector3.one * p.EndScale, progress);
                }
            }
            
            base.Update();
        }
    }
    
    public class PlayerEffectParticle : InstancedEntity
    {
        public float Current, Duration;
        public Vector3 Velocity;
        public Vector3 Gravity;
        public float StartScale = 1f;
        public float EndScale = 0f;
        public bool RotateForwardVelocity;
        public bool Elastic;
        public float Damping = 0f;
        public float Delay = 0;
        
        public void Reset()
        {
            Current = 0f;
            Duration = 0f;
            Velocity = Vector3.zero;
            StartScale = 1f;
            EndScale = 0f;
            RotateForwardVelocity = false;
            Elastic = false;
            Damping = 0f;
            Delay = 0f;
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
            Scale = Vector3.one;
        }
    }
}