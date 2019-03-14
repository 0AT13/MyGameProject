using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public GameObject Hero;
    public Transform ExitPosition;

    private static bool ifHeroWasHere = false;

    void Start ()
    {
        if (ifHeroWasHere)
            Hero.transform.position = ExitPosition.transform.position;
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            ifHeroWasHere = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            ifHeroWasHere = false;
    }
}
