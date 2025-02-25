using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();

    private int limitQuota = 10;
    private int _currentQuota;

    private int laneCount;

    [HideInInspector] public Lane currentPattern;

    // 생성자 (Construct) : 클래스 최초 호출
    public LaneGenerator(int lanecount, int quota)
    {
        
        laneCount = lanecount;
        limitQuota = quota;
        
        // Factory Pattern
        lanePatterns.Add( new LaneStraight() );
        lanePatterns.Add( new LaneWave() );

        currentPattern = lanePatterns[0];
    }

    public int GetNextLane()
    {
        if ( currentPattern == null)
            return -1;

        _currentQuota++;

        if (_currentQuota >= limitQuota)
            SwitchPattern();

        return currentPattern.GetNextLane();
    }

    public void SwitchPattern(int index = -1)
    {        
        int i = index == -1 ? Random.Range(0, lanePatterns.Count ) : Mathf.Clamp(index, 0, lanePatterns.Count-1);

        Lane lanepattern = lanePatterns[i];
        currentPattern = lanepattern;
        currentPattern.Initialize(laneCount);

        _currentQuota = 0;
    }

}
