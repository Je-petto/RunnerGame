using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform EntryPoint;
    public Transform ExitPoint;

    [HideInInspector] public TrackManager trackmgr;
    
    void LateUpdate()
    {
        Scroll();
    }

    void Scroll()
    {
        if (trackmgr == null) return;
        transform.position += Vector3.back * trackmgr.scrollSpeed * Time.smoothDeltaTime;

        //Time.deltaTime => 매프레임당 1번 호출 될때 간격(Interval Time)
        //Time.fixedDeltaTime => 0.02 간격(Time.deltaTime보다 빠름) - 물리엔진 관련해서 사용
        //Time.smoothDeltaTime => Time.deltaTime의 평균 => 값이 고르게 나오게 함

        // 속도: fixedDeltaTime < DeltaTime < smoothDeltaTime
    }
}
