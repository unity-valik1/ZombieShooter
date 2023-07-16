using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI1 : MonoBehaviour
{
    public Zombie1 zombie1;//ссылка на Zombie (лучше для мобилок так)

    public Slider healthSlider;//ссылка на Slider (лучше для мобилок так)

    //public Image healthSlider2;//ссылка на Slider2

    void Start()
    {
        healthSlider.maxValue = zombie1.health;//максимальное здоровье = здоровье зомби
        healthSlider.value = zombie1.health;//ползунок здоровья = здоровье зомби

        zombie1.HealthChanged += UpdateHealthBar;//подписался изменение здоровья
    }


    public void UpdateHealthBar(/*int health*/)//подписался изменение здоровья
    {
        healthSlider.value = zombie1.health; //изменяется здоровье с уроном
    }


    private void LateUpdate()//выполняется позже Update() чтобы не дергалось
    {
        transform.rotation = Quaternion.identity;//Quaternion.identity - никакое вращение канваса
    }
}
