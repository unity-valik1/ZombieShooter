using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f; //Скорость передвижения

    Rigidbody2D rb;//завели ccылка на Rigidbody2D у Player

    Animator animator;//завели ccылка на Animator у Player

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//получаем ccылка на Rigidbody2D
        animator= GetComponent<Animator>();//получаем ccылка на Animator
    }


    void Update()
    {
        Move(); //движение
        Rotate(); //поворот

    }

    private void Move()
    {
        //2D физика(Rigidbody2D) работает с Vector2, лучше использовать Vector2

        float inputX = Input.GetAxis("Horizontal");//движение влево-вправо
        float inputY = Input.GetAxis("Vertical");//движение вверх-вниз

        //движение физического объекта
        Vector2 direction = new Vector2(inputX, inputY);

        //проверка
        //если по диагонали двигается быстрее 
        if (direction.magnitude > 1)
        {
            //то нормализовать скорость
            direction = direction.normalized; 
        }

        //в аниматор передается какая скорость движения
        //если персонаж двигается скорость > 0
        //если не двигается скорость = 0
        animator.SetFloat("Speed", direction.magnitude);

        //скорость объекта
        rb.velocity = direction * speed;



        ////дживение персонажа на стрелках или w-a-s-d
        //Vector3 padNewPosition = transform.position;//берем позицию для изменения

        //float inputX = Input.GetAxis("Horizontal");//движение влево-вправо
        //float inputY = Input.GetAxis("Vertical");//движение вверх-вниз

        //padNewPosition.x += speed * Time.deltaTime * inputX;//движение влево-вправо
        //padNewPosition.y += speed * Time.deltaTime * inputY;//движение вверх-вниз

        //transform.position = padNewPosition;//применить новую позицию

    }

    private void Rotate()
    {
        //transform работает с Vector3, лучше использовать Vector3
        Vector3 playerPosition = transform.position; //позиция игрока

        //позиция мышки (туда будет смотреть игрок)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //находим позицию мыши(курсора)
        Vector3 direction = mousePosition - playerPosition;

        //z = нулю, персонаж не будет колбасится
        direction.z = 0;

        //красная стрелка объекта (transform.up - зеленая стрелка)
        transform.up = -direction; 

    }
}
