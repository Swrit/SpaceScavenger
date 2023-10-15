using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GuiMenu : MonoBehaviour
{
    [SerializeField] private bool startOpen = false;
    [SerializeField] private TextMeshProUGUI MenuTitle;

    public enum MenuMode
    {
        mainmenu,
        pausemenu,
        victory,
        gameover,
    }

    [SerializeField] private List<GuiMenuOption> guiMenuOptions;
    [SerializeField] private List<GuiMenuOption> mainMenu;
    [SerializeField] private List<GuiMenuOption> pauseMenu;
    [SerializeField] private List<GuiMenuOption> endMenu;

    [SerializeField] private AudioClip clickSound;

    private string gameScene = "GameScene";
    private string menuScene = "MenuScene";

    private List<GuiMenuOption> currentOptions = new List<GuiMenuOption>();
    private int selectedOption;

    private void Start()
    {
        currentOptions = new List<GuiMenuOption>(guiMenuOptions);
        ClearMenu();
        if (startOpen) OpenMenu(MenuMode.mainmenu);
        else CloseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (selectedOption > 0) SelectOption(selectedOption - 1);
            else SelectOption(currentOptions.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (selectedOption >= currentOptions.Count - 1) SelectOption(0);
            else SelectOption(selectedOption + 1);
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
            currentOptions[selectedOption].PerformAction();
        }

    }

    public void OpenMenu(MenuMode menuMode)
    {
        Time.timeScale = float.Epsilon;
        gameObject.SetActive(true);
        switch (menuMode)
        {
            case MenuMode.mainmenu:
                MenuTitle.text = "MAIN MENU";
                LoadMenuOptions(mainMenu);
                break;
            case MenuMode.pausemenu:
                MenuTitle.text = "PAUSE MENU";
                LoadMenuOptions(pauseMenu);
                break;
            case MenuMode.victory:
                MenuTitle.text = "VICTORY";
                LoadMenuOptions(endMenu);
                break;
            case MenuMode.gameover:
                MenuTitle.text = "GAME OVER";
                LoadMenuOptions(endMenu);
                break;

        }
        SelectOption(0);
        
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;

        ClearMenu();
        gameObject.SetActive(false);
    }

    private void ClearMenu()
    {
        foreach (GuiMenuOption gmo in currentOptions)
        {
            gmo.Deselect();
            gmo.gameObject.SetActive(false);
        }
        currentOptions.Clear();
        selectedOption = 0;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    private void SelectOption(int index)
    {
        SoundManager.Instance.PlaySound(clickSound);

        currentOptions[selectedOption].Deselect();
        selectedOption = index;
        currentOptions[index].Select();
    }

    private void LoadMenuOptions(List<GuiMenuOption> guiMenuOptions)
    {
        foreach (GuiMenuOption gmo in guiMenuOptions)
        {
            gmo.gameObject.SetActive(true);
            currentOptions.Add(gmo);
        }
    }

}
