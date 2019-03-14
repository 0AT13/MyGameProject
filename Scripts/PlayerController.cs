using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerController : MonoBehaviour {

    [Header("Physycs")]
    //Фізика персонажа
    private Rigidbody2D rigidBody;

    //Фізичне тіло персонажа
    private BoxCollider2D boxBody;

    //Змінна меча
    private Collider2D sword;

    //Фізичні сили
    public float moveSpeed = 10f;
    public float hoverForce = 0f;

    [Space]

    //Кількість монет
    public TextMeshProUGUI coinCounter;
    private int coins;

    [Header("Animations")]
    //Анімаційні змінні
    private bool faceRight = true;
    private Animator anim;

    //Змінні для перевірки знаходження на землі
    private bool isGrounded = false;
    private float groundRadius = 0.2f;

    //Об`єкти перевірки знаходження на землі, коробках
    public Transform gorundCheck;
    public LayerMask whatIsGround;

    [Space]

    //Змінна для перевірки знаходження персонажа на коробці
    private bool crateCheck = false;

    //Змінна для перевірки чи живий персонаж
    private bool dead = false;


    [Header("Hero Stats")]
    //Показники очків здоров'я та енергії
    public Slider HP;
    public Slider MP;

    [Space]

    [Header("Die Screen Settings")]
    //Екран смерті
    public GameObject dieScreen;

    //Отримання ушкоджень
    public void DamageHero(float Damage)
    {
        HP.value -= Damage;

        anim.SetTrigger("GetDamage");
        
        if(HP.value <= 0)
        {
            dead = true;
            anim.ResetTrigger("GetDamage");

            Time.timeScale = 0f;

            dieScreen.SetActive(true);
        }
    }

	void Start ()//Отримання змінних при завантаженні об'єкта героя
    {
        Load();

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        boxBody = GetComponent<BoxCollider2D>();
        sword = GameObject.Find("Sword").GetComponentInChildren<Collider2D>();

        dieScreen.SetActive(false);

        Cursor.visible = true;
    }

    private float inputVertical;
    private bool isClimbing;

    [Header("Ladder layer")]
    public LayerMask whatIsLadder;

    void FixedUpdate() //Програвання анімацій
    {
        Run();

        isGrounded = Physics2D.OverlapCircle(gorundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", isGrounded);
        anim.SetFloat("vSpeed", rigidBody.velocity.y);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 1f, whatIsLadder);

        if (hitInfo.collider != null) //Рух по драбині
        {
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, inputVertical * (moveSpeed - 1));
            rigidBody.gravityScale = 0;
        }
        else
        {
            rigidBody.gravityScale = 1;
        }

        anim.SetBool("Climbing", isClimbing);

        if (!isGrounded) //Перевірка на знаходження на землі
            return;
    }
	
	void Run () //Метод в якому описане прискорення персонажа при русі, а також програвання анімації руху
    {
        float moveX = Input.GetAxis("Horizontal");
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

        rigidBody.velocity = new Vector2(moveX * moveSpeed, rigidBody.velocity.y);

        if (moveX > 0 && !faceRight)
            Flip();
        else if(moveX < 0 && faceRight)
            Flip();
	}

    void Flip() //Обертання моделі персонажа при русі в протилежну сторону
    {
        faceRight = !faceRight;

        Vector3 theScale = transform.localScale;

        theScale.x *= -1;

        transform.localScale = theScale;
    }

    void Update() //Дії які відбуваються по натисканню певних клавіш
    {

        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S) && !dead) //Стрибок
        {
            anim.SetBool("Ground", false);
            anim.SetTrigger("Jump");
            rigidBody.AddForce(new Vector2(0, 50));
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && !crateCheck && !dead) //Падіння з платформи вниз, з перевіркою чи не стоїть герой на коробці
        {
            boxBody.isTrigger = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && !dead && MP.value > 0) //Використання здібностей
        {
            Magic();

            MP.value -= 3;
        }

        if(Input.GetMouseButtonDown(0) && !dead)
        {
            anim.SetTrigger("Attack");
        }
    }

    //Методи бойової системи-------------------------------------------------------------------
    public void Attack()
    {
        sword.enabled = true;
    }

    public void NoAttack()
    {
        sword.enabled = false;

        anim.ResetTrigger("Attack");
    }

    [Header("Magic")]
    public GameObject magicPrefab;
    public Transform magicStart;
    private GameObject magicInstance;
    private Rigidbody2D magicRigidbody;
    private SpriteRenderer magicSprite;

    public void Magic()
    {
        magicInstance = Instantiate(magicPrefab, magicStart.position, magicStart.rotation) as GameObject;

        magicRigidbody = magicInstance.GetComponent<Rigidbody2D>();
        magicSprite = magicInstance.GetComponent<SpriteRenderer>();

        if (faceRight)
        {
            magicRigidbody.AddForce(new Vector2(200, 0));
            magicSprite.flipX = false;
        }
        else if (!faceRight)
        {
            magicRigidbody.AddForce(new Vector2(-200, 0));
            magicSprite.flipX = true;
        }
    }
    //-------------------------------------------------------------------------------------

    //Методи пов'язані з іншими об'єктами----------------------------------------------------
    void OnCollisionEnter2D(Collision2D col) //Зіктнення з об'єктами
    {
        if (col.gameObject.tag == "Crate")
        {
            crateCheck = true;
        }
    }

    void OnCollisionExit2D(Collision2D col) //Відсутність зіткнення
    {
        crateCheck = false;
    }

    void OnTriggerEnter2D(Collider2D col) //Взаємодія з об'єктами в які потрапляє персонаж
    {
        if (col.gameObject.tag == "Coin") //Рахунок монет
        {
            Destroy(col.gameObject);
            coins++;
            coinCounter.text = "" + coins;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 8) //Перевірка на вихід зі слою землі
        {
            boxBody.isTrigger = false; 
        }
    }

    void OnTriggerStay2D(Collider2D col) //Взаємодія з об'єктами в яких знаходиться персонаж
    {
        if (col.gameObject.tag == "ChangeLocation" && Input.GetKeyDown(KeyCode.W)) //Перехід до іншої локації
        {
            LoadLevel(col.gameObject.name);
        }
    }
    //-------------------------------------------------------------------------------------
    
    //Перехід на іншу локацію--------------------------------------------------------------
    [Header("Loading Screen Settings")]
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;

    public void LoadLevel(string sceneName) //Завантаження рівня
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName) //Метод завантаження
    {
        Save();

        Scene currentScene = SceneManager.GetActiveScene();

        loadingScreen.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }

        SceneManager.UnloadSceneAsync(currentScene);
    }
    //-------------------------------------------------------------------------------------

    //Структура збереження інформації про персонажа
    [System.Serializable]
    private struct PlayerParameters
    {
        public float SaveHP;
        public float SaveMP;
        public bool SaveDead;
        public int SaveCoins;
    }

    //Метод збереження данних персонажа
    public void Save()
    {
        PlayerParameters SaveParameters = new PlayerParameters();
        SaveParameters.SaveHP = HP.value;
        SaveParameters.SaveMP = MP.value;
        SaveParameters.SaveDead = dead;
        SaveParameters.SaveCoins = coins;

        if (!Directory.Exists(Application.dataPath + "/saves"))
            Directory.CreateDirectory(Application.dataPath + "/saves");
        FileStream fs = new FileStream(Application.dataPath + "/saves/save.sm", FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, SaveParameters);
        fs.Close();
    }

    //Метод загрузки данних персонажа
    void Load()
    {
        if (File.Exists(Application.dataPath + "/saves/save.sm"))
        {
            FileStream fs = new FileStream(Application.dataPath + "/saves/save.sm", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                PlayerParameters SaveParameters = (PlayerParameters)formatter.Deserialize(fs);

                HP.value = SaveParameters.SaveHP;
                MP.value = SaveParameters.SaveMP;
                dead = SaveParameters.SaveDead;
                coins = SaveParameters.SaveCoins;

                coinCounter.text = "" + coins;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                fs.Close();
            }
        }
        else
        {
            SetStandartParameters();
        }

        if (dead)
        {
            SetStandartParameters();
        }
    }

    //Параметри по замовчуванню
    void SetStandartParameters()
    {
        HP.value = 9;
        MP.value = 9;

        dead = false;
    }

    public void HeroDead()
    {
        dieScreen.SetActive(false);
        Scene currentScene = SceneManager.GetActiveScene();

        string currentSceneName = currentScene.name;

        LoadLevel(currentSceneName);

        Time.timeScale = 1f;
    }
}
