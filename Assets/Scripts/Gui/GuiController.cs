using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiController : MonoBehaviour
{
    public static GuiController Instance { private set; get; }

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GuiMenu guiMenu;

    private Health subscribedHealth;
    private int score = 0;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (guiMenu.isActiveAndEnabled) guiMenu.CloseMenu();
            else guiMenu.OpenMenu(GuiMenu.MenuMode.pausemenu);
        } 
    }

    public void AddScore(int addition)
    {
        score += addition;
        scoreText.text = "SCORE: " + score.ToString("00000");

        if (score >= 10000)
        {
            GameWin();
        }
    }

    public void SubscribeHealth(Health health)
    {
        if (subscribedHealth != null) subscribedHealth.OnHealthChange -= Health_OnHealthChange;

        health.OnHealthChange += Health_OnHealthChange;
        subscribedHealth = health;
    }

    private void Health_OnHealthChange(object sender, Vector2Int e)
    {
        string newText = "HP: ";
        for (int i = 1; i <= e.y; i++)
        {
            if (i > e.x) newText += "-";
            else newText += "+";
        }
        hpText.text = newText;

        if (e.x == 0) GameOver();
    }

    private void GameOver()
    {
        gameEnded = true;
        guiMenu.OpenMenu(GuiMenu.MenuMode.gameover);
    }

    private void GameWin()
    {
        gameEnded = true;
        guiMenu.OpenMenu(GuiMenu.MenuMode.victory);
    }
}
