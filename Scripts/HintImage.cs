using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintImage : MonoBehaviour {

    public GameObject hintPrefab;
    public Transform hintPosition;

    private bool chestActive = true;
    private GameObject itemInstance;

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player" && chestActive) //Створення підсказки 
        {
            itemInstance = Instantiate(hintPrefab, hintPosition.position, hintPosition.rotation) as GameObject;
        }
    }

    void OnTriggerStay2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && gameObject.tag == "Chest" && chestActive) //Видалення підсказки після відкриття скрині
        {
            chestActive = false;
            Destroy(itemInstance);
        }
    }

    void OnTriggerExit2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player" && chestActive) //Видалення підсказки 
        {
            Destroy(itemInstance);
        }
    }
}
