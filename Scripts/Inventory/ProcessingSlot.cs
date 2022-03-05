using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProcessingSlot : MonoBehaviour
{
    //private Text informationText;
    private GameObject wakuUIInstance;
    private Item myItemData;
    private GameObject useMessagePanel;
    private GameManager gameManager;
    private GameObject canvas;
    private GameObject useButton;
    private Text itemName;
    private Text effect;
    private Item item;
    private ItemManager itemManager;


    void OnDisable()
    {
        Destroy(gameObject);
    }

    public void SetItemData(Item item)
    {
        myItemData = item;
        transform.GetChild(0).GetComponent<Image>().sprite = myItemData.GetItemIcon();
    }

    /*public void SetItemData(ItemData itemData)
    {
        myItemData = itemData;
        transform.GetChild(0).GetComponent<Image>().sprite = myItemData.GetItemSprite();
    }*/

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        useMessagePanel = canvas.transform.Find("UseMessagePanel").gameObject;
        //informationText = transform.parent.parent.Find("Information").GetChild(0).GetComponent<Text>();
    }

    private void GetClickObject() //Item欄からItemSlotを選択したときにUse Cancelボタンを表示する処理。
    {
        //itemData.GetItemName();
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        
        foreach (RaycastResult raycastResult in result)
        {
            if (raycastResult.gameObject.name == "ItemSlot")
            {
                var itemSlot = raycastResult.gameObject;
                var imageName = itemSlot.transform.Find("Image").GetComponent<Image>().sprite.name;
                item = itemManager.GetItem(imageName);
                

                useMessagePanel.SetActive(true);
                itemName = GameObject.Find("ItemName").GetComponent<Text>();
                effect = GameObject.Find("Effect").GetComponent<Text>();

                itemName.text = item.GetItemName();
                effect.text = item.GetInformation();
                var animator = useMessagePanel.GetComponent<Animator>();
                animator.SetBool("Open", true);

                useMessagePanel.GetComponent<RectTransform>().SetAsLastSibling();

                
                return;

            }

        }

    }

    public void MouseOver()
    {
        GetClickObject();
        //informationText.text = myItemData.GetInformation();


        /*if (Input.GetMouseButtonDown(0))
        {
            if (itemSlotTitleUIInstance != null)
            {
                Destroy(itemSlotTitleUIInstance);
            }

            itemSlotTitleUIInstance = Instantiate<GameObject>(itemSlotTitleUI, new Vector3(transform.position.x - 25f, transform.position.y + 25f, 0f), Quaternion.identity, transform.parent.parent);
            var itemSlotTitleText = itemSlotTitleUIInstance.GetComponentInChildren<Text>();
            itemSlotTitleText.text = myItemData.GetItemName();
            informationText.text = myItemData.GetItemInformation();
        }*/
    }

    public void MouseExit()
    {
        //informationText.text = "";

    }


}
