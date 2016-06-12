using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform start;
    public Transform game;
    public Transform win;

    public static bool isWin = false;

    void Start()
    {
        startGame();
    }

	public void startGame()
    {
        start.gameObject.SetActive(true);
        game.gameObject.SetActive(false);
        win.gameObject.SetActive(false);
    }

    public void inGame()
    {
        start.gameObject.SetActive(false);
        game.gameObject.SetActive(true);
        win.gameObject.SetActive(false);
    }

    public void winGame()
    {
        start.gameObject.SetActive(false);
        game.gameObject.SetActive(false);
        win.gameObject.SetActive(true);
    }
}
