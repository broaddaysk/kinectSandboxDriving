using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public float idealRPM = 500f;
    public float maxRPM = 1000f;
    public float AntiRoll = 200000.0f;

    public Transform centerOfGravity;

    public WheelCollider frontl; //the front left tire
    public WheelCollider frontr; //the front right tire
    public WheelCollider backl; //...so on...
    public WheelCollider backr;

    public float turnRadius = 30f; //the circle in which the tires can turn
    public float torque = 2000f;
    public float brakeTorque = 1000f;

    public float wheel_mass = 120f;
    public float wheel_spring = 20000f;
    public float wheel_damper = 4000f;
    public float wheel_extremum = 8f;

    public float Speed()
    {
        return backr.radius * Mathf.PI * backr.rpm * .06f;
    }

    public float RPM()
    {
        return backl.rpm;
    }

    void DoRollBar(WheelCollider WheelL, WheelCollider WheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * AntiRoll;
    }

    // Use this for initialization
    void Start()
    {
        frontl.mass = wheel_mass;
        frontr.mass = wheel_mass;
        backl.mass = wheel_mass;
        backr.mass = wheel_mass;

        frontl.suspensionSpring.spring.Equals(wheel_spring);
        frontr.suspensionSpring.spring.Equals(wheel_spring);
        backl.suspensionSpring.spring.Equals(wheel_spring);
        backr.suspensionSpring.spring.Equals(wheel_spring);

        frontl.suspensionSpring.damper.Equals(wheel_damper);
        frontr.suspensionSpring.damper.Equals(wheel_damper);
        backl.suspensionSpring.damper.Equals(wheel_damper);
        backr.suspensionSpring.damper.Equals(wheel_damper);

        frontl.sidewaysFriction.extremumValue.Equals(wheel_extremum);
        frontr.sidewaysFriction.extremumValue.Equals(wheel_extremum);
        backl.sidewaysFriction.extremumValue.Equals(wheel_extremum);
        backr.sidewaysFriction.extremumValue.Equals(wheel_extremum);


        //note: change spring to ~20000 and damper to ~4000 and extremum value to 4 or 5

        this.GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float scaledTorque = -Input.GetAxis("Vertical") * torque;

        if (backl.rpm < idealRPM) //1 torque
            scaledTorque = Mathf.Lerp(scaledTorque / 10f, scaledTorque, backl.rpm / idealRPM);
        else //max speed, therefore 0 torque
            scaledTorque = Mathf.Lerp(scaledTorque, 0, (backl.rpm - idealRPM) / (maxRPM - idealRPM));

        frontl.steerAngle = Input.GetAxis("Horizontal") * turnRadius;
        frontr.steerAngle = Input.GetAxis("Horizontal") * turnRadius;

        DoRollBar(frontr, frontl);
        DoRollBar(backr, backl);

        frontr.motorTorque = scaledTorque;
        frontl.motorTorque = scaledTorque;
        backr.motorTorque = scaledTorque;
        backl.motorTorque = scaledTorque;

        if(Input.GetKey(KeyCode.S)) //apply the brakes
        {
            frontr.brakeTorque = brakeTorque; //note: the first brake torque the wheel collider
            frontl.brakeTorque = brakeTorque; //the second is a variable in this code
            backr.brakeTorque = brakeTorque;
            backl.brakeTorque = brakeTorque;
        }
        else //don't apply the brakes
        {
            frontr.brakeTorque = 0;
            frontl.brakeTorque = 0;
            backr.brakeTorque = 0;
            backl.brakeTorque = 0;
        }
    }
}
