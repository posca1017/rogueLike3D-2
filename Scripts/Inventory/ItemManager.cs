using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public enum ItemType
    {

    }

    [SerializeField]
    private ItemDataBase itemDataBase;
    private Dictionary<Item, int> numOfItem = new Dictionary<Item, int>(); //アイテム数管理　スタートでアイテムのリストを追加。保有数は0にしてる。
    private string objectLinks = "Prefabs/Items/";
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindWithTag("Player").GetComponent<Player>();
        for(int i=0;i<itemDataBase.GetItemLists().Count;i++)
        {
            numOfItem.Add(itemDataBase.GetItemLists()[i], 0);
        }
    }

    public ItemDataBase GetItemDataBase()
    {
        return itemDataBase;
    }


    

    public Item GetItem(string searchName) //引数でアイテム名を渡す→アイテムデータベース(全アイテムの一覧)と照合して該当のアイテムを呼び出し元に返す。
    {
        return itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName() == searchName);

    }

    public void AddItem(string searchName) //アイテムを取得したときに持ち物を追加するスクリプト→ItemDataBaseと照合する→該当のアイテムの数を1つ増やすという処理
    {
        if(player==null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        /*foreach(Item item in numOfItem.Keys)
        {
            if(searchName==item.GetItemName())
            {
                Debug.Log("取得できるかてすと");
            }
        }*/
        
        if(GetItem(searchName))
        {
            player.itemFlags[searchName] = true;

            Debug.Log("test");
        }

        else
        {
            Debug.Log("取得えらー");
        }

        //numOfItem.Add(GetItem(searchName), numOfItem[GetItem(searchName)]);
        //numOfItem.Add(GetItem(searchName), numOfItem[GetItem(searchName)]);

        /*foreach (Item item in numOfItem.Keys)
        {
            if (searchName != item.GetItemName()) //numOfKeyの中身(item)とsearchNameが一致する=numOfItemの中に既にkeyが登録されている
            {
                //なぜか中の処理がされてない？
                Debug.Log("アイテム登録済のため、数だけ増やす");
                numOfItem.Add(GetItem(searchName), 0); //拾ってきてアイテム名がアイテムデータリスト内にあるか照合する。→あれば保有アイテムとして管理。intは保有数。 //int型がうまくいかない←いまここ
            }

            else if (searchName == item.GetItemName())
            {
                Debug.Log("アイテム未登録のため、Dictionaryに登録+数を1にする");
            }

        }*/


    }


        /*foreach (Item item in numOfItem.Keys)
        {
            Debug.Log("22222");
            if (!numOfItem.ContainsKey(item)) //Dictionaryに同じアイテム名が登録されないように検索する。
            {
                numOfItem.Add(GetItem(searchName), 0); //拾ってきてアイテム名がアイテムデータリスト内にあるか照合する。→あれば保有アイテムとして管理。intは保有数。 //int型がうまくいかない←いまここ
                Debug.Log("同じアイテムがない");
                if (numOfItem.ContainsValue(0))
                {
                    Debug.Log("Valueに使われています");
                }
                else if (!numOfItem.ContainsValue(1))
                {
                    Debug.Log("Valueに使われていません");
                }

            }

            else if (numOfItem.ContainsKey(item)) //同じアイテムがあったときは既存のアイテムの数を増やす。　→アイテムの表示を増やす処理は別途行う。
            {
                Debug.Log("同じアイテムがある");
            }


        }*/
}
