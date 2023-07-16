using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{ 
    //бомба с анимацией как и у героев

    public LayerMask damageLayers;//переменна€ класс LayerMask
    public float radius = 5f;//радиус взрыва
    public int damage = 30;//урон от взрыва бочки

    Animator animator;//завели ccылка на Animator у Bomb


    void Awake()
    {
        animator = GetComponent<Animator>();//получаем ccылка на Animator
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();//когда бомба сталкивоетс€ с объектом -
                                                         //мы пытаемс€получаем скрипт пули
        print("damage: " + bullet.damage);

        Explode();
    }


    private void Explode()
    {
        animator.SetTrigger("Explosion"); //вызываем анимацию взрыва при тригере
        Destroy(gameObject, 0.7f); //уничтажаем бочку

        //в Layer добавили слои "Player", "Zombie" и теперь им будет наноситс€ урон в радиусе взрыва бочки
        //LayerMask layerMask = LayerMask.GetMask("Player", "Zombie");

        //находим массив коллайдеров
        //позици€ бочки, радиус взрыва, урон по слою который выберем в gameObject 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);

        foreach (Collider2D collider in colliders)
        {
            //if (collider.gameObject.CompareTag("Player"))
            //{
            //    Player player = collider.GetComponent<Player>();
            //}

            //передаю gameObject сообщение "UpdateHealth" и значение damage
            collider.gameObject.SendMessage("UpdateHealth", -damage);
        }
    }

    //¬идить эти радиусы дл€ тестировани€ и дл€ настройки их
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;//окружность черного цвета
        //(центор сферы = позици€ бомбы, радиус сферы = радиус взрыва)
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
