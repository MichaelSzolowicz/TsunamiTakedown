using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotParallelFloor : MonoBehaviour
{
    public Transform updatedTransform;
    public float probeDepth = 50.0f;
    protected Controller controller;

    public bool probeHit = false;

    protected void Awake()
    {
        controller = GetComponentInParent<Controller>();
    }


    protected void Update()
    {
        UpdateRotation();
    }

    protected void UpdateRotation()
    {
        RaycastHit hit = RaycastDown();

        Quaternion lookRotation = transform.rotation;

        if (hit.transform)
        {
            print("Hit rot: " + hit.transform);

            ///if(controller.GetVelocity().magnitude > 0.1f)
            {
                if(controller.GetVelocity().magnitude != 0)
                {
                    Quaternion velRotation = Quaternion.LookRotation(-controller.GetVelocity().normalized);
                    Quaternion temp = updatedTransform.rotation;
                    temp.y = velRotation.y;
                    updatedTransform.rotation = temp;
                }


                lookRotation = Quaternion.LookRotation(Vector3.Cross(updatedTransform.right, hit.normal));
                updatedTransform.rotation = lookRotation;  
            }
        }

        print("Look at: " + lookRotation);
    }

    protected RaycastHit RaycastDown()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 start = updatedTransform.position;

        if(Physics.Raycast(start, Vector3.down, out hit, probeDepth, 3))
        {
            probeHit = true;
        }
        else
        {
            probeHit = false;
        }

        return hit;
    }
}
