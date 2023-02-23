using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Globalization;

public class Data
{

    Stopwatch _lapWatch;

    List<long> _lapTime;
    List<long> _aTime;
    List<long> _bTime;
    List<long> _cTime;

    public Data()
    {
        _lapWatch = new Stopwatch();

        _lapTime = new List<long>();

        _aTime = new List<long>();
        _bTime = new List<long>();
        _cTime = new List<long>();
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

    public void RecordTime(int timer)
    {
        switch (timer)
        {
            case 0:
                _lapTime.Add(_lapWatch.ElapsedMilliseconds);
                ResetTime();
                break;
            case 1:
                _aTime.Add(_lapWatch.ElapsedMilliseconds);
                break;
            case 2:
                _bTime.Add(_lapWatch.ElapsedMilliseconds);
                break;
            case 3:
                _cTime.Add(_lapWatch.ElapsedMilliseconds);
                break;
        }


    }

    public void OutputData()
    {
        List<Row> rows = new List<Row>();
        for (int i = 0; i < _lapTime.Count; i++)
            rows.Add(new Row {
                LapTime = _lapTime[i],
                CheckpointATime = _aTime[i],
                CheckpointBTime = _bTime[i],
                CheckpointCTime = _cTime[i]
            });

        using (StreamWriter writer = new StreamWriter(Application.dataPath + @"\data.csv"))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("LapTime,CheckpointATime,CheckpointBTime,CheckpointCTime");
            foreach(Row row in rows)
            {
                writer.WriteLine(string.Format("{0},{1},{2},{3}",
                    row.LapTime, row.CheckpointATime, row.CheckpointBTime, row.CheckpointCTime));
            }
        }
    }

}

public class Row
{
    public long LapTime { get; set; }
    public long CheckpointATime { get; set; }
    public long CheckpointBTime { get; set; }
    public long CheckpointCTime { get; set; }
}