using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int MaxThrottle = 10;
    public float SmoothMovement = 1f;
    public float SmoothTurning = 60f;

    private float leftThrottleValue = 0f;
    private float rightThrottleValue = 0f;
    private Rigidbody tankRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        tankRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        leftThrottleValue = ControllerManager.DualSense.L2.ReadValue();
        leftThrottleValue = (leftThrottleValue > MaxThrottle) ? MaxThrottle : leftThrottleValue;
        float roundedLeftThrottle = Mathf.Round(leftThrottleValue);
        if (Mathf.Abs(roundedLeftThrottle - leftThrottleValue) < 0.02f)
        {
            leftThrottleValue = roundedLeftThrottle;
        }
        else if (roundedLeftThrottle > leftThrottleValue)
        {
            leftThrottleValue += 0.01f;
        }
        else if (roundedLeftThrottle < leftThrottleValue)
        {
            leftThrottleValue -= 0.01f;
        }

        rightThrottleValue = ControllerManager.DualSense.R2.ReadValue();
        rightThrottleValue = (rightThrottleValue > MaxThrottle) ? MaxThrottle : rightThrottleValue;
        float roundedRightThrottle = Mathf.Round(rightThrottleValue);
        if (Mathf.Abs(roundedRightThrottle - rightThrottleValue) < 0.02f)
        {
            rightThrottleValue = roundedRightThrottle;
        }
        else if (roundedRightThrottle > rightThrottleValue)
        {
            rightThrottleValue += 0.01f;
        }
        else if (roundedRightThrottle < rightThrottleValue)
        {
            rightThrottleValue -= 0.01f;
        }

        // Only move the tank if there is both a left and right throttle, should allow for Zero turn
        if (leftThrottleValue > float.Epsilon && rightThrottleValue > float.Epsilon)
        {
            // Move the tank.
            Vector3 movement = transform.forward * ((leftThrottleValue + rightThrottleValue) / 2f) * SmoothMovement * Time.deltaTime;
            tankRigidbody.MovePosition(tankRigidbody.position + movement);
        }

        // Turn the tank.
        float turn = (leftThrottleValue - rightThrottleValue) * SmoothTurning * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        tankRigidbody.MoveRotation(tankRigidbody.rotation * turnRotation);
    }
}