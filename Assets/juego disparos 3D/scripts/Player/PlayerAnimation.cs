using UnityEngine;

    
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimations : MonoBehaviour
    {
        private Animator anim;

        void Awake ()
        {
            anim = GetComponent<Animator>();
        }

        void Update (){
            playerAnim();
        }

        void playerAnim(){
            // Leer ejes de movimiento
            float h = Input.GetAxisRaw(InputConstants.AXIS_HORIZONTAL);
            float v = Input.GetAxisRaw(InputConstants.AXIS_VERTICAL);

            // 1. Enviar valores al Blend Tree 2D
            anim.SetFloat(InputConstants.ANIM_SPEED_X, h);
            anim.SetFloat(InputConstants.ANIM_SPEED_Y, v);
        }
}
