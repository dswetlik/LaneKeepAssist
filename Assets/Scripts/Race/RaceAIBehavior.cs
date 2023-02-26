using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System.Linq;

public class RaceAIBehavior : MonoBehaviour
{

    ControllerManager _cm;

    [SerializeField] bool _shouldTurnLeft;
    [SerializeField] bool _shouldTurnRight;

    [SerializeField] bool _lka;
    [SerializeField] bool _ldw;

    // Start is called before the first frame update
    void Start()
    {
        _cm = GameObject.Find("GameManager").GetComponent<ControllerManager>();

        if (_lka)
            StartCoroutine(FollowNodes());
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

        if (_ldw)
        {
            if (_shouldTurnLeft || _shouldTurnRight)
                _cm.PulseController(true, ControllerManager.RumbleStrength.medium);
            else
                _cm.PulseController(false, ControllerManager.RumbleStrength.medium);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ldw)
        {
            if (other.tag.Equals("RightTurnTrigger"))
                _shouldTurnRight = true;
            if (other.tag.Equals("LeftTurnTrigger"))
                _shouldTurnLeft = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_ldw)
        {
            if (other.tag.Equals("RightTurnTrigger"))
                _shouldTurnRight = false;
            if (other.tag.Equals("LeftTurnTrigger"))
                _shouldTurnLeft = false;
        }
    }

    public IEnumerator FollowNodes()
    {

        SplineProjector _sp = GetComponent<SplineProjector>();

        while (_lka)
        {

            Vector3 toNav = _sp.EvaluatePosition((_sp.GetPercent() + 0.05) % 1.0);

            float dist = Vector3.Distance(transform.position, toNav);
            float ang = Vector3.SignedAngle(transform.forward, toNav - transform.position, Vector3.up);

            if (ang > 2.0f)
            {
                _shouldTurnRight = true;
                _shouldTurnLeft = false;
            }
            else if (ang < -2.0f)
            {
                _shouldTurnRight = false;
                _shouldTurnLeft = true;
            }
            else
            {
                _shouldTurnRight = false;
                _shouldTurnLeft = false;
            }

            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
