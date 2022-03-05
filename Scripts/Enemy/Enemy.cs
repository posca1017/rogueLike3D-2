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
        GameManager.instance.AddEnemy(this); //���̃X�N���v�g���̂�n���B
    }

    public void ATMove(int horizontal, int vertical) //Enemy�̍s�����ړ����U�������߂�֐��@���ړ��\���ɃI�u�W�F�N�g���Ȃ���Γ����B�@����΍U���ɂ���B
    {
        if (!isDamage)
        {
            RaycastHit hit; //�����Ă邩���ׂ�B
            bool canMove = Move(horizontal, vertical, out hit); //������Raycast��hit�ϐ��ňړ���ɃI�u�W�F�N�g���Ȃ����`�F�b�N���遨����Έړ��s�Ƃ���Enemy�𓮂����Ȃ�


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

            if (hit.transform == null) //�ړ���ɃI�u�W�F�N�g���Ȃ����return����B�@�� �I�u�W�F�N�g���Ȃ��̂ōU�����s�����Ƃ��Ȃ����߁B
            {
                return;
            }

            Player hitComponent = hit.transform.GetComponent<Player>(); //�����̏��������܂������ĂȂ��B�@�Ȃ��GetComponent�ł��Ȃ��H hit�ɃI�u�W�F�N�g�������Ă�̂͊ԈႢ�Ȃ��B(�I�u�W�F�N�g���ǂȂ̂��A�C�e���Ȃ̂�Player�Ȃ̂��̎��ʂ������̑���ōs���Ă���)

            if (!canMove && hitComponent != null) //�ړ��悪�v���C���[�������Ƃ�(hitComponent��Player�^�̂��߁APlayer�Ɍ��肳���)
            {
                OnCantMove(hitComponent);
            }
        }


    }

    public bool Move(int horizontal, int vertical, out RaycastHit hit)
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(horizontal, 0, vertical); //end���ړ��\��̈ʒu�B

        boxCollider.enabled = false; //ray���O�ɔ�΂��Ȃ��Ȃ�̂ňꎞ�I��false�ɂ���B

        if (Physics.Linecast(start, end, out hit, blockingLayer)) //�ړ��\��̍��W�Ɍ������΂��BblockingLayer�̃I�u�W�F�N�g���Ȃ����m�F�B�@��blockingLayer�̃I�u�W�F�N�g���Ȃ�==�ړ��\�ȍ��W�B
        {

        }

        //hit = Physics.Linecast(start, end,out hit,blockingLayer); //start�Eend�̊ԂɌ����΂��A���������I�u�W�F�N�g�̃��C���[���擾����hit�ɑ���ɂ���B
        boxCollider.enabled = true;

        if (!isMoving && hit.transform == null) //�����Ă��Ȃ��@���@BlockingLayer�̉����ƐڐG���Ă��Ȃ��Ƃ��@���ړ���ɃI�u�W�F�N�g(�ǁE�A�C�e���EPlayer)������񂾂��瓮�����Ȃ��B�@���I�u�W�F�N�g������Ȃ��ATMove�ɖ߂��čU���������s���B
        {
            StartCoroutine(Movement(end)); //Enemy���ړ�������B
            return true;
        }

        return false;
    }

    public void OnCantMove(Player hit)
    {
        hit.EnemyAttack(attackDamage); //�ړ���ɕǂ�����Ƃ��͕ǂɍU������
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
        float remainingDistance = (transform.position - end).sqrMagnitude; //���ݒn�ƈړ��\��̈ʒu�����������B

        while (remainingDistance > float.Epsilon) //epsilon==����Ȃ�0�ɋ߂�����
        {
            transform.position = Vector3.MoveTowards(transform.position, end, 1f / moveTime* Time.deltaTime); //���ݒn����end�܂ŏ��X�Ɉړ�������

            remainingDistance = (transform.position - end).sqrMagnitude; //�ړ���̍��W���Z�o��while�̏����ɓ��Ă͂܂邩�ēx�`�F�b�N����

            yield return null;
        }

        transform.position = end; //�ړI�n�ɓ���������B
        isMoving = false;

    }

    public void Death()
    {
        GameManager.instance.DestroyEnemy(this); //���X�g���炱��Enemy�̃X�N���v�g���폜����B
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

    public void MoveEnemy() //Enemy���ړ�������W�����߂�X�N���v�g�@(�ړ����邩�U�����邩�͂܂����܂��ĂȂ�)
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


    /*public void MoveEnemy() //Enemy���ړ�������W�����߂�X�N���v�g�@(�ړ����邩�U�����邩�͂܂����܂��ĂȂ�)
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
