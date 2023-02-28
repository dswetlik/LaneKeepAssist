using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    // Start is called before the first frame update
    void Start()
    {
        if (_collectData)
        {
            data = new Data(_trackScene.name, _raceNumber);
        }
        _isColliding = false;

        _distanceTravelled = 0;
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

    private void OnTriggerEnter(Collider other)
    {
        if(_collectData)
        {
            if(other.tag.Equals("LeftTurnTrigger") || other.tag.Equals("RightTurnTrigger"))
            {
                StartCoroutine(TrackCollision());
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

    IEnumerator TrackCollision()
    {
        yield return null;
    }

}

public class CollisionData
{

    public List<long> _times;
    public List<Vector3> _positions;

}
