using UnityEngine;
using CustomInspector;

// MonoBehaviour -> Runtime(실행 중) 작동 클래스
// ScriptableObject -> 에디터 어디서든 존재할 수 있는 클래스

[CreateAssetMenu(menuName = "Data/Phase")]
public class PhaseSO : ScriptableObject
{
    
    public string displayName;
    [Preview(Size.small)] public Sprite Icon;
    public uint mileage;

    public float scrollSpeed;

    // 장애물(Obstacle) 설정

    [AsRange(0, 100)] public Vector2 obstacleInterval;

    [Foldout] public ObstacleSO obstacleData;

}
