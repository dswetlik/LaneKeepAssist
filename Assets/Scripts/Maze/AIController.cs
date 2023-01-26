using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Polarith.AI.Move;

public class AIController : MonoBehaviour
{

    //AIMContext AIMC;

    public List<GameObject> _markers;
    [SerializeField] private GameObject _currentlyTrackedMarker;
    private int _trackedMarkerIndex;

    [SerializeField] private bool _steerToSpeaker;
    [SerializeField] private bool _steerToNextMarker;

    [SerializeField] bool _flipAngles;

    // Start is called before the first frame update
    void Start()
    {
        _currentlyTrackedMarker = _markers[0];
        _trackedMarkerIndex = 0;

        _steerToSpeaker = false;
        _steerToNextMarker = false;
    }

    private void Update()
    {
        if (_currentlyTrackedMarker != null)
        {

            Debug.DrawRay(transform.position, transform.forward, Color.red);
            Debug.DrawRay(transform.position, _currentlyTrackedMarker.transform.position - transform.position, Color.red);

            if (_steerToSpeaker)
            {

                float distance = Vector3.Distance(transform.position, _currentlyTrackedMarker.transform.position);
                float angle = Vector3.SignedAngle(transform.forward, _currentlyTrackedMarker.transform.position - transform.position, Vector3.up);

                if(distance < 1f)
                    SetTriggersOnAngle(ControllerManager.TriggerHapticStrength.high, angle);
                else if (distance < 2.0f)
                    SetTriggersOnAngle(ControllerManager.TriggerHapticStrength.medium, angle);
                else if (distance < 3.5f)
                    SetTriggersOnAngle(ControllerManager.TriggerHapticStrength.low, angle);
                else
                    SetTriggersOnAngle(ControllerManager.TriggerHapticStrength.off, angle);

                /*
                // returns true if there is an intersection between X and Y on layers Default and Marker
                if (Physics.Linecast(transform.position, _currentlyTrackedMarker.transform.position - transform.position, out RaycastHit hit, 9)) // 00001001
                {
                    Debug.Log(string.Format("Intersection on {0} at [{1},{2},{3}]", hit.collider.name, hit.transform.position.x, hit.transform.position.y, hit.transform.position.z));
                }
                else
                {
                    float angle = Vector3.SignedAngle(transform.forward, _currentlyTrackedMarker.transform.position - transform.position, Vector3.up);

                    Debug.Log("No intersection, steering at " + angle);
                    if (angle < 0)
                    {
                        GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.medium);
                    }
                    else if (angle > 0)
                    {
                        GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.medium, ControllerManager.TriggerHapticStrength.off);
                    }
                    else
                    {
                        GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.off);
                    }
                }
                */
            }
            if (_steerToNextMarker)
            {
                float angle = Vector3.SignedAngle(transform.forward, _currentlyTrackedMarker.transform.position - transform.position, Vector3.up);
                SetTriggersOnAngle(ControllerManager.TriggerHapticStrength.high, angle);
            }
        }
    }



    private void OnApplicationQuit()
    {
        GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.off);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentlyTrackedMarker != null)
        {
            if (other.tag.Equals("Speaker"))
            {
                _steerToNextMarker = false;

                if (other.transform.parent.gameObject == _currentlyTrackedMarker && !_currentlyTrackedMarker.GetComponent<Marker>().HasBeenVisited)
                {
                    _steerToSpeaker = true;
                }
            }
            else
                _steerToSpeaker = false;

            if (other.tag.Equals("Touch"))
            {
                _steerToSpeaker = false;

                if(other.transform.parent.gameObject == _currentlyTrackedMarker)
                {
                    _currentlyTrackedMarker.GetComponent<Marker>().VisitedMarker();
                    _currentlyTrackedMarker = GetNextMarker();
                    if (_currentlyTrackedMarker == null)
                        return;

                    _steerToNextMarker = true;
                }

            }
        }
        else
        {
            _steerToSpeaker = false;
            _steerToNextMarker = false;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Touch") || other.tag.Equals("Speaker"))
        {
            GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.off);

            if (other.tag.Equals("Touch"))
                _steerToNextMarker = false;
            else if (other.tag.Equals("Speaker"))
                _steerToSpeaker = false;
        }
    }

    GameObject GetNextMarker()
    {
        if (_trackedMarkerIndex < _markers.Count - 1)
        {
            _trackedMarkerIndex++;
            return _markers[_trackedMarkerIndex];
        }
        else
            return null;

    }

    void SetTriggersOnAngle(ControllerManager.TriggerHapticStrength triggerHapticStrength, float angle)
    {
        if (_flipAngles)
        {
            if (angle < 0)
            {
                GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(triggerHapticStrength, ControllerManager.TriggerHapticStrength.off);
            }
            else if (angle > 0)
            {
                GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, triggerHapticStrength);
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.off);
            }
        }
        else
        {
            if (angle < 0)
            {
                GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, triggerHapticStrength);
            }
            else if (angle > 0)
            {
                GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(triggerHapticStrength, ControllerManager.TriggerHapticStrength.off);
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<ControllerManager>().SetTriggerHaptics(ControllerManager.TriggerHapticStrength.off, ControllerManager.TriggerHapticStrength.off);
            }
        }
    }

}
