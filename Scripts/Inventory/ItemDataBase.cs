using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<Item> itemLists = new List<Item>();

    public List<Item> GetItemLists() //この中に個別のアイテムの情報が入ってる　�@アイテムの種類(weapon or useitem)�Aアイテムのアイコン(Sprite)�Bアイテムの名前�Cアイテムの情報(information)
    {
        return itemLists; //あくまでデータベース。最初からゲームで使うアイテムを全部入れておく箱。　拾ったアイテムとの照合に使う。
    }



}