using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb2 : MonoBehaviour
{
    //бомба с анимацией как в платформе

    public GameObject explosionEffectPrefab; //делаем ссылку на какой-то GameObject

    void Awake()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //чек таг тригер пули

        Explode();

    }


    private void Explode()
    {
        //создаем взрыв, в своей позиции, без поворотов 
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        //уничтожить бочку
        Destroy(gameObject);
    }
}
