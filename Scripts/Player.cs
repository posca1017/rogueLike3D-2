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
    private string getItemMessage = "����ɓ��ꂽ";
    private string useItemMessage = "���g����";

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
        foreach(Item item in itemDataBase.GetItemLists()) //�A�C�e������GameManager�Ƌ��L���K�v
        {
            itemFlags.Add(item.GetItemName(),false); //�S�ẴA�C�e�������擾�B�����������Ă��Ȃ����Ƃɂ���
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
        //GameManager.instance.BackStatus(level, food, MaxHP, HP, ATK);  //�ł����GameManager�ňꊇ�Ǘ�������
        /*food = GameManager.instance.food;
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        foodText.text = "Food " + food.ToString() + "%";*/
        direction = Direction.North;
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        camera.transform.position = transform.position+new Vector3(0f,4f,-6f);
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool GetItemFlag(string itemName) //�A�C�e�������m�F��bool�^��true�͕ێ��Efalse�͎����ĂȂ��B
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
        Vector3 end = start + new Vector3(horizontal, 0, vertical); //end���ړ��\��̈ʒu�B

        boxCollider.enabled = false; //ray���O�ɔ�΂��Ȃ��Ȃ�̂ňꎞ�I��false�ɂ���B

        if(Physics.Linecast(start,end,out hit/*,blockingLayer*/)) //RaycastHit�̒��g������������̂��ړI�Ȃ̂ŁAif���̒��ɏ����������ĂȂ��Ă�OK�@4�߂�layer�̈���������ƁA�w���layer�̂Ƃ��̂�Ray���΂��B
        {

        }

        //hit = Physics.Linecast(start, end,out hit,blockingLayer); //start�Eend�̊ԂɌ����΂��A���������I�u�W�F�N�g�̃��C���[���擾����hit�ɑ���ɂ���B
        boxCollider.enabled = true;


        if (!isMoving) //�����Ă��Ȃ����ړ���ɃI�u�W�F�N�g���Ȃ��Ƃ��@��trigger��������߂�ꍇ�͕ς���K�v����B
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

            StartCoroutine(Movement(end)); //end���ړ��\��̈ʒu�B
            return true;
        }
        return false;
    }

    public IEnumerator ItemMessageWindow(string name)
    {

        if (isGetItem) //�Z�Z����ɓ��ꂽ�̕\��
        {
            itemMessage.SetActive(true);
            itemMessageText.text = name + getItemMessage;
            AudioSource.PlayClipAtPoint(getItemClip, camera.transform.position);

            yield return new WaitForSeconds(1.0f);
            isGetItem = false;
            itemMessage.SetActive(false);
        }

        if (isUseItem) //�Z�Z���g�����̕\��
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
        float remainingDistance = (transform.position - end).sqrMagnitude; //���ݒn�ƈړ��\��̈ʒu�����������B
        
        while(remainingDistance>float.Epsilon) //epsilon==����Ȃ�0�ɋ߂�����
        {
            transform.position = Vector3.MoveTowards(transform.position,end,1f/moveTime*Time.deltaTime); //���ݒn����end�܂ŏ��X�Ɉړ�������
            remainingDistance = (transform.position - end).sqrMagnitude; //�ړ���̍��W���Z�o��while�̏����ɓ��Ă͂܂邩�ēx�`�F�b�N����
            
            yield return null;
        }
        camera.transform.position = transform.position + new Vector3(0f, 3f, -4f);
        transform.position = end; //�ړI�n�ɓ���������B
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


    public void ATMove(int horizontal,int vertical) //�s�������Ƃ��ɌĂ΂��X�N���v�g
    {
        Walk();

        RaycastHit hit; //�����Ă邩���ׂ�B
        bool canMove = Move(horizontal, vertical,out hit);
        
        if(hit.transform==null) //������return���邱�ƂŃL�����N�^�[����L�[�ŘA�����ē����Ȃ����Ă�B
        {
            GameManager.instance.playerTurn = false;
            return;
        }

        Damage hitComponent = hit.transform.GetComponent<Damage>();

        if(!canMove && hitComponent!=null) //�ړ��悪�ǂ������ɂԂ���Ƃ�
        {
            OnCantMove(hitComponent);
        }

        CheckFood();
        GameManager.instance.playerTurn = false;
    }

    public void OnCantMove(Damage hit)
    {
        ChangeDirection();
        hit.AttackDamage(GameManager.instance.ATK); //�ړ���ɕǂ�����Ƃ��͕ǂɍU������

        animator.SetTrigger("Chop");
        AudioSource.PlayClipAtPoint(attackClip, camera.transform.position);
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void CheckFood()
    {
        if(GameManager.instance.food<=0�@|| GameManager.instance.HP<=0)
        {
            GameManager.instance.GameOver(); 
        }
    }

    /*public void OnDisable() //�I�u�W�F�N�g����A�N�e�B�u�̂Ƃ��ɌĂ΂��B
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
