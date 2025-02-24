using UnityEngine;
using TMPro;
using DG.Tweening;
using CustomInspector;

public class IngameUI : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmInformation;


    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmMileage;
    [SerializeField] TextMeshProUGUI tmCoin;
    [SerializeField] TextMeshProUGUI tmHealth;


    void Awake()
    {
        tmInformation.text = "";        
    }

    void Update()
    {
        UpdateMileage();
        UpdateCoins(); 
    }
    

    Sequence _seqInfo;
    public void ShowInfo(string info, float duration = 1f)
    {                
        tmInformation.transform.localScale = Vector3.zero;
        
        if (_seqInfo != null)
            _seqInfo.Kill(true);

        // duration 전체 길이 => 연출이 duration 안에 종료되도록
        _seqInfo = DOTween.Sequence().OnComplete( ()=> tmInformation.transform.localScale = Vector3.zero );
        _seqInfo.AppendCallback( ()=> tmInformation.text = info );
        _seqInfo.Append(tmInformation.transform.DOScale(1.2f, duration*0.1f));
        _seqInfo.Append(tmInformation.transform.DOScale(1f, duration*0.2f));
        _seqInfo.AppendInterval(duration*0.4f);
        _seqInfo.Append(tmInformation.transform.DOScale(0f, duration*0.3f));
    }

    void UpdateMileage()
    {
        // 작은수 표현
        if ( GameManager.mileage <= 1000f )
        {
            long intpart = (long)GameManager.mileage;
            int decpart = (int)((GameManager.mileage - intpart) * 10);
            tmMileage.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }
        // 큰수 표현
        else
        {
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string unitpart);        
            tmMileage.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }
    }

    private uint _lastcoins;
    private Tween _tweencoin;
    void UpdateCoins()
    {
        if (_lastcoins == GameManager.coins) 
            return;

        if (_tweencoin != null)
            _tweencoin.Kill(true);

        // "N0" 역할 12345 => 12,345
        tmCoin.text = GameManager.coins.ToString("N0");
        _lastcoins = GameManager.coins;

        tmCoin.rectTransform.localScale = Vector3.one;
        _tweencoin = tmCoin.rectTransform.DOPunchScale(Vector3.one*0.5f, 0.2f, 10, 1)
                        .OnComplete(()=> tmCoin.rectTransform.localScale = Vector3.one);
    }
}