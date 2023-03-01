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

    // DISTANCE DATA OBJECTS
    float _distanceTravelled;
    Vector3 _lastPosition;

    // COLLISION DATA OBJECTS
    bool _continueCollisionTracking;
    List<CollisionData> _collisionData;
    int _currentLapCollisionCount;

    // Start is called before the first frame update
    void Start()
    {
        if (_collectData)
        {
            data = new Data(_trackScene.name, _raceNumber);
        }
        _isColliding = false;

        _distanceTravelled = 0;
        _collisionData = new List<CollisionData>();

        _continueCollisionTracking = false;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (_raceOngoing)
        {
            if (_lastPosition == null)
                _lastPosition = transform.position;
            else
            {
                _distanceTravelled += Vector3.Distance(transform.position, _lastPosition);
                _lastPosition = transform.position;
            }
        }
    }

    void EndLap()
    {
        data.RecordLap(data.GetTime(), _currentLapCollisionCount);
        _currentLapCollisionCount = 0;
        data.RestartTime();

        _lapCount++;
        if (_lapCount > _maxLaps)
        {
            data.StopTime();

            data.ImportCollisionData(_collisionData);

            data.OutputData();
            finishTextObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_collectData)
        {
            if(other.tag.Equals("LeftTurnTrigger") || other.tag.Equals("RightTurnTrigger"))
            {
                if (!_continueCollisionTracking)
                {
                    StartCoroutine(TrackCollision());
                    _continueCollisionTracking = true;
                    _currentLapCollisionCount++;
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
                if (_continueCollisionTracking)
                {
                    _continueCollisionTracking = false;
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

    IEnumerator TrackCollision()
    {

        CollisionData cd = new CollisionData();

        while (_continueCollisionTracking)
        {
            cd._times.Add(data.GetTime());
            cd._positions.Add(transform.position);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        _collisionData.Add(cd);

    }

}
