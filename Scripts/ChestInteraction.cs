using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour {

    public GameObject itemPrefab;
    public Transform chestOutput;
    public int coinsNumber;

    public List<GameObject> chestItems = new List<GameObject>();

    private Animator anim;
    private bool chestActive = true;
    private GameObject itemInstance;
    private Rigidbody2D rigidBody;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerStay2D (Collider2D col)
    { 
        if (col.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && chestActive) //Відкриття скрині
        {
            anim.SetBool("Opened", chestActive);

            for (int i = 0; i < coinsNumber; i++) //Викидування монет
                itemInstanceFunction(itemPrefab);

            for (int i = 0; i < chestItems.Count; i++)
                itemInstanceFunction(chestItems[i]);

            chestActive = false;
        }
	}

    void itemInstanceFunction(GameObject itemPrefab) //Метод викидування об'єктів зі скрині
    {
        itemInstance = Instantiate(itemPrefab, chestOutput.position, chestOutput.rotation) as GameObject;

        rigidBody = itemInstance.GetComponent<Rigidbody2D>();
        rigidBody.AddForce(new Vector3(Random.Range(-23f, 23f), Random.Range(23f, 25f), 0));
    }
}
