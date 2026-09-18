using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotacion : MonoBehaviour{

    public Vector3 rotationSpeed;
    

    private void Update()
    {

        Vector3 Rotation=new Vector3(rotationSpeed.x,rotationSpeed.y,rotationSpeed.z);

        transform.Rotate(Rotation * Time.deltaTime);
    }

}
