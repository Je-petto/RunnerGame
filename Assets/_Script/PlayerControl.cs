using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using Deform;

public enum PlayerState { Idle = 0, Move, Jump, Slide }

public class PlayerControl : MonoBehaviour
{
    [Space(20)]
    // 속성 : 인스펙터 노출
    [SerializeField] Transform pivot;
    [SerializeField] Collider colNormal, colSlide; // 0: 기본상태 , 1: 슬라이드
    
    [SerializeField] SquashAndStretchDeformer deformLeft, deformRight, deformJumpUp, deformJumpDown, deformSlide;


    [Space(20)]
    [SerializeField] float moveDuration = 0.5f; // 이동에 걸리는 시간
    [SerializeField] Ease moveEase;

    [Space(20)]
    [SerializeField] float jumpDuration = 0.5f;    // 점프 지속 시간
    [SerializeField] float jumpHeight = 3f;      // 점프 높이
    [SerializeField] Ease jumpEase;
    [SerializeField] float[] jumpIntervals = { 0.25f, 0.5f, 0.75f, 0.25f }; // 점프 시퀀스 타이밍 조절

    [Space(20)]
    [SerializeField] float slideDuration = 0.5f;    // 슬라이드 지속 시간


    // 다른 클래스에 공개는 하지만 인스펙터 노출 안함
    [HideInInspector] public TrackManager trackMgr;

    public PlayerState state;

    // 내부 사용 : 인스펙터 노출 안함
    private int currentLane = 1;
    private Vector3 targetpos;

    void Start()
    {
        SwitchCollider(true);
    }

    

    void Update()
    {
        //CHEAT
        //Space 키 토글 , 처음 => 멈춤 ,  다시 => Play
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.IsPlaying = !GameManager.IsPlaying; 

        if (pivot == null || GameManager.IsPlaying == false)
            return;

        if (Input.GetButtonDown("Left") && currentLane > 0)
            HandleDirection(-1);

        if (Input.GetButtonDown("Right") && currentLane < trackMgr.laneList.Count-1 )
            HandleDirection(1);
        
        if (Input.GetButton("Jump"))
            HandleJump();          

        if (Input.GetButton("Slide"))
            HandleSlide();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other)        
        {
            GameManager.IsPlaying = false;
        }
    }



    private Sequence _seqMove;
    // direction -1 이면 왼쪽 , +1 이면 오른쪽
    void HandleDirection(int direction)
    {
        if ( state == PlayerState.Jump || state == PlayerState.Slide ) return;

        state = PlayerState.Move;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null)
        {
            _seqMove.Kill(true);
            state = PlayerState.Move;
        }

        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y , pivot.position.z );

        _seqMove = DOTween.Sequence().OnComplete(()=> {squash.Factor = 0; state = PlayerState.Idle; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> squash.Factor = v ));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> squash.Factor = v ));
    }

    void HandleJump()
    {
        if ( state != PlayerState.Idle ) return;

        state = PlayerState.Jump;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
                .SetEase(jumpEase);

        deformJumpUp.Factor = 0f;
        deformJumpDown.Factor = 0f;  

        Sequence seq = DOTween.Sequence().OnComplete( ()=> state = PlayerState.Idle );
        seq.Append(DOVirtual.Float( 0f, 1f, jumpDuration * jumpIntervals[0], v => deformJumpUp.Factor = v ));
        seq.Append(DOVirtual.Float( 1f, 0f, jumpDuration * jumpIntervals[1], v => deformJumpUp.Factor = v ));        
        seq.Join(DOVirtual.Float( 0f, 1f, jumpDuration * jumpIntervals[2], v => deformJumpDown.Factor = v ));
        seq.Append(DOVirtual.Float( 1f, 0f, jumpDuration * jumpIntervals[3], v => deformJumpDown.Factor = v ));        
    }

    void HandleSlide()
    {
        if ( state != PlayerState.Idle ) return;

        state = PlayerState.Slide;
        SwitchCollider(false);

        Sequence seq = DOTween.Sequence().OnComplete( ()=> 
        {
            state = PlayerState.Idle;
            SwitchCollider(true);
        });
        seq.Append(DOVirtual.Float( 0f, -1f, slideDuration * 0.25f , v => deformSlide.Factor = v ));
        seq.AppendInterval(slideDuration * 0.25f);
        seq.Append(DOVirtual.Float( -1f, 0f, slideDuration * 0.5f, v => deformSlide.Factor = v ));        
    }

    // b : TRUE -> 기본모드 , FALSE -> 슬라이드
    void SwitchCollider(bool b)
    {
        colNormal.gameObject.SetActive(b);
        colSlide.gameObject.SetActive(!b);
    }

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

