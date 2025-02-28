using UnityEngine;
using CustomInspector;

[System.Serializable]
public struct Phase
{
    public string Name;
    public uint Mileage;

    [Space(5)]
    public float scrollSpeed;
    [SerializeField, AsRange(0, 100)] Vector2 obstacleInterval;
}
