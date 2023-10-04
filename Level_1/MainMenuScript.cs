using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject Menu;
    public  bool isMenu = false;

    private void Update()
    {
        OpenMenu();
    }


    void OpenMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isMenu = !isMenu;
            Cursor.visible = true;
        }

        ActiveMenu();
    }

    void ActiveMenu()
    {
        if(isMenu)
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void ContinueGame()
    {
        isMenu = false;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
