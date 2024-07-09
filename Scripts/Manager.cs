using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
};
public class Manager : Loader <Manager>
{
    //public static Manager instance = null;
    [SerializeField]
    int totalWaves = 10;
    [SerializeField]
    TextMeshProUGUI totalMoneyLabel;
    [SerializeField]
    TextMeshProUGUI currentWave;
    [SerializeField]
    TextMeshProUGUI totalEscapedLabel;
    [SerializeField]
    TextMeshProUGUI playBtnLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enimies;
/*    [SerializeField]
    int maxenemiesOnScreen;*/
    [SerializeField]
    int totalEnemies =5;
    [SerializeField]
    int enemiesPerSpawn;

    int waveMunber = 0;
    int totalMoney = 10;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKiller = 0;
    //int whichEnemiesToSpawn = 0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;
    AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

    //int enemiesOnScreen = 0;

   // public static object Instance { get; internal set; }
    const float spawDelay = 2f; //spawn delay in seconds

    public int TotalEscaped
    {
        get { return totalEscaped; }
        set { totalEscaped = value; }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set { roundEscaped = value; }
    }
    public int TotalKiller
    {
        get { return totalKiller; }
        set { totalKiller = value; }
    }

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotalMoney.ToString();
        }
    }

    public AudioSource AudioSource
    {
        get { return audioSource; }
    }

    /*    void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

        }*/

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
    }
    private void Update()
    {
        HandleEscape();
    }



    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enimies[Random.Range(0, enemiesToSpawn)]);
                    newEnemy.transform.position = spawnPoint.transform.position;
                    //enemiesOnScreen += 1;
                }
            }
            yield return new WaitForSeconds(spawDelay);
            StartCoroutine(Spawn());

        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }

    public void addMoney(int amout)
    {
        TotalMoney += amout;
    }
    public void subtracMoney(int amout)
    {
        TotalMoney -= amout;
    }

    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Escaped" + TotalEscaped + "/10";
        if ((RoundEscaped + TotalKiller) == totalEnemies) 
        { 
            if(waveMunber <= enimies.Length)
            {
                enemiesToSpawn = waveMunber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
       if (totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
       else if (waveMunber == 0 && (RoundEscaped + TotalKiller) == 0)
        {
            currentState = gameStatus.play;
        }
       else if (waveMunber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
       else
        {
            currentState = gameStatus.next;
        }
    }

    public void ShowMenu()
    {
        switch(currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Play Again!";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);

                break;
            case gameStatus.next:
                playBtnLabel.text = "Next Wave";
                break;

            case gameStatus.play:
                playBtnLabel.text = "Play";
                break;

            case gameStatus.win:
                playBtnLabel.text = "Play";
                break;
        }
        playBtn.gameObject.SetActive(true);
    }

    public void PlayButtonPressed()
    {
        Debug.Log("Play button Preseed");

        switch (currentState)
        {
            case gameStatus.next:
                waveMunber += 1;
                totalEnemies += waveMunber;
                break;
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;
                //enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSide();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped" + TotalEscaped + "/10";
                AudioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;

        }
        DestroyEnemies();
        TotalKiller = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave" + (waveMunber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
/*    public void removeEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }*/
}
