using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour {

    private int temp = 0;
    public float damage = 0;

    private PlayerController Hero;

    void OnTriggerEnter2D(Collider2D col) //Взаємодія з об'єктами в які потрапляє персонаж
    {
        if (col.gameObject.tag == "Player") //Попадання героя в ворожий об'єкт
        {
            Hero = col.GetComponentInParent<PlayerController>(); //Звернення до об'єкта героя

            //Перевірка на потрапляння більше ніж на однин об'єкт за раз
            temp++;
            if (temp < 2)
                Hero.DamageHero(damage);
            else
                temp = 0;
        }
    }
}
