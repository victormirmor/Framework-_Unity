using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Se coloca en los objetos de pared/límite o en el padre Boundary.
    /// Al entrar en contacto con cualquier pared, destruye proyectiles y 
    /// limpia los registros de enemigos en los gestores del juego.
    /// </summary>
    public class BoundaryController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // 1. Ignorar colisiones con la nave del jugador
            if (other.CompareTag("Player"))
            {
                return;
            }

            // 2. Si se trata de un enemigo, realizar limpieza silenciosa de listas y diccionarios
            if (other.CompareTag("Enemy"))
            {
                if (EnemyHealthManager.Instance != null)
                {
                    EnemyHealthManager.Instance.RemoveAndDestroy(other.gameObject);
                }
                else
                {
                    Destroy(other.gameObject);
                }
                return;
            }

            // 3. Si es un proyectil (PlayerBolt o Bolt_enemy), destruirlo directamente
            Destroy(other.gameObject);
        }
    }
}