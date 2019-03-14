using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //Меню паузи
    public static bool GameIsPaused = false;

    public GameObject Hero;
    public GameObject PauseMenuUI;
    public GameObject Minimap;
    public GameObject HeroUI;
    public GameObject Inventory;

	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
                
        }
	}

    public void Resume()
    {
        Minimap.SetActive(true);
        HeroUI.SetActive(true);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.transform.position = new Vector3(Hero.transform.position.x, Hero.transform.position.y);
        Minimap.SetActive(false);
        HeroUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        if(Inventory.activeInHierarchy == true)
        {
            Inventory.SetActive(false);
        }

        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

        PlayerController Saving = Hero.GetComponent<PlayerController>();

        Saving.Save();
    }

    public void QuitGame()
    {
        Application.Quit();

        PlayerController Saving = Hero.GetComponent<PlayerController>();

        Saving.Save();
    }
}

