using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;


public class RaceTracker : MonoBehaviour
{

    Data data;

    bool _raceOngoing;
    bool _isColliding;

    [SerializeField] Scene _trackScene;
    [SerializeField] int _raceNumber;

    [SerializeField] int _maxLaps;
    [SerializeField] int _lapCount;

    [SerializeField] bool _collectData;
  
    public GameObject finishTextObject;

    // RACE DATA OBJECTS
    RaceData _rd;
    Vector3 _lastPosition;

    // COLLISION DATA OBJECTS
    bool _inBorderTrigger;
    CollisionData _cd;
    int _currentLapCollisionCount;
    int _totalBorderTriggerCount;

    // LAP DATA OBJECT
    List<LapData> _lapData;

    // Start is called before the first frame update
    void Start()
    {
        _trackScene = SceneManager.GetActiveScene();

        if (_collectData)
        {
            data = new Data(_trackScene.name, _raceNumber);

            _isColliding = false;

            _rd = new RaceData();

            _rd._timeStamps = new List<long>();
            _rd._positions = new List<Vector3>();
            _rd._distances = new List<float>();
            _rd._rtPull = new List<float>();
            _rd._ltPull = new List<float>();

            _cd = new CollisionData();

            _cd._collisionId = new List<int>();
            _cd._positions = new List<Vector3>();
            _cd._times = new List<long>();

            _inBorderTrigger = false;
            _lastPosition = transform.position;
            _currentLapCollisionCount = 0;
            _totalBorderTriggerCount = 0;

            _lapData = new List<LapData>();
        }
    }

    void FixedUpdate()
    {
        if (_collectData)
        {
            if (_raceOngoing)
            {
                Vector3 pos = transform.position;
                long time = data.GetTime();

                _rd._timeStamps.Add(time);
                _rd._positions.Add(pos);
                _rd._distances.Add(Vector3.Distance(pos, _lastPosition));
                _rd._rtPull.Add(ControllerManager.DualSense.rightTrigger.ReadValue());
                _rd._ltPull.Add(ControllerManager.DualSense.leftTrigger.ReadValue());

                if(_inBorderTrigger)
                {
                    _cd._collisionId.Add(_totalBorderTriggerCount);
                    _cd._times.Add(time);
                    _cd._positions.Add(pos);
                }

                _lastPosition = transform.position;
            }
        }
    }

    void EndLap()
    {

        _lapData.Add(new LapData(data.GetTime(), _currentLapCollisionCount));

        _currentLapCollisionCount = 0;
        data.RestartTime();

        _lapCount++;

        if (_lapCount > _maxLaps)
        {

            data.StopTime();

            _raceOngoing = false;
            finishTextObject.SetActive(true);

            data.ImportLapData(_lapData);
            data.ImportCollisionData(_cd);
            data.ImportRaceData(_rd);

            data.OutputData();

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(_collectData)
        {
            if(other.tag.Equals("LeftTurnTrigger") || other.tag.Equals("RightTurnTrigger"))
            {
                if (!_inBorderTrigger)
                {
                    _inBorderTrigger = true;
                    _currentLapCollisionCount++;
                    _totalBorderTriggerCount++;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (_isColliding)
            return;
        _isColliding = true;

        if (_collectData)
        {
            if (other.tag.Equals("LeftTurnTrigger") || other.tag.Equals("RightTurnTrigger"))
            {
                if (_inBorderTrigger)
                {
                    _inBorderTrigger = false;
                }
            }

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
                        EndLap();
                    }
                }
            }
            _isColliding = false;
        }
    }
}
