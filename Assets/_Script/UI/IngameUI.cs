using UnityEngine;
using TMPro;


public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;

    void Update()
    {
        if (GameManager.IsPlaying == false)
            return;

        //int mileageI = GameManager.mileage;

        //string mileage = $"{GameManager.mileage:F1}M";
        tmDistance.text = ((int)GameManager.mileage).ToStringKilo() + "<size=80%>m</size>";
    }

}
