using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    private GameObject propertyUI;
    private Player player;
    private int portionPoint = 20;
    private int foodPoint = 20;
    private Text itemName;
    private EffectScript effect;

    private ItemManager itemManager;
    private ItemButton itemButton;
    [SerializeField]
    private AudioClip cancelButtonClip;
    private GameObject camera;
    private GameObject itemMessage;


    void Start()
    {

        propertyUI = GameObject.Find("PropertyUI");
        effect = GameObject.Find("effects").GetComponent<EffectScript>();
        itemName = GameObject.Find("ItemName").GetComponent<Text>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        gameObject.SetActive(false);
        propertyUI.SetActive(false);
        itemButton = GameObject.Find("ItemButton").GetComponent<ItemButton>();
        camera = GameObject.FindWithTag("MainCamera");

    }

    public void UseButton()
    {
        string name = "Portion";
        //なんのアイテムを使うか？
        GameManager.instance.HP += portionPoint;
        effect.MakeEffects("Recovery");
        if(GameManager.instance.HP>GameManager.instance.maxHP)
        {
            GameManager.instance.HP = GameManager.instance.maxHP;
        }
        player.IsDrinking();
        StartCoroutine(player.ItemMessageWindow(name));
        //アイテムを使用したメッセージの表示　→体力が〇〇回復したなど
        player.GetComponent<RecoveryText>().ViewDamage(20);
        //ステータスの回復など
        CloseUI();
        //使用したアイテムオブジェクトをアイテム画面とアイテムリストから削除

    }
    public void CancelButton()
    {
        CloseUI();
        //オブジェクトの選択を解除する。
    }

    private void CloseUI()
    {
        propertyUI.GetComponent<Animator>().SetBool("Open", false);
        gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(cancelButtonClip, camera.transform.position);

        if (itemButton.GetIsOpen())
        {
            itemButton.SetIsOpen(false);
        }               

    }

}
