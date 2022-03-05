using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public int wallHp = 3;
    public int enemyHp = 5;
    private Enemy enemy;
    public GameObject floor;
    private DamageText damageText;

    //public Sprite dmgWall; //壁がダメージ受けた時の演出
    //private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //spriteRenderer=GetComponent<SPriteRenderer>();
        enemy = GetComponent<Enemy>();
        damageText = GetComponent<DamageText>();
    }


    public void AttackDamage(int loss)
    {
        if(gameObject.CompareTag("Wall"))
        {
            //spriteRenderer.sprite = dmgWall;
            wallHp -= loss;

            if (wallHp <= 0)
            {
                gameObject.SetActive(false);
                Instantiate(floor, transform.position,Quaternion.identity);
            }
        }

        else if(gameObject.CompareTag("Enemy"))
        {
            enemyHp -= loss;
            damageText.ViewDamage(loss);
            if (enemyHp <= 0)
            {
                enemy.Death();
            }

        }

    }


}
