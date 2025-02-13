using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float horzspeed;

    [HideInInspector]public TrackManager trackMgr;
    int currentlane =1;

    void Update()
    {
        //float horz = Input.GetAxisRaw("Horizontal");

        Input.GetButtonDown("Horizontal");
        if (Input.GetKeyDown(KeyCode.A))// 왼쪽 이동
        {
            currentlane -= 1;
            currentlane = math.clamp(currentlane, 0, trackMgr.laneList.Count-1);

            Transform l = trackMgr.laneList[currentlane];

            transform.position = new Vector3(l.position.x, transform.position.y, transform.position.z);
        }
        else if(Input.GetKeyDown(KeyCode.D))// 오른쪽 이동
        {
            currentlane += 1;
            currentlane = math.clamp(currentlane, 0, trackMgr.laneList.Count-1);

            Transform l = trackMgr.laneList[currentlane];

            transform.position = new Vector3(l.position.x, transform.position.y, transform.position.z);
        }
        else// 움직임 없음
        {

        }

        //transform.position += Vector3.right * horzspeed *horz* Time.smoothDeltaTime;
    }
}
