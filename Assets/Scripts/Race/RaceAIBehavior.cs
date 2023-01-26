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
    void Update()
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("InnerCollider"))
        {
            _shouldTurnLeft = true;
        }
        
        if(other.name.Equals("OuterCollider"))
        {
            _shouldTurnRight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("InnerCollider"))
        {
            _shouldTurnLeft = false;
        }

        if (other.name.Equals("OuterCollider"))
        {
            _shouldTurnRight = true;
        }
    }


}
