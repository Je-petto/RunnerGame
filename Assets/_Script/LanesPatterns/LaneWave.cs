using UnityEngine;

public class LaneWave : Lane
{
    public float amplitude = 3f; //진폭(Amplitude)
    public float frequency = 1f; //주기(Frequency)
    public string Name => "WavePattern";

    private int currentLane;

    // 외부 노출용
    public int MaxLane { get { return _maxLane;} set {_maxLane = value ; }}
    // 데이터 보관용
    private int _maxLane;

    public void Initialize(int maxlane)
    {
        currentLane = Random.Range(0, maxlane);
    }

    public int GetNextLane()
    {
        float t = Time.time;
        GameManager.waveValue = Mathf.Abs(Mathf.Sin(t * Mathf.PI * frequency)) * amplitude;
        
        return -1;
    }

    // void OnDrawGizmos()
    // {

    //     Gizmos.color = Color.yellow;

    //     // count만큼 배치 => 반복문 사용
    //     for ( int i=0 ; i<count; i++ )
    //     { 
    //         float t = (float)i/(count-1);
    //         Vector3 v = Vector3.Lerp(transform.position, transform.position + (transform.forward *  offsetZ), t );
    //         // PI(3.14) => 180도, 2PI => 360도
    //         // Sin => -1f ~ 1f => 음수를 양수로 전환해야 함
    //         float s = Mathf.Abs(Mathf.Sin(t * Mathf.PI * frequency)) * amplitude;          
    //         v = new Vector3(v.x, v.y + s, v.z);
    //         Gizmos.DrawCube(v, Vector3.one*0.5f);
    //     }
    // }
}
