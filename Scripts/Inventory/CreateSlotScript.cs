using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSlotScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slot;
    private Player player;

    private ItemDataBase itemDataBase;

    void OnEnable()
    {
        itemDataBase = GameObject.Find("ItemManager").GetComponent<ItemManager>().GetItemDataBase();
        //player = GameObject.FindWithTag("Player").GetComponent<Player>();
        CreateSlot(itemDataBase.GetItemLists()); //ItemDataBaseにあるアイテム情報を取得→持ってるアイテムのスロットを作成。
    }

    public void CreateSlot(List<Item> itemList)
    {
        if(player==null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        int i = 0;

        foreach(var item in itemList)
        {
            if(player.GetItemFlag(item.GetItemName()))
            {
                var instanceSlot = Instantiate<GameObject>(slot, transform);
                instanceSlot.name = "ItemSlot";
                instanceSlot.transform.localScale = new Vector3(1f, 1f, 1f);
                instanceSlot.GetComponent<ProcessingSlot>().SetItemData(item);
            }
        }
    }

    /*public void CreateSlot(List<ItemData> itemList)
    {
        int i = 0;

        foreach(var item in itemList)
        {
            if(player.GetItemFlag(item.GetItemName()))
            {
                var instanceSlot = Instantiate<GameObject>(slot, transform);
                instanceSlot.name = "ItemSlot";
                instanceSlot.transform.localScale = new Vector3(1f, 1f, 1f);
                instanceSlot.GetComponent<ProcessingSlot>().SetItemData(item);
                
            }
        }
    }*/
}
