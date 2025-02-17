using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{

    [Space(20)]
    public Track trackPrefab;
    public PlayerControl playerPrefab;

    [Space(20)]
    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 100)] public int trackCount = 3;
    //public float trackThreshold = 10f; //트랙 삭제의 z축 간격 조건

    public Material CurvedMaterial;
    [Range(0f, 0.5f)] public float CurvedFrequencyX; // 주기
    [Range(0f, 12f)] public float CurvedAmplitudeX; //진폭
    [Range(0f, 0.5f)] public float CurvedFrequencyY; // 주기
    [Range(0f, 12f)] public float CurvedAmplitudeY; //진폭
    
    
    private List<Track> trackList = new List<Track>(); //생성한 트랙들 보관
    private Transform camTransform;
    [HideInInspector] public List<Transform> laneList; // 현재 트랙의 라인 정보를 전달
    
    // 캐시 데이터
    private int _curveAmount = Shader.PropertyToID("_CurveAmount");
    
    void Start()
    {
        //메인 카메라 Transform을 미리 받아온다
        camTransform = Camera.main.transform;
        
        SpawnInitialTrack();
        SpawnPlayer();
    }
    
    void Update()
    {
        RepositionTrack();
        CurveTrack();
        
    }

    //초기 트랙 생성( 한번만 실행)
    void SpawnInitialTrack()
    {
        // 초기값: 카메라의 z좌표
        Vector3 position = new Vector3(0f, 0f, camTransform.position.z);
        for(int i = 0 ; i<trackCount-1; i++)
        {
            //이전 ExitPoint에 다음 EntryPoint 접합
            Track next = SpawnNextTrack(position, $"Track _{i}");
            position = next.ExitPoint.position;            
        }
        
    }

    Track SpawnNextTrack(Vector3 position, string trackname)
    {
        //첫번째 ExitPoint에 두번째 EntryPoint 접합
        Track Next = Instantiate(trackPrefab, position, Quaternion.identity, transform);
        Next.name = trackname;
        Next.trackmgr = this;
        laneList = Next.laneList;
        trackList.Add(Next);

        return Next;
    }

    //트랙을 재배치
    void RepositionTrack()
    {
        if (trackList.Count <= 0) return;
        // 언제 재배치하냐 ? Z축 < 0f -> 삭제 -> 리스트 마지막에 생성
        // Track_0 => tracks[0]
        // trackList[0] => 트랙의 첫번째 값 가져오기
        // trackList[trackList.Count-1] => 트랙의 마지막 값 가져오기
        if (trackList[0].ExitPoint.position.z < camTransform.position.z)
        {
            Track lastTrack = trackList[trackList.Count-1];
            SpawnNextTrack(lastTrack.ExitPoint.position, trackList[0].name);  
            
            Destroy(trackList[0].gameObject);
            trackList.RemoveAt(0);
        }    
    }

    void CurveTrack()
    {
        // 반환 값이 0f ~ 1f => -1f ~ 1f;
        float rndX = Mathf.PerlinNoise1D(Time.time * CurvedFrequencyX) * 2f -1f;
        rndX = rndX * CurvedAmplitudeX;
        float rndY = Mathf.PerlinNoise1D(Time.time * CurvedFrequencyY) * 2f -1f;
        rndY = rndY * CurvedAmplitudeY;

        
        CurvedMaterial.SetVector(_curveAmount, new Vector4(rndX, rndY, 0f, 0f)); 
    }

    void SpawnPlayer()
    {
        PlayerControl player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.trackMgr = this;
    }

}
