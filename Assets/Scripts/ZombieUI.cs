using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI : MonoBehaviour
{
    public Zombie zombie;//ссылка на Zombie (лучше для мобилок так)

    public Slider healthSlider;//ссылка на Slider (лучше для мобилок так)

    //public Image healthSlider2;//ссылка на Slider2

    void Start()
    {
        healthSlider.maxValue = zombie.health;//максимальное здоровье = здоровье зомби
        healthSlider.value = zombie.health;//ползунок здоровья = здоровье зомби

        zombie.HealthChanged += UpdateHealthBar;//подписался изменение здоровья
    }


    public void UpdateHealthBar(/*int health*/)//подписался изменение здоровья
    {
        healthSlider.value = zombie.health; //изменяется здоровье с уроном
    }


    private void LateUpdate()//выполняется позже Update() чтобы не дергалось
    {
        transform.rotation = Quaternion.identity;//Quaternion.identity - никакое вращение канваса
    }
}
