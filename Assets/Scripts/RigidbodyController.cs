using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RigidbodyController : MonoBehaviour
{
    public List<Collider> ragdollParts = new List<Collider>();
    [FormerlySerializedAs("rigidbodieParts")] public List<Rigidbody> rigidbodyParts = new List<Rigidbody>();
    private Collider baseCollider;
    private Animator animator;
    private void Awake()
    {
        RagdollInit();
        SetRagdollParts();
    }

    void Start()
    {
        baseCollider = this.gameObject.GetComponent<Collider>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnableRigibody(bool isEnabled )
    {
        if (isEnabled)
        {
            baseCollider.enabled = animator.enabled = false;
            foreach (var r in rigidbodyParts.ToArray())
            {
                Debug.Log(r);
                r.isKinematic = false;
                r.AddExplosionForce(30,Vector3.forward, 10f);
            }
            foreach (var c in ragdollParts.ToArray())
            {
                c.isTrigger = false;
            }
        }
        else
        {
            {
                baseCollider.enabled = animator.enabled = true;
                foreach (var r in rigidbodyParts.ToArray())
                {
                    Debug.Log(r);
                    r.isKinematic = true;
                }
                foreach (var c in ragdollParts.ToArray())
                {
                    c.isTrigger = true;
                }
            
            }

        }

    }
    void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                ragdollParts.Add(c);
            }
        }
    }

    void RagdollInit()
    {
        Rigidbody[] rigidbodies = this.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
            r.collisionDetectionMode = CollisionDetectionMode.Continuous;
            r.gameObject.layer = 8;
            r.mass /= 200;
            r.constraints = RigidbodyConstraints.FreezePositionZ;
            rigidbodyParts.Add(r);

        }
    }
}