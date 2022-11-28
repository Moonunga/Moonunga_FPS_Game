using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public int enemyNumber;

    [Header("------Player Stuff-------")]
    public GameObject player;
    public playerController playerScript;
    public GameObject spawnPosition;

    [Header("------UI-----------")]
    public GameObject pauseMenu;
    public GameObject DeadMenu;
    public GameObject WinMenu;
    public GameObject playerDamageFlash;
    public Image playerHPBar;
    public Text enemyCountText;


    public bool isPaused;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        spawnPosition = GameObject.FindGameObjectWithTag("Spawn Position");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")&& !DeadMenu.activeSelf && !WinMenu.activeSelf)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                Pause();
            else
                UnPause();
        }
    }
    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator playerDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }

    public void checkEnemyTotal()
    {
        enemyNumber--;
        enemyCountText.text = enemyNumber.ToString("f0");
        if (enemyNumber <= 0)
        {
            WinMenu.SetActive(true);
            Pause();
        }
    }
}
