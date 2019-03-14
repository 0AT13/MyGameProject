using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {

    public int damage = 2;

	void Start ()
    {
        Destroy(gameObject, 0.75f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        SnakeAI enemy = col.GetComponent<SnakeAI>();

        if (enemy != null)
            enemy.GetDamage(damage);
    }
}
