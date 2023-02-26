using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTracker : MonoBehaviour
{

    Data data;

    bool _raceOngoing;
    bool _isColliding;

    [SerializeField] int _maxLaps;
    [SerializeField] int _lapCount;

    [SerializeField] bool _collectData;

    public GameObject finishTextObject;

    // Start is called before the first frame update
    void Start()
    {
        if (_collectData)
        {
            data = new Data();
        }
        _isColliding = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (_isColliding)
            return;
        _isColliding = true;

        if (_collectData)
        {
            if (other.tag.Equals("Checkpoint"))
            {
                if (other.name.Equals("Start"))
                {
                    if (!_raceOngoing)
                    {
                        data.StartTime();
                        _raceOngoing = true;
                    }
                    else
                    {
                        data.RecordTime();
                        _lapCount++;
                        if (_lapCount > _maxLaps)
                        {
                            data.StopTime();
                            data.OutputData();
                            finishTextObject.SetActive(true);
                        }
                    }
                }
            }

            _isColliding = false;
        }
    }
}
