using UnityEngine;

public class PlayerAtackController : MonoBehaviour {

    public float damage = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            SnakeAI enemy = col.GetComponent<SnakeAI>();
            
            enemy.GetDamage(damage);
        }
    }
}
