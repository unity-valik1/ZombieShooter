using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float speed = 20f; //скорость пули
    public int damage = 10;//урон пули

    Rigidbody2D rb; //завели ccылку на Rigidbody2D у пули

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//получаем ccылка на Rigidbody2D

    }


    private void Start()
    {
        //направление пули 
        rb.velocity = -transform.up * speed;
    }


    //пуля не видна - удаляем ее
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
