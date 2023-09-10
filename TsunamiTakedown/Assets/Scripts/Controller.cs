using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    protected const float GRAVITY = 980.0f;

    protected DefaultPlayerActions defaultPlayerActions;

    [Header("Physics Properties")]
    [SerializeField]
    protected float mass = 100.0f;
    [SerializeField]
    protected float walkingFritction = 0.9f;
    [SerializeField]
    protected float brakingFritction = 0.9f;
    [SerializeField]
    protected float maxSpeed = float.MaxValue;
    [SerializeField]
    protected float maxAcceleration = float.MaxValue;
    /// <summary>
    /// Minimun dot product of up vector and walkable surface normal.
    /// </summary>
    [SerializeField]
    protected float slopeThreshold = float.MinValue;
    [SerializeField]
    protected float gravityScale = 2.0f;
    [SerializeField]
    protected float airControlMultiplier = .5f;
    protected Vector3 accumulatedForce;
    protected Vector3 componentVelocity;
    protected Vector3 groundNormal;
    /// <summary>
    /// Is the player standing on a walkable surface?
    /// </summary>
    protected bool grounded = false;
    /// <summary>
    /// Is the character touching aother collider?
    /// </summary>
    protected bool contacts = false;
    protected float baseWalkingFriction;

    [Header("Input")]
    [SerializeField]
    protected float inputScale = 50.0f;
    protected float pendingInput;

    protected float startZ;

    protected void Awake()
    {
        // Setup player actions.
        defaultPlayerActions = new DefaultPlayerActions();
        defaultPlayerActions.Enable();

        defaultPlayerActions.DefaultActionMap.Jump.performed += StartJump;
        defaultPlayerActions.DefaultActionMap.Jump.canceled += CancelJump;

        // Set base property values.
        baseWalkingFriction = walkingFritction;

        startZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        UpdatePhysics(Time.fixedDeltaTime);
    }

    /// <summary>
    /// Update the physics state, accouting for gravity, input, and friction.
    /// </summary>
    /// <param name="deltaTime"></param>
    void UpdatePhysics(float deltaTime)
    {

        // Apply forces
        if(!grounded) ApplyGravity();
        AddInput();
        ApplyFriction();
        ApplyInput();

        // Acceleration
        Vector3 a = accumulatedForce / mass;
        if(a.magnitude > maxAcceleration)
        {
            a = a.normalized * maxAcceleration;
        }

        // Velocity
        componentVelocity += a * deltaTime;
        if (componentVelocity.magnitude > maxSpeed)
        {
            componentVelocity = componentVelocity.normalized * maxSpeed;
        }
        


        // Appply delta position.
        Vector3 deltaPos = componentVelocity * deltaTime;

        RaycastHit hit;
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        Vector3 sphere1 = transform.position - ((capsule.height / 2) + capsule.radius) * transform.up;
        Vector3 sphere2 = transform.position + ((capsule.height / 2) + capsule.radius) * transform.up;

        if (Physics.Linecast(transform.position, transform.position + deltaPos, out hit))
        {
            print("Hit: " + hit.transform);
            deltaPos += hit.normal * (deltaPos.magnitude - hit.distance);
        }

        transform.position += deltaPos;
        Vector3 temp = transform.position;
        temp.z = startZ;
        transform.position = temp;

        // Zero out forces.
        accumulatedForce = Vector3.zero;
    }


    /// <summary>
    /// Add player action move value to Pending Input.
    /// </summary>
    protected void AddInput()
    {
        pendingInput += defaultPlayerActions.DefaultActionMap.Move.ReadValue<float>();  
    }

    /// <summary>
    /// Apply friction force.
    /// </summary>
    protected void ApplyFriction()
    {
        float useFriction = 0.0f;
        if (contacts)
        {
            // If we are touching a surface but not grounded, friction should resist movement against the surface but not sliding down it.
            useFriction = Vector3.Dot(groundNormal, pendingInput * transform.right) < 0 ? walkingFritction : 0f;
        }
        if (grounded)
        {
            // If we are grounded, just use the appropriate friction multiplier.
            useFriction = Mathf.Abs(pendingInput) > 0.01f ? walkingFritction : brakingFritction;
        }

        print("Fr: " + useFriction);

        Vector3 forceAlongNormal = Vector3.Dot(accumulatedForce + Vector3.down * GRAVITY * gravityScale, groundNormal) * groundNormal;
        float normalForce = forceAlongNormal.magnitude;
        Vector3 frictionForce = useFriction * normalForce * -componentVelocity;
        frictionForce = Vector3.ProjectOnPlane(frictionForce, groundNormal);
        accumulatedForce += frictionForce;
    }

    /// <summary>
    /// Applie Pending Input as force, the clears Pending Input.
    /// </summary>
    protected void ApplyInput()
    {
        print("ApplyInput");

        Vector3 force = transform.right * pendingInput * inputScale;
        if (grounded)
        {
            // Move parallel to floor.
            float magnitude = force.magnitude;
            force = Vector3.ProjectOnPlane(force, groundNormal);
            force = force.normalized * magnitude;
        }
        else if(!contacts)
        {
            force *= airControlMultiplier;
        }
        accumulatedForce += force;
        pendingInput = 0.0f;
    }


    /// <summary>
    /// Apply gravity force.
    /// </summary>
    protected void ApplyGravity()
    {
        accumulatedForce += Vector3.down * GRAVITY * gravityScale;
    }

    /// <summary>
    /// Collision correction and normal impulse for all contacts.
    /// </summary>
    /// <param name="collision"></param>
    protected void HandleCollision(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            Vector3 correction;
            float distance;
            Physics.ComputePenetration(GetComponent<CapsuleCollider>(), transform.position, transform.rotation, collision.collider, collision.transform.position, collision.transform.rotation, out correction, out distance);

            if (distance > 0)
            {
                // Only apply the correction if it is valid.
                transform.Translate(correction * distance);

                componentVelocity += Vector3.Dot(-componentVelocity, contact.normal) * contact.normal;

                if (!grounded) groundNormal = Vector3.zero;
                groundNormal += contact.normal;

                print(Vector3.Dot(contact.normal, Vector3.up));
                
                if(Vector3.Dot(contact.normal, Vector3.up) >= slopeThreshold)
                {
                    grounded = true;
                }

                contacts = true;
            }
        }

        groundNormal.Normalize();
    }


    protected void OnCollisionEnter(Collision collision)
    {
        print("no.");
        HandleCollision(collision);
    }

    protected void OnCollisionStay(Collision collision)
    {
        print("no.");
        HandleCollision(collision);
    }

    protected void OnCollisionExit(Collision collision)
    {
        grounded = false;
        contacts = false;
        groundNormal = Vector3.zero;
    }


    protected void StartJump(InputAction.CallbackContext context)
    {
        walkingFritction = 0.0f;
    }

    protected void CancelJump(InputAction.CallbackContext context)
    {
        walkingFritction = baseWalkingFriction;
    }


    public Vector3 GetVelocity()
    {
        return componentVelocity;
    }

    public float GetInput()
    {
        return pendingInput;
    }

    public bool GetContact()
    {
        return contacts;
    }
}
