using UnityEngine;

public enum EnemyColor
{
    Red,
    Green,
    Blue,
    Yellow
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "SpaceShooter/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public BrickColor color = BrickColor.Red;
    public int points = 10;
}
