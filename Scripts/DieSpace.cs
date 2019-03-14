using UnityEngine;

public class DieSpace : MonoBehaviour {

    //Метод "Мертвої зони"
    public Transform respawn;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController temp;
            temp = other.GetComponentInChildren<PlayerController>();

            temp.DamageHero(1);

            other.transform.position = respawn.transform.position;
        }
        else
        {
            Destroy(other.gameObject);
        }
    } 
}
