using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private GameObject DamageObj;
    [SerializeField]
    private GameObject PosObj;
    [SerializeField]
    private Vector3 AdjPos;

    public void ViewDamage(int _damage)
    {
        GameObject _damageObj = Instantiate(DamageObj);
        _damageObj.GetComponent<TextMesh>().text = _damage.ToString();
        _damageObj.transform.position = PosObj.transform.position + AdjPos+new Vector3(0,2,0);
        StartCoroutine(MoveObject(_damageObj));
    }

    IEnumerator MoveObject(GameObject obj)
    {
        obj.transform.DOMove(obj.transform.position+new Vector3(0, 0.15f, 0),0.2f);
        yield return new WaitForSeconds(0.3f);
        obj.transform.DOMove(obj.transform.position+new Vector3(0, -0.15f, 0),0.2f);
    }

}

