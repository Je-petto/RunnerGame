using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{

    public Track trackPrefab;
    public PlayerControl playerPrefab;

    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 100)] public int trackCount = 3;
    //public float trackThreshold = 10f; //트랙 삭제의 z축 간격 조건
    
    
    private List<Track> trackList = new List<Track>(); //생성한 트랙들 보관
    private Transform camTransform;
    [HideInInspector] public List<Transform> laneList; // 현재 트랙의 라인 정보를 전달
    
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

    void SpawnPlayer()
    {
        PlayerControl player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.trackMgr = this;
    }

}
