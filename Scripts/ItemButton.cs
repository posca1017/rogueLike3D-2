using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private GameObject propertyUI;
    private Text closeText;
    private bool isOpen = false;
    private Animator animator;
    private Transform camera;
    [SerializeField]
    private AudioClip openItem;
    [SerializeField]
    private AudioClip closeItem;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        closeText = transform.GetChild(0).GetComponent<Text>();
        propertyUI = GameObject.Find("PropertyUI");
        //propertyUI.SetActive(false);　→別のスクリプトで実行する。
    }

    public void OpenInventory()
    {
        if (!isOpen)
        {
            isOpen = true;
            propertyUI.SetActive(true);
            animator = GameObject.Find("PropertyUI").GetComponent<Animator>();
            animator.SetBool("Open", true);
            AudioSource.PlayClipAtPoint(openItem, camera.transform.position);
        }

    }

    public void CloseInventory()
    {
        if (animator == null)
        {
            animator = GameObject.Find("PropertyUI").GetComponent<Animator>();
        }

        if (isOpen)
        {
            isOpen = false;
            animator.SetBool("Open", false);
            AudioSource.PlayClipAtPoint(closeItem, camera.transform.position);
        }

    }

    public bool GetIsOpen()
    {
        return isOpen;
    }

    public void SetIsOpen(bool setIsOpen)
    {
        isOpen = setIsOpen;
    }

}
