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

    List<long> _lapTimes;
    List<Vector3> _worldPositions;


    public Data(string trackName, int raceNum)
    {
        _lapWatch = new Stopwatch();

        this.raceNum = raceNum;

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

        string lapDataPath = Application.dataPath + @"\Data\Lap\" + trackName + @"_lap_data_" + raceNum + @".csv";

        OutputLapRow(lapDataPath);

    }

    void OutputLapRow(string path)
    {
        List<LapRow> rows = new List<LapRow>();
        for (int i = 0; i < _lapTimes.Count; i++)
            rows.Add(new LapRow
            {
                LapTime = _lapTimes[i]
            });

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("sep=,");
            writer.WriteLine("LapTime");
            foreach (LapRow row in rows)
            {
                writer.WriteLine(string.Format("{0}", row.LapTime));
            }
        }
    }

}

public class LapRow
{
    public long LapTime { get; set; }
    public int CollisionCount { get; set; }

}