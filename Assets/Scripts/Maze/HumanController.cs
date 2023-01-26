using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{

    CharacterController _cc;

    [SerializeField] float _movementSpeed;
    [SerializeField] float _rotationSpeed;
   
    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (ControllerManager.DualSense.R1.isPressed)
            _cc.Move(transform.forward * Time.deltaTime * _movementSpeed);
        else if (ControllerManager.DualSense.L1.isPressed)
            _cc.Move(-transform.forward * Time.deltaTime * _movementSpeed);

        Vector3 rot = new Vector3(0, ControllerManager.DualSense.R2.ReadValue() - ControllerManager.DualSense.L2.ReadValue());
        transform.Rotate(rot * Time.deltaTime * (30 * _rotationSpeed));
    }
}
