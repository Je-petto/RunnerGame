using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using Deform;

public class PlayerControl : MonoBehaviour
{
    // 속성: Inspector에 노출
    [SerializeField] Transform pivot;
    [SerializeField] SquashAndStretchDeformer deformLeft, deformRight, deformJumpUp, deformJumpDown;

    [SerializeField] float moveDuration = 0.5f; //이동에 걸리는 시간
    //[SerializeField] AnimationCurve jumpCurve; //점프 그래프 모양
    [SerializeField] Ease moveEase;
    [SerializeField] float jumpDuration = 0.5f; //점프 지속 시간
    [SerializeField] float jumpHeight = 3f; //점프 높이
    [SerializeField] Ease jumpEase;
    
    // Inspector에 노출 안됨 - but 다른 클래스에 공개는 함
    [HideInInspector] public TrackManager trackMgr;
    
    // 내부 사용: Inspector에 노출 안됨
    private int currentlane = 1;
    private Vector3 targetpos;
    private bool isMoving = false;

    void Update()
    {
        if (pivot == null)
            return;
        
        if (Input.GetButtonDown("Left") && currentlane > 0)
            HandleDirection(-1);
        else if (Input.GetButtonDown("Right") && currentlane < trackMgr.laneList.Count-1)
            HandleDirection(1);
        else if (Input.GetButtonDown("Jump"))
            HandleJump();
    }

    //direction -1이면 왼쪽, +1이면 오른쪽
    private Sequence _seqMove;
    void HandleDirection(int direction)
    {
        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if ( _seqMove != null )
        {
            _seqMove.Kill(true);
            squash.Factor = 0f;
        }
        
        isMoving = true;
        
        currentlane += direction;
        currentlane = math.clamp(currentlane, 0, trackMgr.laneList.Count-1);

        Transform l = trackMgr.laneList[currentlane];

        targetpos = new Vector3(l.position.x, pivot.position.y, pivot.position.z);

        _seqMove = DOTween.Sequence().OnComplete(()=> squash.Factor = 0);
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> squash.Factor = v));
        _seqMove.Join(DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> squash.Factor = v));

    }

    void HandleJump()
    {
        var seqjump = DOTween.Sequence().OnComplete( ()=> {deformJumpUp.Factor = 0; deformJumpDown.Factor = 0; });
        seqjump.Append(pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration).SetEase(jumpEase));
        seqjump.Join(DOVirtual.Float(0f, 1f, jumpDuration/4f, (v)=> deformJumpUp.Factor = v));
        seqjump.Append(DOVirtual.Float(1f, 0f, jumpDuration/4f, (v)=> deformJumpUp.Factor = v));
        seqjump.Join(DOVirtual.Float(0f, 1f, jumpDuration/2f, (v)=> deformJumpDown.Factor = v));
        seqjump.Append(DOVirtual.Float(1f, 0f, jumpDuration/2f, (v)=> deformJumpDown.Factor = v));
    }
    
    // void Update()
    // {
    //     if (Input.GetButtonDown("Left") && isJumping == false)// 왼쪽 이동
    //         HandlePlayer(-1);
    //     else if(Input.GetButtonDown("Right") && isJumping == false)// 오른쪽 이동
    //         HandlePlayer(1);
    //     else if(Input.GetButtonDown("Jump") && isJumping == false)// 점프
    //     {
    //         jumpStartTime = Time.time;
    //         isJumping = true;
    //     }

    //     if( isJumping == true )
    //     {
    //         //점프 시작 후 경과 시간 체크
    //         float elapsedTime = Time.time - jumpStartTime;
    //         // 분자/분모 => 퍼센트 
    //         float p = Mathf.Clamp(elapsedTime / jumpDuration, 0f, 1f);
    //         float height = jumpCurve.Evaluate(p) * jumpHeight;

    //         transform.position = new Vector3(transform.position.x,height,targetpos.z);

    //         if(p >=1f)
    //         {
    //             isJumping = false;
    //             transform.position = new Vector3(transform.position.x,0f,targetpos.z);
    //         }

    //     }
    //     // 점프 시간 종료 => isJumping을 False로 바꾼다


    //     // UpdatePosition();
    // }
    // void UpdatePosition()
    // {
    //     // Lerp = Linear Interpolation : 선형보간
    //     transform.position = Vector3.Lerp(transform.position, targetpos, speed * Time.deltaTime);
    // }

    // void UpdateJump()
    // {
    //     jumpStartTime = Time.time;
    // }

    
        
        // if (direction == -1) //left 이동
        // {
        //     //deformLeft.Factor = 1;
        //     DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> deformLeft.Factor = v)
        //             .OnComplete(()=> DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> deformLeft.Factor = v));
        // }
        // else if (direction == 1) //right 이동
        // {
        //     //deformRight.Factor = -1;
        //     DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> deformLeft.Factor = v)
        //             .OnComplete(()=> DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> deformLeft.Factor = v));
        // }
    
        // pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
        //         .OnComplete( ()=> isMoving = false)
        //         .SetEase(jumpEase);
}
