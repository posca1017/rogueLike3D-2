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
    private Dictionary<Item, int> numOfItem = new Dictionary<Item, int>(); //�A�C�e�����Ǘ��@�X�^�[�g�ŃA�C�e���̃��X�g��ǉ��B�ۗL����0�ɂ��Ă�B
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


    

    public Item GetItem(string searchName) //�����ŃA�C�e������n�����A�C�e���f�[�^�x�[�X(�S�A�C�e���̈ꗗ)�Əƍ����ĊY���̃A�C�e�����Ăяo�����ɕԂ��B
    {
        return itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName() == searchName);

    }

    public void AddItem(string searchName) //�A�C�e�����擾�����Ƃ��Ɏ�������ǉ�����X�N���v�g��ItemDataBase�Əƍ����遨�Y���̃A�C�e���̐���1���₷�Ƃ�������
    {
        if(player==null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        /*foreach(Item item in numOfItem.Keys)
        {
            if(searchName==item.GetItemName())
            {
                Debug.Log("�擾�ł��邩�Ă���");
            }
        }*/
        
        if(GetItem(searchName))
        {
            player.itemFlags[searchName] = true;

            Debug.Log("test");
        }

        else
        {
            Debug.Log("�擾����[");
        }

        //numOfItem.Add(GetItem(searchName), numOfItem[GetItem(searchName)]);
        //numOfItem.Add(GetItem(searchName), numOfItem[GetItem(searchName)]);

        /*foreach (Item item in numOfItem.Keys)
        {
            if (searchName != item.GetItemName()) //numOfKey�̒��g(item)��searchName����v����=numOfItem�̒��Ɋ���key���o�^����Ă���
            {
                //�Ȃ������̏���������ĂȂ��H
                Debug.Log("�A�C�e���o�^�ς̂��߁A���������₷");
                numOfItem.Add(GetItem(searchName), 0); //�E���Ă��ăA�C�e�������A�C�e���f�[�^���X�g���ɂ��邩�ƍ�����B������ΕۗL�A�C�e���Ƃ��ĊǗ��Bint�͕ۗL���B //int�^�����܂������Ȃ������܂���
            }

            else if (searchName == item.GetItemName())
            {
                Debug.Log("�A�C�e�����o�^�̂��߁ADictionary�ɓo�^+����1�ɂ���");
            }

        }*/


    }


        /*foreach (Item item in numOfItem.Keys)
        {
            Debug.Log("22222");
            if (!numOfItem.ContainsKey(item)) //Dictionary�ɓ����A�C�e�������o�^����Ȃ��悤�Ɍ�������B
            {
                numOfItem.Add(GetItem(searchName), 0); //�E���Ă��ăA�C�e�������A�C�e���f�[�^���X�g���ɂ��邩�ƍ�����B������ΕۗL�A�C�e���Ƃ��ĊǗ��Bint�͕ۗL���B //int�^�����܂������Ȃ������܂���
                Debug.Log("�����A�C�e�����Ȃ�");
                if (numOfItem.ContainsValue(0))
                {
                    Debug.Log("Value�Ɏg���Ă��܂�");
                }
                else if (!numOfItem.ContainsValue(1))
                {
                    Debug.Log("Value�Ɏg���Ă��܂���");
                }

            }

            else if (numOfItem.ContainsKey(item)) //�����A�C�e�����������Ƃ��͊����̃A�C�e���̐��𑝂₷�B�@���A�C�e���̕\���𑝂₷�����͕ʓr�s���B
            {
                Debug.Log("�����A�C�e��������");
            }


        }*/
}
