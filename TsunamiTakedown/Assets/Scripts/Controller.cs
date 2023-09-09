using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    float GRAVITY = 980.0f;

    float mass = 100.0f;

    DefaultPlayerActions defaultPlayerActions;

    public float inputScale = 50.0f;

    public float pendingInput;

    public Vector3 accumulatedForce;

    public Vector3 gravityForce;

    public Vector3 componentVelocity;
    public Vector3 gravityVelocity;
    public Vector3 deltaPos;

    public Vector3 groundNormal;

    public float friction = 0.9f;

    bool grounded = false;

    public float groundThreshold = .8f;

    protected void Start()
    {
        defaultPlayerActions = new DefaultPlayerActions();
        defaultPlayerActions.Enable();

    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    void UpdatePhysics()
    {
        float DeltaTime = Time.fixedDeltaTime;

        //HandleCollision(deltaPos);
        transform.position += deltaPos;
        deltaPos = componentVelocity * DeltaTime;

        if(!grounded) AddGravity();
        AddInput();
        ApplyInput();

        if (grounded)
        {
            Vector3 forceAlongNormal = Vector3.Dot(accumulatedForce + Vector3.down * GRAVITY, groundNormal) * groundNormal;
            float normalForce = forceAlongNormal.magnitude;
            Vector3 frictionForce = friction * normalForce * -componentVelocity;
            frictionForce = Vector3.ProjectOnPlane(frictionForce, groundNormal);
            accumulatedForce += frictionForce;

            print("Friction: " + groundNormal);
        }


        Vector3 a = accumulatedForce / mass;
        componentVelocity += a * DeltaTime;

        print("Velocity" + componentVelocity);

        accumulatedForce = Vector3.zero;
    }

    protected void AddInput()
    {
        pendingInput += defaultPlayerActions.DefaultActionMap.Move.ReadValue<float>();  
    }

    protected void ApplyInput()
    {
        print("ApplyInput");
        accumulatedForce += transform.right * pendingInput * inputScale;
        pendingInput = 0.0f;
    }

    protected void AddGravity()
    {
        accumulatedForce += Vector3.down * GRAVITY;
    }


    protected void HandleCollision(Vector3 deltaPos, Collision collision)
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
                // Only apply the correction if it is valid.
                transform.Translate(correction * distance);

                componentVelocity += Vector3.Dot(-componentVelocity, contact.normal) * contact.normal;

                if(Vector3.Dot(contact.normal, Vector3.up) > groundThreshold)
                {
                    if (!grounded) groundNormal = Vector3.zero;
                    groundNormal += contact.normal;
                    grounded = true;
                }
            }
        }

        groundNormal.Normalize();

    }


    private void OnCollisionEnter(Collision collision)
    {
        print("Collide");
        HandleCollision(deltaPos, collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(deltaPos, collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
