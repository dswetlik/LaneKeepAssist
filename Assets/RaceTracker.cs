using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Dreamteck.Splines;

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

    SplineProjector _sp;

    // LAP DATA OBJECTS
    float distanceTravelled;
    int degCount;
    float totalDegOff;

    // RACE DATA OBJECTS
    RaceData _rd;
    Vector3 _lastPosition;

    // COLLISION DATA OBJECTS
    bool _continueCollisionTracking;
    List<CollisionData> _collisionData;
    int _currentLapCollisionCount;

    // Start is called before the first frame update
    void Start()
    {
        _sp = GetComponent<SplineProjector>();
        _trackScene = SceneManager.GetActiveScene();

        distanceTravelled = 0;
        degCount = 0;
        totalDegOff = 0;

        if (_collectData)
        {
            data = new Data(_trackScene.name, _raceNumber);
        }
        _isColliding = false;

        _rd = new RaceData();

        _rd._timeStamps = new List<long>();
        _rd._positions = new List<Vector3>();
        _rd._distances = new List<float>();

        _collisionData = new List<CollisionData>();

        _continueCollisionTracking = false;
        _lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (_collectData)
        {

            float dis = Vector3.Distance(transform.position, _lastPosition);

            Vector3 toNav = _sp.EvaluatePosition((_sp.GetPercent() + 0.05) % 1.0);
            float ang = Vector3.SignedAngle(transform.forward, toNav - transform.position, Vector3.up);

            _rd._timeStamps.Add(data.GetTime());
            _rd._positions.Add(transform.position);
            _rd._distances.Add(dis);
            distanceTravelled += dis;

            totalDegOff += ang;
            degCount++;

            _lastPosition = transform.position;


        }
    }

    void EndLap()
    {
        data.RecordLap(data.GetTime(), _currentLapCollisionCount, distanceTravelled, totalDegOff / degCount);
        
        distanceTravelled = 0;
        totalDegOff = 0;
        degCount = 0;

        _currentLapCollisionCount = 0;
        data.RestartTime();

        _lapCount++;
        if (_lapCount > _maxLaps)
        {
            data.StopTime();

            data.ImportCollisionData(_collisionData);
            data.ImportRaceData(_rd);

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

        cd._times = new List<long>();
        cd._positions = new List<Vector3>();

        while (_continueCollisionTracking)
        {
            cd._times.Add(data.GetTime());
            cd._positions.Add(transform.position);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        _collisionData.Add(cd);

    }

}
