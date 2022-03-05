using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;　//シングルトン化してるので壊れないオブジェクト
    //BoardManager boardManager;

    public bool playerTurn = true;
    public bool enemiesMoving = false;

    private bool doingSetUp;
    public Text GameOverText;
    public GameObject levelImage;

    public int floorLevel = 1;
    public int level = 1;
    public int food = 100;
    public int maxFood = 100;
    public int maxHP = 50;
    public int HP = 50;
    public int ATK = 1;
    public int Exp = 0;
    public int LevelUpExp = 1;

    private Text floorLevelText;
    private Text levelText;
    private Text foodText;
    private Text hpText;
    private Text atkText;

    public GameObject levelUpEffect;
    public AudioClip levelUpClip;
    private GameObject mainCamera;
    private Transform player;



    private List<Enemy> enemies;


    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }

        else if(instance != this) //中身がシングルトンのこのゲームオブジェクトじゃないとき
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        InitGame();

    }

    private void SetUpUI()
    {
        //GameOverText = GameObject.Find("GameOver").GetComponent<Text>();
        floorLevelText = GameObject.Find("FloorLevelText").GetComponent<Text>();
        levelText = GameObject.Find("PlayerLevelText").GetComponent<Text>();
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        hpText = GameObject.Find("HPText").GetComponent<Text>();
        //atkText = GameObject.Find("ATKText").GetComponent<Text>();

        floorLevelText.text= floorLevel.ToString() + "F";
        levelText.text = "LV."+level.ToString();
        hpText.text = "HP" + HP.ToString() + "/" + maxHP.ToString();
        foodText.text = "Food " + food.ToString() + "%";

        //atkText.text = "ATK:" + ATK.ToString();
    }

    public void BackStatus(int Plevel,int Pfood,int PMaxHP,int PHP,int PATK)
    {
        Plevel = level;
        Pfood = food;
        PMaxHP = maxHP;
        PHP = HP;
        PATK = ATK;

        floorLevelText.text = floorLevel.ToString() + "F";
        levelText.text = "LV." + level.ToString();
        hpText.text = "HP" + HP.ToString() + "/" + maxHP.ToString();
        foodText.text = "Food " + food.ToString() + "%";

    }

    public void UpdateStatus()
    {
        /*level = Plevel;
        food = Pfood;
        HP = PHP;
        ATK = PATK;*/

        floorLevelText.text = floorLevel.ToString() + "F";
        levelText.text = "LV." + level.ToString();
        hpText.text = "HP" + HP.ToString() + "/" + maxHP.ToString();
        foodText.text = "Food " + food.ToString() + "%";
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]  //ゲーム開始時に一度だけロードする
    static public void Call()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded(Scene next,LoadSceneMode a)
    {
        instance.floorLevel++;
        instance.InitGame(); //ロードするたびにマップを再生成するために入れる。(階が変わるたびにマップを作る)

    }

    public void InitGame() //BoardManagerのセットアップシーンを呼び出す。
    {
        doingSetUp = true;
        levelImage = GameObject.Find("LevelImage");
        //floorLevelText = GameObject.Find("LevelText").GetComponent<Text>();
        //floorLevelText.text = "Day:" + floorlevel;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", 2f);

        enemies.Clear(); //階が変わったので敵を初期化する。

        SetUpUI();
        //boardManager.SetupScene(level);
    }

    public void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetUp = false;
    }

    void Update()
    {
        SetUpUI();
        if(playerTurn || enemiesMoving || doingSetUp)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(0.1f);

        if(enemies.Count==0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        for(int i=0;i<enemies.Count;i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(0.1f);
        }
        playerTurn = true;

        enemiesMoving = false;
    }

    public void GameOver()
    {
        GameOverText.text = "Game Over";
        levelImage.SetActive(true);
        enabled = false; //呼ぶのはプレイヤーなので、プレイヤーが表示されなくなる。
    }

    public void AddEnemy(Enemy script)
    {
        enemies.Add(script);
    }

    public void DestroyEnemy(Enemy script)
    {
        enemies.Remove(script);
    }

    public void AddExp(int exp)
    {
        this.Exp += exp;
        LevelUP();

    }

    private void LevelUP()
    {
        if(this.Exp>=LevelUpExp)
        {
            if (player == null)
            {
                player=GameObject.FindWithTag("Player").GetComponent<Transform>();
            }

            maxHP += 10;
            HP = maxHP;
            ATK += 2;
            food = maxFood;
            LevelUpExp += 30;
            var obj=Instantiate(levelUpEffect, player.transform.position, Quaternion.identity);
            StartCoroutine(DestroyEffect(obj));
            AudioSource.PlayClipAtPoint(levelUpClip, mainCamera.transform.position);

        }
    }

    IEnumerator DestroyEffect(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(obj);
    }

}
