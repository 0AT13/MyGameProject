using UnityEngine;

public class Minimap : MonoBehaviour {

    //Мінікарта
    public Transform player;

    private Camera map;
    static float distance = 15;

    void Start ()
    {
        map = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    public void ZoomIn()
    {
        if(distance >= 10)
        {
            distance -= 3;
            map.orthographicSize = distance;
            LateUpdate();
        }
        
    }

    public void ZoomOut()
    {
        if (distance <= 20)
        {
            distance += 3;
            map.orthographicSize = distance;
            LateUpdate();
        } 
    }
}
