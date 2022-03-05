using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveTime = 0.1f;
    public bool isMoving = false;

    public LayerMask blockingLayer;
    private BoxCollider boxCollider;

    private Animator animator;

    public int attackDamage = 5;

    private Transform target;
    private bool skipMove = false;

    public bool isDamage = false;

    public Direction direction;
    public int Exp = 5;
    private Transform camera;

    [SerializeField]
    private GameObject attackEffect;
    [SerializeField]
    private AudioClip NormalAttackClip;
    [SerializeField]
    private AudioClip DeadClip;



    public enum Direction
    {
        North,
        South,
        East,
        West,
    }


    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        target = GameObject.FindWithTag("Player").transform;
        GameManager.instance.AddEnemy(this); //このスクリプト自体を渡す。
    }

    public void ATMove(int horizontal, int vertical) //Enemyの行動を移動か攻撃か決める関数　→移動予定先にオブジェクトがなければ動く。　あれば攻撃にする。
    {
        if (!isDamage)
        {
            RaycastHit hit; //あってるか調べる。
            bool canMove = Move(horizontal, vertical, out hit); //ここでRaycastのhit変数で移動先にオブジェクトがないかチェックする→あれば移動不可としてEnemyを動かさない


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

            if (hit.transform == null) //移動先にオブジェクトがなければreturnする。　→ オブジェクトがないので攻撃を行うことがないため。
            {
                return;
            }

            Player hitComponent = hit.transform.GetComponent<Player>(); //ここの処理がうまくいってない。　なんでGetComponentできない？ hitにオブジェクトが入ってるのは間違いない。(オブジェクトが壁なのかアイテムなのかPlayerなのかの識別もここの代入で行っている)

            if (!canMove && hitComponent != null) //移動先がプレイヤーだったとき(hitComponentがPlayer型のため、Playerに限定される)
            {
                OnCantMove(hitComponent);
            }
        }


    }

    public bool Move(int horizontal, int vertical, out RaycastHit hit)
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(horizontal, 0, vertical); //endが移動予定の位置。

        boxCollider.enabled = false; //rayを外に飛ばせなくなるので一時的にfalseにする。

        if (Physics.Linecast(start, end, out hit, blockingLayer)) //移動予定の座標に光線を飛ばす。blockingLayerのオブジェクトがないか確認。　→blockingLayerのオブジェクトがない==移動可能な座標。
        {

        }

        //hit = Physics.Linecast(start, end,out hit,blockingLayer); //start・endの間に光を飛ばし、当たったオブジェクトのレイヤーを取得してhitに代入にする。
        boxCollider.enabled = true;

        if (!isMoving && hit.transform == null) //動いていない　かつ　BlockingLayerの何かと接触していないとき　→移動先にオブジェクト(壁・アイテム・Player)があるんだから動かせない。　→オブジェクトがあるならばATMoveに戻って攻撃処理を行う。
        {
            StartCoroutine(Movement(end)); //Enemyを移動させる。
            return true;
        }

        return false;
    }

    public void OnCantMove(Player hit)
    {
        hit.EnemyAttack(attackDamage); //移動先に壁があるときは壁に攻撃する
        animator.SetTrigger("Attack");
        var effect = Instantiate(attackEffect,transform.position, Quaternion.identity);
        StartCoroutine(DestroyEffect(effect));
        AudioSource.PlayClipAtPoint(NormalAttackClip, camera.transform.position);
    }

    private IEnumerator DestroyEffect(GameObject obj)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(obj);
    }

    IEnumerator Movement(Vector3 end)
    {
        isMoving = true;
        animator.SetTrigger("Move");
        float remainingDistance = (transform.position - end).sqrMagnitude; //現在地と移動予定の位置を差し引く。

        while (remainingDistance > float.Epsilon) //epsilon==限りなく0に近い数字
        {
            transform.position = Vector3.MoveTowards(transform.position, end, 1f / moveTime* Time.deltaTime); //現在地からendまで徐々に移動させる

            remainingDistance = (transform.position - end).sqrMagnitude; //移動後の座標を算出→whileの条件に当てはまるか再度チェックする

            yield return null;
        }

        transform.position = end; //目的地に到着させる。
        isMoving = false;

    }

    public void Death()
    {
        GameManager.instance.DestroyEnemy(this); //リストからこのEnemyのスクリプトを削除する。
        animator.SetBool("Dead", true);
        Invoke("DeadCharacter", 2f);
        boxCollider.enabled = false;
        GameManager.instance.AddExp(Exp);
        AudioSource.PlayClipAtPoint(DeadClip, camera.transform.position);

    }

    private void DeadCharacter()
    {
        gameObject.SetActive(false);
    }

    public void MoveEnemy() //Enemyが移動する座標を決めるスクリプト　(移動するか攻撃するかはまだ決まってない)
    {
        if (!isDamage)
        {
            int xdir = 0;
            int zdir = 0;

            float dx = target.position.x - transform.position.x;
            float dz = target.position.z - transform.position.z;

            if (Mathf.Abs(dx) > Mathf.Abs(dz))
            {
                if (dx < 0)
                {
                    xdir = -1;
                    direction = Direction.West;
                }
                else
                {
                    xdir = 1;
                    direction = Direction.East;
                }

            }



            else
            {
                if (dz < 0)
                {
                    zdir = -1;
                    direction = Direction.South;
                }

                else
                {
                    zdir = 1;
                    direction = Direction.North;
                }

            }

            ATMove(xdir, zdir);
        }
    }


    /*public void MoveEnemy() //Enemyが移動する座標を決めるスクリプト　(移動するか攻撃するかはまだ決まってない)
    {
        if (!isDamage)
        {
            int xdir = 0;
            int zdir = 0;

            float dx = target.position.x - transform.position.x;
            float dz = target.position.z - transform.position.z;

            if (Mathf.Abs(dx) > Mathf.Abs(dz))
            {
                if (dx < 0)
                {
                    xdir = -1;
                    direction = Direction.West;
                }
                else
                {
                    xdir = 1;
                    direction = Direction.East;
                }

            }



            else
            {
                if (dz < 0)
                {
                    zdir = -1;
                    direction = Direction.South;
                }

                else
                {
                    zdir = 1;
                    direction = Direction.North;
                }

            }

            ATMove(xdir, zdir);
        }
    }*/


}
