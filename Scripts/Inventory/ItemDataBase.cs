using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<Item> itemLists = new List<Item>();

    public List<Item> GetItemLists() //���̒��Ɍʂ̃A�C�e���̏�񂪓����Ă�@�@�A�C�e���̎��(weapon or useitem)�A�A�C�e���̃A�C�R��(Sprite)�B�A�C�e���̖��O�C�A�C�e���̏��(information)
    {
        return itemLists; //�����܂Ńf�[�^�x�[�X�B�ŏ�����Q�[���Ŏg���A�C�e����S������Ă������B�@�E�����A�C�e���Ƃ̏ƍ��Ɏg���B
    }



}