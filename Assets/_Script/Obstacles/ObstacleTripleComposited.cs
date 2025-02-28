using System.Collections.Generic;
using UnityEngine;


public class ObstacleTripleComposited : ObstacleTriple
{
    // 프리팹을 모아놓은 리스트
    [SerializeField] List<ObstacleSingle> compositedPrefabs;
    // 프리팹 중에 SingleType이 None인 것만 모아놓은 리스트
    private List<ObstacleSingle> nonePrefabs;
    //생성할 레인 위치 (3Lane => 3개)
    private List<Vector3> spawnedPos = new List<Vector3>();

    private void Start()
    {
        //None 블록 형태만 찾아 놓는다.
        nonePrefabs = compositedPrefabs.FindAll( f => f.singletype == ObstacleSingle.SingleType.NONE );
        
        SpawnComposited();
    }

    private void SpawnComposited()
    {        
        // 3개의 싱글 장애물 스폰
        // spawnedPos (0번,1번,2번) 위치에 생성
        
        int blocked = 0;
        
        foreach( var p in spawnedPos)
        {
            ObstacleSingle prefab = GetRandomPrefab(compositedPrefabs);

            // 랜덤으로 가져온 프래팹이 Block 이면, Blocked 카운트 증가
            if (prefab.singletype == ObstacleSingle.SingleType.BLOCK)
                if (++blocked > 2)
                    prefab = GetRandomPrefab(nonePrefabs);
            
            Spawn(prefab, p);
        };
    }

    private ObstacleSingle GetRandomPrefab(List<ObstacleSingle> prefabs)
    {
        int rnd = Random.Range(0,  prefabs.Count);
        ObstacleSingle prefab =  prefabs[rnd] as ObstacleSingle;

        return prefab;
    }

    private void Spawn(Obstacle prefab, Vector3 pos)
    {
        var o = Instantiate(prefab, pos, Quaternion.identity, transform);
            Vector3 localpos = o.transform.localPosition;
            o.transform.localPosition = new Vector3(localpos.x, 0f, 0f);
    }

    public override void SetLanePostion(int lane, float zpos, TrackManager tm)
    {
        spawnedPos.Clear();   

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);        
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        spawnedPos.Add(lanepos0);
        spawnedPos.Add(lanepos1);
        spawnedPos.Add(lanepos2);        
                
        // 위치와 회전 설정
        Vector3 pos = new Vector3(lanepos1.x, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}
