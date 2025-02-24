using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


// Serialize : 인스펙터 노출을 위한 내부 작업들은 한다
[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;

    public override Object GetItem()
    {
        return collectable;
    }
}


public class CollectableManager : MonoBehaviour
{
    [Space(20)]
    public List<CollectablePool> collectablePools;


    [Space(20)]
    [SerializeField] float spawnZpos = 60f;
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;


    private TrackManager trackMgr;
    private RandomGenerator randomGenerator = new RandomGenerator();

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙관리자 없음");
            yield break;
        }
        
        foreach( var pool in collectablePools )
            randomGenerator.AddItem(pool);

        yield return new WaitUntil( ()=> GameManager.IsPlaying == true );

        StartCoroutine(InfiniteSpawn());        
    }


    // 장애물 생성 ( lane = 0,1,2 )
    public void SpawnCollectable()
    {
        (int lane, Collectable prefab) = RandomLanePrefab();
        
        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다        
        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }
        
        if (prefab != null)
        {
            Collectable o = Instantiate(prefab, t.CollectableRoot);
            o.SetLanePostion(lane, spawnZpos, trackMgr);
        }
    }


    IEnumerator InfiniteSpawn()
    {
        double lastMileage = 0f;
        while( true )
        {
            yield return new WaitUntil( ()=> GameManager.IsPlaying );

            if (GameManager.mileage - lastMileage > Random.Range(spawnInterval.x,spawnInterval.y))
            {
                SpawnCollectable();        

                lastMileage = GameManager.mileage;
            }
        }
    }


    (int, Collectable) RandomLanePrefab()
    {
        // 랜덤1 : Lane 을 랜덤 생성
        int rndLane = Random.Range(0,trackMgr.laneList.Count);
        Collectable prefab = randomGenerator.GetRandom().GetItem() as Collectable;

        if (prefab == null)
            return (-1, null);

        return (rndLane, prefab);
    }

}
