using UnityEngine;

namespace SpaceShooter
{
    public class SimpleBoltDebug : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("La bala chocó con: " + other.gameObject.name + " | Tag: " + other.tag);
        }
    }
}