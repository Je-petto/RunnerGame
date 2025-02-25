using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;

public class CollectableCoin : Collectable
{
    [SerializeField] Transform pivot;
    [SerializeField] MMF_Player feedbackDisappear;
    
    // 해당 코인 증가량
    public uint Add = 1;

    public override void SetLanePostion(int lane, float ypos, float zpos, TrackManager tm)
    {
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);                
        Transform laneTransform = tm.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, ypos, zpos );

        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public override void Collect()
    {
        GameManager.coins += Add;

        transform.SetParent(null);
        feedbackDisappear?.PlayFeedbacks();


        //StartCoroutine(Disappear());


    }

    // 하드코딩 방법
    // IEnumerator Disappear()
    // {
    //     // 코인이 사라질 때, Track 종속이 아닌, World로 바꾼다.
    //     // Local => World 
    //     transform.SetParent(null);
       
    //     pivot.gameObject.SetActive(false);
    //     particle.Play();

    //     yield return new WaitUntil( ()=> particle.isPlaying == false);

    //     Destroy(gameObject);
    // }
}
