using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceAIBehavior : MonoBehaviour
{

    ControllerManager _cm;

    [SerializeField] bool _shouldTurnLeft;
    [SerializeField] bool _shouldTurnRight;


    // Start is called before the first frame update
    void Start()
    {
        _cm = GameObject.Find("GameManager").GetComponent<ControllerManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_shouldTurnLeft)
        {
            _cm.SetTriggerHaptics(ControllerManager.TriggerHapticStrength.high, ControllerManager.TriggerHapticStrength.ignore);
        }
        else
        {
            _cm.SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.ignore);
        }

        if (_shouldTurnRight)
        {
            _cm.SetTriggerHaptics(ControllerManager.TriggerHapticStrength.ignore, ControllerManager.TriggerHapticStrength.high);
        }
        else
        {
            _cm.SetTriggerHaptics(ControllerManager.TriggerHapticStrength.ignore, ControllerManager.TriggerHapticStrength.off);
        }


        if (_shouldTurnLeft || _shouldTurnRight)
            _cm.PulseController(true, ControllerManager.RumbleStrength.medium);
        else
            _cm.PulseController(false, ControllerManager.RumbleStrength.medium);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("RightTurnTrigger"))
            _shouldTurnRight = true;
        if (other.tag.Equals("LeftTurnTrigger"))
            _shouldTurnLeft = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("RightTurnTrigger"))
            _shouldTurnRight = false;
        if (other.tag.Equals("LeftTurnTrigger"))
            _shouldTurnLeft = false;
    }


}
