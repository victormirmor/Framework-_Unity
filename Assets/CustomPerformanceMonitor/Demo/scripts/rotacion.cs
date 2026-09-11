using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotacion : MonoBehaviour{

    public Vector2 rotationSpeed;
    

    private void Update()
    {

        Vector3 Rotation=new Vector3(rotationSpeed.x,rotationSpeed.y,0f);

        transform.Rotate(Rotation * Time.deltaTime);
    }

}
