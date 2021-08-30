using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterImageController : MonoBehaviour
{
    public CenterAnimationWidget[] widgets;
    bool needStop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        needStop = false;
        for(int i = 0; i < widgets.Length;++i)
        {
            widgets[i].SetActive(false);
        }
        StartCoroutine(Play());
    }
    private void OnDisable()
    {
        needStop = true;
        StopCoroutine(Play());
    }
    IEnumerator Play()
    {
        for (int i = 0; true; ++i)
        {
            widgets[i % widgets.Length].PlayInAnim();
            yield return new WaitForSeconds(2);
            widgets[i % widgets.Length].PlayOutAnim();
        }
    }
}
