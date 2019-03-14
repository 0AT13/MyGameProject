using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public Item item;

	void OnTriggerEnter2D (Collider2D col)
    {
        if (col.name == "Hero")
        {
            PickUp();
        }
    }

    void PickUp ()
    {
        bool wasPickedUp = Inventory.instance.Add(item);

        if(wasPickedUp)
            Destroy(gameObject);
    }
}
