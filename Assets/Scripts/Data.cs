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
    CollisionData _collisionData;
    RaceData _raceData;

    string BASE_PATH;

    public Data(string trackName, int raceNum)
    {
        _lapWatch = new Stopwatch();

        this.raceNum = raceNum;

        _lapRows = new List<LapData>();

        BASE_PATH = Application.dataPath + @"\Data\" + raceNum + "_" + trackName;
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

    public void ImportLapData(List<LapData> ld)
    {
        _lapRows = ld;
    }

    public void ImportCollisionData(CollisionData cd)
    {
        _collisionData = cd;
    }

    public void ImportRaceData(RaceData rd)
    {
        _raceData = rd;
    }

    public void OutputData()
    {

        string lapDataPath = BASE_PATH + @"_lap_data.csv";
        string collisionDataPath = BASE_PATH + @"_clsn_data.csv";
        string raceDataPath = BASE_PATH + @"_race_data.csv";

        OutputLapRow(lapDataPath);
        OutputCollisionRow(collisionDataPath);
        OutputRaceRow(raceDataPath);

    }

    void OutputLapRow(string path)
    {
        using (StreamWriter writer = File.AppendText(path))
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
        using (StreamWriter writer = File.AppendText(path))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("CollisionID,time,position");

            int count = Mathf.Max(_collisionData._times.Count, _collisionData._positions.Count);
            for (int i = 0; i < count; i++)
            {
                string collisionId = "", time = "", xposition = "", zposition = "";

                if (!(i > _collisionData._collisionId.Count))
                    collisionId = _collisionData._collisionId[i].ToString();

                if (!(i > _collisionData._times.Count))
                    time = _collisionData._times[i].ToString();

                if (!(i > _collisionData._positions.Count))
                {
                    xposition = _collisionData._positions[i].x.ToString();
                    zposition = _collisionData._positions[i].z.ToString();
                }

                writer.WriteLine(string.Format("{0},{1},{2},{3}", collisionId, time, xposition,zposition));
            }

        }
    }

    void OutputRaceRow(string path)
    {
        using(StreamWriter writer = File.AppendText(path))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("TimeStamp,xPosition,zPosition,DistanceFromLast,rtTriggerPull,ltTriggerPull");

            int count = Mathf.Max(_raceData._timeStamps.Count, _raceData._positions.Count, _raceData._distances.Count);
            for(int i = 0; i < count; i++)
            {
                string time = "", xposition = "", zposition = "", distance = "", rtPull = "", ltPull = "";

                if (!(i > _raceData._timeStamps.Count))
                    time = _raceData._timeStamps[i].ToString();

                if (!(i > _raceData._positions.Count))
                {
                    xposition = _raceData._positions[i].x.ToString();
                    zposition = _raceData._positions[i].z.ToString();
                }

                if (!(i > _raceData._distances.Count))
                    distance = _raceData._distances[i].ToString();

                if (!(i > _raceData._rtPull.Count))
                    rtPull = _raceData._rtPull[i].ToString();

                if (!(i > _raceData._ltPull.Count))
                    ltPull = _raceData._ltPull[i].ToString();


                writer.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}",time,xposition,zposition,distance,rtPull,ltPull));
            }
        }
    }

}

public class LapData
{

    public long LapTime { get; set; }
    public int CollisionCount { get; set; }

    public LapData(long lt, int cc)
    {
        LapTime = lt;
        CollisionCount = cc;
    }

}

public class RaceData
{

    public List<long> _timeStamps;
    public List<Vector3> _positions;
    public List<float> _distances;
    public List<float> _rtPull;
    public List<float> _ltPull;

}

public class CollisionData
{

    public List<int> _collisionId;
    public List<long> _times;
    public List<Vector3> _positions;

}