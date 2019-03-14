using UnityEngine;

public class CameraMove : MonoBehaviour {

    //Привязування камери до гравця
    public GameObject player;

    void Update ()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}
