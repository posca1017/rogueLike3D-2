using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rd;
    public float moveTime = 0.1f;
    public bool isMoving = false;

    public LayerMask blockingLayer;
    private BoxCollider boxCollider;

    public Animator animator;
    private DamageText damageText;

    private Text foodText;
    private int foodPoint = 10;
    private int portionPoint = 20;
    public Direction direction;
    private int walkCount = 0;

    private GameObject itemMessage;
    private Text itemMessageText;

    private FixedJoystick joyStick;

    private EffectScript effect;

    private bool isDamage = false;
    private bool isUseItem = false;
    private bool isGetItem = false;
    private string getItemMessage = "を手に入れた";
    private string useItemMessage = "を使った";

    private Camera camera;

    public Dictionary<string, bool> itemFlags = new Dictionary<string, bool>();
    [SerializeField]
    private ItemDataBase itemDataBase;
    private ItemManager itemManager;
    [SerializeField]
    private AudioClip WalkClip;
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip getItemClip;

    public enum Direction
    {
        North,
        South,
        East,
        West,
    }

    void Start()
    {
        effect = GameObject.Find("effects").GetComponent<EffectScript>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        foreach(Item item in itemDataBase.GetItemLists()) //アイテム情報はGameManagerと共有が必要
        {
            itemFlags.Add(item.GetItemName(),false); //全てのアイテム情報を取得。ただし持っていないことにする
        }

        itemFlags["Portion"] = false;

        /*foreach (var itemData in itemDataBase2.GetItemLists())
        {
            itemFlags["FlashLight"] = true;
            itemFlags["BroadSword"] = true;
            itemFlags["HandGun"] = true;
        }*/
        
        joyStick = FindObjectOfType<FixedJoystick>();
        itemMessage = GameObject.Find("ItemMessage");
        itemMessageText = itemMessage.transform.GetChild(0).gameObject.GetComponent<Text>();
        itemMessage.SetActive(false);
        damageText = GetComponent<DamageText>();
        rd = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //GameManager.instance.BackStatus(level, food, MaxHP, HP, ATK);  //できればGameManagerで一括管理したい
        /*food = GameManager.instance.food;
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        foodText.text = "Food " + food.ToString() + "%";*/
        direction = Direction.North;
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        camera.transform.position = transform.position+new Vector3(0f,4f,-6f);
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool GetItemFlag(string itemName) //アイテム所持確認→bool型でtrueは保持・falseは持ってない。
    {
        return itemFlags[itemName];
    }

    public void IsDrinking()
    {
        animator.SetBool("Drink", true);
        isUseItem = true;
    }

    public void Drinked()
    {
        animator.SetBool("Drink", false);
        isUseItem = false;
    }

    void Update()
    {
        if(itemFlags["Portion"]==true)
        {
            Debug.Log("wesrt");
        }
        if (!isUseItem)
        {
            if (!isDamage)
            {
                if (!GameManager.instance.playerTurn)
                {
                    return;
                }

                int horizontal = (int)Input.GetAxisRaw("Horizontal");
                int vertical = (int)Input.GetAxisRaw("Vertical");

                float x = joyStick.Horizontal;
                float y = joyStick.Vertical;
                //Debug.Log(x);
                if (x > 0.5f)
                {
                    horizontal = 1;
                }

                else if (x < -0.5f)
                {
                    horizontal = -1;
                }

                if (y > 0.5f)
                {
                    vertical = 1;
                }

                else if (y < -0.5f)
                {
                    vertical = -1;
                }


                if (horizontal != 0)
                {
                    vertical = 0;

                    if (horizontal == 1)
                    {
                        direction = Direction.East;
                    }

                    else if (horizontal == -1)
                    {
                        direction = Direction.West;
                    }

                }

                else if (vertical != 0)
                {
                    horizontal = 0;

                    if (vertical == 1)
                    {
                        direction = Direction.North;
                    }

                    else if (vertical == -1)
                    {
                        direction = Direction.South;
                    }
                }



                if (horizontal != 0 || vertical != 0)
                {
                    ATMove(horizontal, vertical);
                }
                //GameManager.instance.UpdateStatus(level, food, MaxHP, HP, ATK);
            }
        }



    }

    public  bool Move(int horizontal,int vertical,out RaycastHit hit)
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(horizontal, 0, vertical); //endが移動予定の位置。

        boxCollider.enabled = false; //rayを外に飛ばせなくなるので一時的にfalseにする。

        if(Physics.Linecast(start,end,out hit/*,blockingLayer*/)) //RaycastHitの中身を書き換えるのが目的なので、if文の中に処理が書いてなくてもOK　4つめにlayerの引数を入れると、指定のlayerのときのみRayを飛ばす。
        {

        }

        //hit = Physics.Linecast(start, end,out hit,blockingLayer); //start・endの間に光を飛ばし、当たったオブジェクトのレイヤーを取得してhitに代入にする。
        boxCollider.enabled = true;


        if (!isMoving) //動いていないかつ移動先にオブジェクトがないとき　→trigger処理をやめる場合は変える必要あり。
        {
            //Debug.Log(hit.transform.tag);

            if(hit.transform.tag=="Wall")
            {
                return false;
            }
            else if (hit.transform.tag=="OutWall")
            {
                return false;
            }

            else if (hit.transform.tag == "Recovery" || hit.transform.tag=="Food")
            {
                isGetItem = true;
                string name = hit.transform.name;
                Debug.Log(name);
                StartCoroutine(ItemMessageWindow(name));
                itemManager.AddItem(name);
                //itemFlags["Portion"] = true;

                /*if(HP>MaxHP)
                {
                    HP = MaxHP;
                }*/

                hit.transform.gameObject.SetActive(false);
                //GameManager.instance.UpdateStatus(level, food, MaxHP, HP, ATK);
            }

            /*else if (hit.transform.tag == "Food")
            {
                string name = hit.transform.tag;
                StartCoroutine(ItemMessageWindow(name));
                food += foodPoint;
                hit.transform.gameObject.SetActive(false);
                GameManager.instance.UpdateStatus(level, food, MaxHP, HP, ATK);
            }*/
            else if (hit.transform.tag == "Exit")
            {
                Invoke("Restart", 1f);
                enabled = false;
            }

            else if(hit.transform.tag=="Enemy")
            {
                return false;
            }

            StartCoroutine(Movement(end)); //endが移動予定の位置。
            return true;
        }
        return false;
    }

    public IEnumerator ItemMessageWindow(string name)
    {

        if (isGetItem) //〇〇を手に入れたの表示
        {
            itemMessage.SetActive(true);
            itemMessageText.text = name + getItemMessage;
            AudioSource.PlayClipAtPoint(getItemClip, camera.transform.position);

            yield return new WaitForSeconds(1.0f);
            isGetItem = false;
            itemMessage.SetActive(false);
        }

        if (isUseItem) //〇〇を使ったの表示
        {
            itemMessage.SetActive(true);
            itemMessageText.text = name + useItemMessage;
            AudioSource.PlayClipAtPoint(getItemClip, camera.transform.position);

            yield return new WaitForSeconds(1.0f);
            Drinked();
            itemMessage.SetActive(false);
        }

    }

    private void ChangeDirection()
    {
        if (direction == Direction.North)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direction == Direction.South)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }
        
        else if (direction == Direction.East)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        
        else if (direction == Direction.West)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
       
    }


    IEnumerator Movement(Vector3 end)
    {
        AudioSource.PlayClipAtPoint(WalkClip,camera.transform.position);
        ChangeDirection();
        isMoving = true;
        animator.SetTrigger("Move");
        float remainingDistance = (transform.position - end).sqrMagnitude; //現在地と移動予定の位置を差し引く。
        
        while(remainingDistance>float.Epsilon) //epsilon==限りなく0に近い数字
        {
            transform.position = Vector3.MoveTowards(transform.position,end,1f/moveTime*Time.deltaTime); //現在地からendまで徐々に移動させる
            remainingDistance = (transform.position - end).sqrMagnitude; //移動後の座標を算出→whileの条件に当てはまるか再度チェックする
            
            yield return null;
        }
        camera.transform.position = transform.position + new Vector3(0f, 3f, -4f);
        transform.position = end; //目的地に到着させる。
        isMoving = false;
        transform.position = new Vector3(transform.position.x, 0,transform.position.z);
        CheckFood();
    }

    private void Walk()
    {
        walkCount+= 1;

        if(walkCount>10)
        {
            walkCount = 0;
            GameManager.instance.food--;
            //GameManager.instance.UpdateStatus(level, food,MaxHP, HP, ATK);

        }
    }


    public void ATMove(int horizontal,int vertical) //行動したときに呼ばれるスクリプト
    {
        Walk();

        RaycastHit hit; //あってるか調べる。
        bool canMove = Move(horizontal, vertical,out hit);
        
        if(hit.transform==null) //ここでreturnすることでキャラクターを矢印キーで連続して動けなくしてる。
        {
            GameManager.instance.playerTurn = false;
            return;
        }

        Damage hitComponent = hit.transform.GetComponent<Damage>();

        if(!canMove && hitComponent!=null) //移動先が壁か何かにぶつかるとき
        {
            OnCantMove(hitComponent);
        }

        CheckFood();
        GameManager.instance.playerTurn = false;
    }

    public void OnCantMove(Damage hit)
    {
        ChangeDirection();
        hit.AttackDamage(GameManager.instance.ATK); //移動先に壁があるときは壁に攻撃する

        animator.SetTrigger("Chop");
        AudioSource.PlayClipAtPoint(attackClip, camera.transform.position);
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void CheckFood()
    {
        if(GameManager.instance.food<=0　|| GameManager.instance.HP<=0)
        {
            GameManager.instance.GameOver(); 
        }
    }

    /*public void OnDisable() //オブジェクトが非アクティブのときに呼ばれる。
    {
        GameManager.instance.food = food;
    }*/

    public void EnemyAttack(int loss)
    {
        effect.MakeEffects("Damage");
        animator.SetTrigger("Hit");
        isDamage = true;
        GameManager.instance.HP -= loss;
        damageText.ViewDamage(loss);
        //GameManager.instance.UpdateStatus(level, food,MaxHP, HP, ATK);
        StartCoroutine("IsDamage");
        CheckFood();
    }

    IEnumerator IsDamage()
    {
        yield return new WaitForSeconds(1.3f);
        isDamage = false;
    }


}
