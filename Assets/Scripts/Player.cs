using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //объ€вл€ем событие которое будет вызыватьс€ (здоровье изменилось)//нужен using System;
    public Action OnHealthChange = delegate { };//delegate { } - пустое действие, чтобы не было ошибки в случае, если никто не подпишетс€
    public Action OnDeath = delegate { };//delegate { } - пустое действие, чтобы не было ошибки в случае, если никто не подпишетс€



    public Bullet bulletPrefab; // дл€ пули

    //пустой GameObject shootPosition от куда вылетает пул€
    public GameObject shootPosition;
    public bool isDead = false;// игрок мертв

    public float fireRate = 1f; //чистота скорости вылета поли (1f - раз в секунду)
    public int health = 100;//здоровье игрока

    float nextFire; //через сколько можно произвести следующий выстрел следующий выстрел

    Animator animator;//завели ccылка на Animator у Player


    private void Awake()
    {
        animator= GetComponent<Animator>();//получаем ccылка на Animator
    }

    public void UpdateHealth(int amount)
    {
        health += amount;//мен€ет здоровье на пришедшее кол-во
        OnHealthChange();

        if (health <= 0)//если здоровье игрока <= 0
        {
            isDead = true;//то игрок умирает
            animator.SetTrigger("Dead");//анимаци€ смерти
            OnDeath(); //смерть игрока
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckFire(); //стрельба

    }

    private void CheckFire()//стрельба
    {
        //GetKeyDown (кнопка с клавы)
        //GetButtonDown (виртуальную кнопку из InputManager)
        //GetKeyDown и GetButtonDown по нажатию
        //GetKey и GetButton можно зажимать

        //если кнопка стрельбы нажата и врем€ дл€ выстрела пршло 
        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            Shoot(); //јнимаци€ и след выстрел
        }

        // проверка (отнимать только врем€ до следующего выстрела)
        //т.е. от 1 сек до 0 сек) в минус не идти и не засор€ть)
        if (nextFire > 0)
        {//Time.deltaTime;  врем€,которое прошло с предидущего кадра
         // уменьшает врем€ после выстрела (например: 1сек - насто€щее врем€)
            nextFire -= Time.deltaTime;
        }
    }

    private void Shoot()//јнимаци€ и след выстрел
    {
        animator.SetTrigger("Shoot");//вызываем анимацию стрельбы (по имени) при вылете пули
                                     //»грок создает вылет пули,
                                     //¬ качестве позиции вылета пули = сво€ позици€ +
                                     //+ пустой GameObject shootPosition,
                                     //¬ качестве вращение вылета пули = свое вращение)

        Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);

        //следующий выстрел через заданное врем€
        nextFire = fireRate;

        //задаем направление луч
        //позици€ игрока, куда летит пул€, цвет зеленый, длительность 10f
        //Debug.DrawRay(transform.position, (shootPosition.transform.position - transform.position) * 30, Color.green, 10f);

    }
}
