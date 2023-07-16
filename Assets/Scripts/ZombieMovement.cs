using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{

    public float speed = 10f; //Скорость передвижения

    public Vector3 targetPosiyion;//будем задавать позицию

    //Player player; //ищем ccылку на чужой объект Player

    Rigidbody2D rb;//ищем ccылку на cвой компонент Rigidbody2D у Enemy

    Animator animator;//ищем ccылку на cвой компонент Animator у Enemy

    void Awake()//свои компоненты ищем через Awake
    {
        rb = GetComponent<Rigidbody2D>();//получаем ccылка на cвой компонент Rigidbody2D
        animator = GetComponent<Animator>();//получаем ccылка на cвой компонент Animator
    }

    //private void Start()//другие объекты ищем через Start
    //{
    //    player = FindObjectOfType<Player>();//получаем ccылка чужой объект Player
    //}


    void Update()
    {    

        Move(); //движение 
        Rotate(); //поворот 

    }

    private void Move()
    {

        //transform работает с Vector3, лучше использовать Vector3
        Vector3 zombiePosition = transform.position; //позиция зомби

        //позиция игрока (туда будет смотреть зомби)
        //Vector3 playerPosition = player.transform.position;

        //находит позицию какого-то таргета, который мы укажем
        Vector3 direction = targetPosiyion - zombiePosition;


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
        Vector3 zombiePosition = transform.position; //позиция зомби

        //позиция игрока (туда будет смотреть зомби)
        //Vector3 playerPosition = player.transform.position;

        //находит позицию какого-то таргета, который мы укажем
        Vector3 direction = targetPosiyion - zombiePosition;

        //z = нулю, персонаж не будет колбасится
        direction.z = 0;

        //красная стрелка объекта (transform.up - зеленая стрелка)
        transform.up = -direction;

    }


    //Вызывается,когда объект выключается (например выключается скрипт)
    private void OnDisable()
    {
        rb.velocity = Vector2.zero;//скорость передвижения зомби сбрасывается до 0
    }
}
