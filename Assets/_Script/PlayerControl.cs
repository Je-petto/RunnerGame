using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // 속성: Inspector에 노출
    [SerializeField] float speed;
    [SerializeField] AnimationCurve jumpCurve; //점프 그래프 모양
    [SerializeField] float jumpDuration = 0.5f; //점프 지속 시간
    [SerializeField] float jumpHeight = 3f; //점프 높이
    
    // Inspector에 노출 안됨 - but 다른 클래스에 공개는 함
    [HideInInspector] public TrackManager trackMgr;
    [HideInInspector] public int currentlane = 1;

    // 내부 사용: Inspector에 노출 안됨
    private Vector3 targetpos;
    private float jumpStartTime;
    private bool isJumping = false; //true: 점프 중, false : 바닥에 붙어 있는 중

    void Update()
    {
        if (Input.GetButtonDown("Left") && isJumping == false)// 왼쪽 이동
            HandlePlayer(-1);
        else if(Input.GetButtonDown("Right") && isJumping == false)// 오른쪽 이동
            HandlePlayer(1);
        else if(Input.GetButtonDown("Jump") && isJumping == false)// 점프
        {
            jumpStartTime = Time.time;
            isJumping = true;
        }
        
        if( isJumping == true )
        {
            //점프 시작 후 경과 시간 체크
            float elapsedTime = Time.time - jumpStartTime;
            // 분자/분모 => 퍼센트 
            float p = Mathf.Clamp(elapsedTime / jumpDuration, 0f, 1f);
            float height = jumpCurve.Evaluate(p) * jumpHeight;

            transform.position = new Vector3(transform.position.x,height,targetpos.z);

            if(p >=1f)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x,0f,targetpos.z);
            }

        }
        // 점프 시간 종료 => isJumping을 False로 바꾼다


        UpdatePosition();
    }

    //direction -1이면 왼쪽, +1이면 오른쪽
    void HandlePlayer(int direction)
    {
        currentlane += direction;
        currentlane = math.clamp(currentlane, 0, trackMgr.laneList.Count-1);

        Transform l = trackMgr.laneList[currentlane];

        targetpos = new Vector3(l.position.x, transform.position.y, transform.position.z);
    }
    void UpdatePosition()
    {
        // Lerp = Linear Interpolation : 선형보간
        transform.position = Vector3.Lerp(transform.position, targetpos, speed * Time.deltaTime);
    }

    // void UpdateJump()
    // {
    //     jumpStartTime = Time.time;
    // }
}
