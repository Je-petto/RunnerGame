using UnityEngine;
using TMPro;
using DG.Tweening;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;
    [SerializeField] TextMeshProUGUI tmInformation;


    void Awake()
    {
        tmInformation.text = "";
    }

    void Update()
    {
        if (GameManager.IsPlaying == false)
            return;


        UpdateMileage();        
    }

    void UpdateMileage()
    {
        // 작은수 표현
        if ( GameManager.mileage <= 1000f )
        {
            long intpart = (long)GameManager.mileage;
            int decpart = (int)((GameManager.mileage - intpart) * 10);
            tmDistance.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }
        // 큰수 표현
        else
        {
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string unitpart);        
            tmDistance.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }
    }

    Sequence _seqInfo;
    public void ShowInfo(string info, float duration = 1f)
    {
        tmInformation.transform.localScale = Vector3.zero;
        
        if (_seqInfo != null)
            _seqInfo.Kill(true);

        // duration 전체 길이 => 연출이 duration 안에 종료되도록
        _seqInfo = DOTween.Sequence();
        _seqInfo.AppendCallback( ()=> tmInformation.text = info );
        _seqInfo.Append( tmInformation.transform.DOScale(1.2f, duration*0.1f));
        _seqInfo.Append( tmInformation.transform.DOScale(1f, duration*0.2f));
        _seqInfo.AppendInterval(duration*0.4f);
        _seqInfo.Append( tmInformation.transform.DOScale(0f, duration*0.3f));

        // tmInformation.transform.DOScale()
        // 숫자 시작할때 크기 120% -> 100% -> 0%
        // 모든 연출은 duration 동안 완료되도록

    }
}