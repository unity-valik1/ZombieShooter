using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{

    public float destroyDelay = 1f;//через какое время уничтожиться взрыв

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyDelay);//уничтожение взрыва
    }
}
