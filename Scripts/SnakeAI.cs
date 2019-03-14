using UnityEngine;

public class SnakeAI : MonoBehaviour{

    public float speed;

    public float HP = 2;
    private bool movingRight = true;

    private Animator anim;

    public Transform groundDetection;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);

        if (groundInfo.collider == false || groundInfo.collider.gameObject.tag == "Spikes")
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void GetDamage(float dmg)
    {
        HP -= dmg;

        anim.SetTrigger("GetDamage");
    }
}
