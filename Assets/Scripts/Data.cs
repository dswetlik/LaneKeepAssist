using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Globalization;

public class Data
{

    Stopwatch _lapWatch;

    List<long> _lapTimes;
    List<Vector3> _worldPositions;


    public Data()
    {
        _lapWatch = new Stopwatch();

        _lapTimes = new List<long>();
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

    public void ResetTime()
    {
        _lapWatch.Restart();
    }

    public void RecordTime()
    {
        _lapTimes.Add(_lapWatch.ElapsedMilliseconds);
        ResetTime();
    }

    public void OutputData()
    {
        List<Row> rows = new List<Row>();
        for (int i = 0; i < _lapTimes.Count; i++)
            rows.Add(new Row {
                LapTime = _lapTimes[i]
            });

        using (StreamWriter writer = new StreamWriter(Application.dataPath + @"\Data\data.csv"))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("LapTime,CheckpointATime,CheckpointBTime,CheckpointCTime");
            foreach(Row row in rows)
            {
                writer.WriteLine(string.Format("{0}", row.LapTime));
            }
        }
    }

}

public class Row
{
    public long LapTime { get; set; }

}