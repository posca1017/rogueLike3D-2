using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RecoveryText : MonoBehaviour
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
    }

}

