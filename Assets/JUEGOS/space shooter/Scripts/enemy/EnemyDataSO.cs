using UnityEngine;

namespace SpaceShooter
{
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
        [Header("Configuración Estética y Visual")]
        public EnemyColor color = EnemyColor.Red;

        [Header("Parámetros de Gameplay")]
        public int maxHealth = 1;
        public int points = 10;
    }
}