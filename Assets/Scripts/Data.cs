using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Globalization;

public class Data
{

    string trackName;
    int raceNum;

    Stopwatch _lapWatch;

    List<Vector3> _worldPositions;
    List<LapData> _lapRows;
    List<CollisionData> _collisionData;
    RaceData _raceData;

    string BASE_PATH;

    public Data(string trackName, int raceNum)
    {
        _lapWatch = new Stopwatch();

        this.raceNum = raceNum;

        _lapRows = new List<LapData>();

        BASE_PATH = Application.dataPath + @"\Data\" + raceNum;
    }

    public void StartTime()
    {
        if(!_lapWatch.IsRunning)
            _lapWatch.Start();
    }

    public void StopTime()
    {
        if (_lapWatch.IsRunning)
            _lapWatch.Stop();
    }

    public void RestartTime()
    {
        _lapWatch.Restart();
    }

    public long GetTime()
    {
        return _lapWatch.ElapsedMilliseconds;
    }

    public void RecordLap(long time, int collisionCount)
    {
        _lapRows.Add(new LapData
        {
            LapTime = time,
            CollisionCount = collisionCount
        });
    }

    public void ImportCollisionData(List<CollisionData> cd)
    {
        _collisionData = cd;
    }

    public void ImportRaceData(RaceData rd)
    {

    }

    public void OutputData()
    {

        string lapDataPath = BASE_PATH + @"\Lap\" + trackName + @"_lap_data.csv";
        string collisionDataPath = BASE_PATH + @"\Collisions\" + trackName + @"_clsn_data.csv";

        OutputLapRow(lapDataPath);
        OutputCollisionRow(collisionDataPath);

    }

    void OutputLapRow(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("LapTime,CollisionCount");
            foreach (LapData row in _lapRows)
            {
                writer.WriteLine(string.Format("{0},{1}", row.LapTime, row.CollisionCount));
            }
        }
    }

    void OutputCollisionRow(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("CollisionID,time,position");

            for(int i = 0; i < _collisionData.Count; i++)
            {
                int count = Mathf.Max(_collisionData[i]._times.Count, _collisionData[i]._positions.Count);
                for (int j = 0; j < count; j++)
                {
                    string time = "", position = "";

                    if(!(j > _collisionData[i]._times.Count))
                        time = _collisionData[i]._times[j].ToString();

                    if(!(j > _collisionData[i]._positions.Count))
                        position = _collisionData[i]._positions[j].ToString();

                    writer.WriteLine(string.Format("{0},{1},{2}",i,time,position));
                }
            }
        }
    }

}

public class LapData
{

    public long LapTime { get; set; }
    public int CollisionCount { get; set; }

}

public class RaceData
{

    public List<long> _timeStamps;
    public List<Vector3> _positions;
    public List<float> _distances;

}

public class CollisionData
{

    public List<long> _times;
    public List<Vector3> _positions;

}