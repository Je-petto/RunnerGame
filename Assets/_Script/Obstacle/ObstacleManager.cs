using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum : Enumerator
public enum ObstacleType { Single, Top, Bottom, _MAX_}

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<Obstacle> obstacleSingle;
    [SerializeField] List<Obstacle> obstacleTop;
    [SerializeField] List<Obstacle> obstacleBottom;

    [Space(20)]
    [SerializeField] Obstacle obstaclePrefab;
    [SerializeField] Transform spawnPoint;

    [SerializeField] float spawnInterval = 1f;



    private TrackManager trackMgr;


    // Coroutine 방식 : Function, Method, Subroutine
    IEnumerator Start()
    {
        TrackManager[] tm = FindObjectsByType<TrackManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if ( tm == null || tm.Length <= 0 )
        {
            Debug.LogError($"트랙관리자 없음");
            yield break;
        }

        trackMgr = tm[0];

        //yield return new WaitForEndOfFrame();  // 지연 : 1프레임만 지연
        //yield return new WaitForSeconds(2f);   // 지연 : 2초 지연
        yield return new WaitUntil( ()=> GameManager.IsPlaying == true );

        StartCoroutine(InfiniteSpawn());        
    }


    // 장애물 생성 ( lane = 0,1,2 )
    public void SpawnObstacle(int lane)
    {
        // Lane 위치
        lane = Mathf.Clamp(lane, 0, trackMgr.laneList.Count-1);                
        Transform laneTransform = trackMgr.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, spawnPoint.position.z );
        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다
        // 현재 해당 트랙 ? 
        Track t = trackMgr.GetTrackByZ(spawnPoint.position.z);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        var obstacle = RandomTypeSpawn();

        Instantiate(obstacle, pos, Quaternion.identity, t.ObstacleRoot );
    }


    IEnumerator InfiniteSpawn()
    {
        while( true )
        {
            yield return new WaitUntil( ()=> GameManager.IsPlaying == true);
            // if (GameManager.IsPlaying == false) -> false를 활용한 방식
            //     yield break;

            SpawnObstacle(Random.Range(0,trackMgr.laneList.Count));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Obstacle RandomTypeSpawn()
    {
        // 랜덤: ObstacleType
        int rndType = Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX_);

        List<Obstacle> obstacles = rndType switch
        {
            (int)ObstacleType.Single => obstacleSingle,
            (int)ObstacleType.Top => obstacleTop,
            (int)ObstacleType.Bottom => obstacleBottom,
            _=> null
        };

        // 랜덤: varient
        Obstacle prefab = obstacles[Random.Range(0, obstacles.Count)];

        return prefab;
    }


}