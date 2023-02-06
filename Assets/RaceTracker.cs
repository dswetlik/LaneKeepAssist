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

    // Start is called before the first frame update
    void Start()
    {
        data = new Data();
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

        if(other.tag.Equals("Checkpoint"))
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
                    data.RecordTime(0);
                    _lapCount++;
                    if (_lapCount > _maxLaps)
                    {
                        data.StopTime();
                        data.OutputData();
                    }
                }
            }
            else if (other.name.Equals("CheckpointA"))
            {
                if (_raceOngoing)
                {
                    data.RecordTime(1);
                }
            }
            else if (other.name.Equals("CheckpointB"))
            {
                if (_raceOngoing)
                {
                    data.RecordTime(2);
                }

            }
            else if (other.name.Equals("CheckpointC"))
            {
                if (_raceOngoing)
                {
                    data.RecordTime(3);
                }
            }
        }

        _isColliding = false;
    }
}
