using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    //объявляем событие которое будет вызываться (здоровье изменилось)//нужен using System;
    public Action HealthChanged = delegate { };//delegate { } - пустое действие, чтобы не было ошибки в случае, если никто не подпишется


    [Header("AI config")]
    public float moveRadius = 10; //радиус преследования
    public float standbyRadius = 15; //радиус видеть игрока
    public float attackRadius = 3; //радиус атаки
    public int viewAngle = 90;//радиус обзора перед собой

    [Header("Gameplay config")]
    public float attackRate = 1f; //чистота скорости удара зомби (1f - раз в секунду)
    public int health = 100;//здоровье зомби
    public int damage = 20;//урон зомби


    Player player;//ищем ccылку на чужой объект Player

    ZombieState activeState;//тип с фиксированными значениями (может хранить только 1 из значение из enum ZombieState)

    Animator animator;//ищем ccылку на cвой компонент Animator у Enemy
    AIPath aiPath;//ищем ccылку на cвой компонент aiPath у Enemy //using Pathfinding;
    AIDestinationSetter aiDestinationSetter;

    float nextAttack; //через сколько будет произведена следующая атака следующая атака 

    float distanceToPlayer;//дистанция до игрока

    bool isDead = false;//мертв ли зомби

    Vector3 startPosition;//стартовая позиция

    //структура перечисления
    //значение когда зомби стоит, идет и атакует
    enum ZombieState
    {
        //набор значений (названий объекта)
        STAND,
        RETURN,
        MOVE_TO_PLATER,
        ATTACK
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();//получаем ccылка на Animator у Enemy
        aiPath = GetComponent<AIPath>();//получаем ccылка на aiPath у Enemy
        aiDestinationSetter = GetComponent<AIDestinationSetter>();//получаем ccылка на aiDestinationSetter у Enemy
    }

    void Start()
    {
        player = FindObjectOfType<Player>();//получаем ccылка чужой объект Player

        //стартовая позиция = текущей позиции
        startPosition = transform.position;
        //активное состояние зомби на момент старата = стоит
        ChangeState(ZombieState.STAND);//после точки только значения, которые в enum ZombieState

        player.OnDeath += PlayerDied;//подписываемся на смерть playera
    }

    private void PlayerDied()//player умер
    {
        ChangeState(ZombieState.RETURN);//зоибм вернуться на свое место
    }

    public void UpdateHealth(int amount)//метод принимает какое колличество здоровья изменить
    {
        health += amount;//здоровье меняется на колличество

        //если здоровье <= 0
        if (health <= 0)
        {
            isDead = true;//зомби мертв

            animator.SetTrigger("Dead");//анимация смерти
            Destroy(gameObject, 0.9f); //уничтажаем зомби

            player.OnDeath -= PlayerDied;//отписываемся на смерть playera (чтобы больше не вызывалось)
        }
        HealthChanged();//вызов события
    }


    private void OnTriggerEnter2D(Collider2D collision)//урон от пули
    {
        Bullet bullet = collision.GetComponent<Bullet>();//получаем пулю у collision
        UpdateHealth(-bullet.damage);//изменяем здоровье на урон от пули
    }

    void Update()
    {
        //если умер
        if (isDead)
        {
            return;//выходим из Update() (ничего не апдейтить для него)
        }


        //дистанция между игроком и зомби
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        //делаем логику для activeState
        switch (activeState)
        {
            case ZombieState.STAND://тогда зомби стоит
                DoStand();//тогда зомби идет               
                break;
            case ZombieState.RETURN://тогда зомби возвращается на стартовую позицию
                DoReturn();//тогда зомби идет               
                break;
            case ZombieState.MOVE_TO_PLATER://тогда зомби идет
                DoMove();//тогда зомби атакует
                break;
            case ZombieState.ATTACK://тогда зомби атакует
                DoAttack();//тогда зомби идет
                break;
        }

    }


    private void ChangeState(ZombieState newState)
    {
        //делаем логику для activeState
        switch (newState)
        {
            case ZombieState.STAND://тогда зомби стоит
                aiPath.enabled = false;// НЕ ПРИСЛЕДУЕТ ИГРОКА, А СТОИТ НА МЕСТЕ (выключаем движение)
                break;
            case ZombieState.RETURN://тогда зомби возвращается на стартовую позицию
                //aiDestinationSetter.target =
                //movement.targetPosiyion = startPosition;//тогда зомби возвращается на стартовую позицию
                aiPath.enabled = true;// НЕ ПРИСЛЕДУЕТ ИГРОКА, А идет на стартовую позицию
                break;
            case ZombieState.MOVE_TO_PLATER://тогда зомби идет
                aiPath.enabled = true;//ПРИСЛЕДУЕТ ИГРОКА
                aiDestinationSetter.target = player.transform;//тогда зомби идет за игроком
                break;
            case ZombieState.ATTACK://тогда зомби атакует
                aiPath.enabled = false;// ОСТАНАВЛИВАЕТСЯ И АТАКУЕТ
                break;
        }
        activeState = newState;//активная позиция равна новой позиции
    }

    private void DoStand()
    {
        if (!player.isDead) //если Player живой
        {
            CheckMoveToPlayer();
        }
    }


    private void DoReturn()
    {
        if (!player.isDead && CheckMoveToPlayer())//если Player живой и дистанция до него больше
        {
            return;//то ничего не делаем, выходим из всего метода
        }

        //CheckMoveToPlayer();

        //дистанция к стартовой позиции
        //позиция между текущей и стартовой позиции
        float distanceToStart = Vector3.Distance(transform.position, startPosition);
        if (distanceToStart <= 0.05f)//если почти в стартовой позиции 
        {
            ChangeState(ZombieState.STAND);//стоит111111111111111111111111111111111
            return;
        }
    }


    private bool CheckMoveToPlayer()//метод который обрабатывает переходы в новое состояние зомби ( стоит, идет и тд)
    {

        if (distanceToPlayer > moveRadius)
        {
            return false;
        }

        //проверяем препядствия
        //куда мы хотим чтобы зомби был направлен
        //от позиции игрока отнимаем позицию зомьби
        Vector3 directionToPlayer = player.transform.position - transform.position;

        //задаем направление луч
        //позиция зомби, позиция игрока, цвет красный
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        float angle =Vector3.Angle(-transform.up, directionToPlayer); //угол - перед зомби и позиция игрока
        if(angle > viewAngle/2)//если угол - перед зомби и позиция игрока > угол 90 градусов перед игроком
        {
            return false;//ничего не делаем
        }

        LayerMask layerMask = LayerMask.GetMask("Obstacles");
        //RaycastHit2D пускает луч и проверяет коллайдеры на пути(проверяет  есть ли на пути луча физ объект)
        //позиция зомби, позиция игрока, длинна между зомби и игроком, layerMask ???
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask);
        if (hit.collider != null)//есть объекты(коллайдер) на пути луча
        {
            return false;
        }
        else//нет объектов на пути луча
        {

        }

        //бежать за игроком
        ChangeState(ZombieState.MOVE_TO_PLATER);//тогда зомби идет
        return true;
    }

    private void DoMove()
    {
        if (distanceToPlayer < attackRadius)
        {
            ChangeState(ZombieState.ATTACK);//тогда зомби атакует
            animator.SetFloat("Speed", 0);

            return;
        }
        if (distanceToPlayer > standbyRadius)
        {
            ChangeState(ZombieState.RETURN);//тогда зомби возвращается на стартовую позицию
            animator.SetFloat("Speed", 0);

            return;
            //анимация стоит на месте
        }
        animator.SetFloat("Speed", 1);
    }


    private void DoAttack()
    {
        if (distanceToPlayer > attackRadius)
        {
            ChangeState(ZombieState.MOVE_TO_PLATER);//тогда зомби идет
            StopAllCoroutines();//остановить все карутины которые запущенны у этого объекта
            return;
        }
        //ТО ЖЕ ЧТО И КАРУТИНА
        //Time.deltaTime;  время,которое прошло с предидущего кадра
        // уменьшает время после атаки (например: 1сек - настоящее время)
        nextAttack -= Time.deltaTime;

        ////если до след атаки осталось < 0
        if (nextAttack <= 0)
        {
            animator.SetTrigger("Shoot");//анимация атаки

            //player.UpdateHealth(-damage);// изменяем здороье игрока

            nextAttack = attackRate;// следующая атака возможна через чистоту атаки
        }
    }


    public void DamageToPlayer()
    {
        //если дистанция до играка больше чем радиус атаки зомби
        if (distanceToPlayer > attackRadius)
        {
            // то зомби ударит по воздуху и не нанесет урон игроку
            return;
        }
        player.UpdateHealth(-damage);// изменяем здороье игрока
    }


    private void OnDrawGizmos() //Видить эти радиусы для тестирования и для настройки их
    {
        Gizmos.color = Color.blue;//окружность синего цвета
        //(центор сферы = позиция игрока, радиус сферы = радиус преследования)
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;//окружность красного цвета
        //(центор сферы = позиция игрока, радиус сферы = радиус атаки)
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;//окружность желтого цвета
        //(центор сферы = позиция игрока, радиус сферы = радиус видеть игрока)
        Gizmos.DrawWireSphere(transform.position, standbyRadius);


        Gizmos.color = Color.cyan;//окружность желтого цвета
        Vector3 lookDirection = -transform.up;//взгляд зомби вперед
        //= угол вокруг оси(угол на который нужно развирнуть, ось на которую нужно развернуть) * на взгляд зомби вперед
        Vector3 leftViewVector = Quaternion.AngleAxis(viewAngle / 2, Vector3.forward) * lookDirection;
        //= угол вокруг оси( -угол на который нужно развирнуть, ось на которую нужно развернуть) * на взгляд зомби вперед
        Vector3 rightViewVector = Quaternion.AngleAxis(-viewAngle / 2, Vector3.forward) * lookDirection;
        Gizmos.DrawRay (transform.position, lookDirection * moveRadius);//луч(позиция зомби, взгляд зомби вперед * на радиус начать ходьбу)
    }
}
