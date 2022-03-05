using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> effectLists;
    [SerializeField]
    private List<AudioClip> seList;
    public bool isDebug = false;
    private GameObject camera;

    public List<GameObject> GetEffectLists()
    {
        return effectLists;
    }

    public void MakeEffects(string effectName)
    {
        camera = GameObject.FindWithTag("MainCamera");
        var player = GameObject.FindWithTag("Player");

        foreach(GameObject effect in effectLists)
        {
            if(effectName==effect.name)
            {
                var effectObject=Instantiate((GameObject)effect, player.transform.position+new Vector3(0,1,0), Quaternion.identity);
                foreach(var se in seList)
                {
                    if(se.name==effect.name) //effect‚Ì–¼‘O‚Æse‚Ì–¼‘O‚ð“¯‚¶‚É‚·‚é
                    {
                        AudioSource.PlayClipAtPoint(se,camera.transform.position);
                    }
                }
                StartCoroutine(DestroyObject(effectObject));
            }
        }

    }

    IEnumerator DestroyObject(GameObject effect)
    {
        if (isDebug == false)
        {
            yield return new WaitForSeconds(1.2f);
            Destroy(effect);
        }
    }
}
