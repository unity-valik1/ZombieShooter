using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target; //кладем объект к которому привяжется камера и будет с ним бегать


    void Update()
    {
        Vector3 newPosition = target.transform.position;// позицию берем у игрока

        newPosition.z = transform.position.z; //только z оставляем свой (-10)

        transform.position= newPosition; //позиция камеры = позиции игрока
    }
}
