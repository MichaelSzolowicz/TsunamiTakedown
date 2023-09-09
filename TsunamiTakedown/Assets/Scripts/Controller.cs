using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    const float GRAVITY = 980.0f;

    DefaultPlayerActions defaultPlayerActions;

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
    [SerializeField]
    protected float slopeThreshold = float.MinValue;
    protected Vector3 accumulatedForce;
    public Vector3 componentVelocity;
    protected Vector3 groundNormal;
    public bool grounded = false;
    public bool contacts = false;

    [Header("Input")]
    [SerializeField]
    protected float inputScale = 50.0f;
    protected float pendingInput;


    protected float baseWalkingFriction;
    public float probeDistance = 1.0f;
    public float groundedGravityScale = 2.0f;
    public float airControlMultiplier = .5f;

    protected void Awake()
    {
        defaultPlayerActions = new DefaultPlayerActions();
        defaultPlayerActions.Enable();


        defaultPlayerActions.DefaultActionMap.Jump.performed += StartJump;
        defaultPlayerActions.DefaultActionMap.Jump.canceled += CancelJump;


        baseWalkingFriction = walkingFritction;
    }

    private void FixedUpdate()
    {
        UpdatePhysics(Time.fixedDeltaTime);
    }

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
        transform.position += deltaPos;

        // Zero out forces.
        accumulatedForce = Vector3.zero;
    }

    protected void AddInput()
    {
        pendingInput += defaultPlayerActions.DefaultActionMap.Move.ReadValue<float>();  
    }


    protected void ApplyFriction()
    {
        float useFriction = 0.0f;
        if (contacts)
        {
            useFriction = Vector3.Dot(groundNormal, pendingInput * transform.right) < 0 ? walkingFritction : 0f;
        }
        if (grounded)
        {
            useFriction = Mathf.Abs(pendingInput) > 0.01f ? walkingFritction : brakingFritction;
        }

        print("Fr: " + Vector3.Dot(groundNormal, pendingInput * transform.right));

        Vector3 forceAlongNormal = Vector3.Dot(accumulatedForce + Vector3.down * GRAVITY * groundedGravityScale, groundNormal) * groundNormal;
        float normalForce = forceAlongNormal.magnitude;
        Vector3 frictionForce = useFriction * normalForce * -componentVelocity;
        frictionForce = Vector3.ProjectOnPlane(frictionForce, groundNormal);
        accumulatedForce += frictionForce;
    }

    protected void ApplyInput()
    {
        print("ApplyInput");

        Vector3 force = transform.right * pendingInput * inputScale;
        if (contacts)
        {
            //force += Vector3.ProjectOnPlane(groundNormal, -force.normalized) * 999; 

            // Move parallel to floor.
            float magnitude = force.magnitude;
            force = Vector3.ProjectOnPlane(force, groundNormal);
            force = force.normalized * magnitude;
        }
        else
        {
            force *= airControlMultiplier;
        }
        accumulatedForce += force;
        pendingInput = 0.0f;
    }

    protected void ApplyGravity()
    {
        if (contacts) accumulatedForce += Vector3.down * GRAVITY * groundedGravityScale;
        else accumulatedForce += Vector3.down * GRAVITY;
    }


    protected void HandleCollision(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            print("loop");
            Vector3 correction;
            float distance;
            Physics.ComputePenetration(GetComponent<CapsuleCollider>(), transform.position, transform.rotation, collision.collider, collision.transform.position, collision.transform.rotation, out correction, out distance);

            print("Normal" + contact.normal);

            if (distance > 0)
            {
                contacts = true;
                // Only apply the correction if it is valid.
                transform.Translate(correction * distance);

                componentVelocity += Vector3.Dot(-componentVelocity, contact.normal) * contact.normal;

                if (!grounded) groundNormal = Vector3.zero;
                groundNormal += contact.normal;
                

                if(Vector3.Dot(contact.normal, Vector3.up) >= slopeThreshold)
                {
                    grounded = true;
                }
                else
                {

                }
            }
        }

        groundNormal.Normalize();
        print("Grnd: " + groundNormal);
    }


    protected void OnCollisionEnter(Collision collision)
    {
        print("Collide");
        HandleCollision(collision);
    }

    protected void OnCollisionStay(Collision collision)
    {
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
}
