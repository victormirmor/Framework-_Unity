using UnityEngine;

public enum BrickColor
{
    Red,
    Green,
    Blue,
    Yellow
}

[CreateAssetMenu(fileName = "NewBrickData", menuName = "Arkanoid/Brick Data")]
public class BrickDataSO : ScriptableObject
{
    public BrickColor color = BrickColor.Red;
    public int points = 10;
}