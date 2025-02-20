
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class RandomItem
{
    public string name;
    public int weight; //비중

    public abstract Object GetItem();
}

public class RandomGenerator
{
    public List<RandomItem> items = new List<RandomItem>();

    public int totalweight; // 모든 아이템 안의 비중 총합

    // 비중 총 합 구하기
    private void CalcTotalWeight()
    {
        totalweight = 0;
        
        foreach(var item in items)
        {
            totalweight += item.weight;
        }
    }

    // 비중에 맞게 랜덤 발생시키기

    // 3, 4, 7, 1 = 4개 : 총합 15
    public RandomItem GetRandom()
    {
        int rnd = Random.Range(0, totalweight);
        int weightSum = 0;
        // 0 ~ 14 : 15

        foreach ( var i in items )
        {
            weightSum += i.weight;
            //1루프 : 5 < 3 => 실패
            //2루프 : 5 < 3+4 => 성공
            if (rnd < weightSum)
                return i;
        }

    return null;
    }

    // 아이템을 등록하고, 비중 총 합을 다시 계산한다.
    public void AddItem(RandomItem item)
    {
        items.Add(item);
        CalcTotalWeight();
    }


}
