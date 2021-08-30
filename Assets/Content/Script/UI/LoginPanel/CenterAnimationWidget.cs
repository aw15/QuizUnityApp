using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterAnimationWidget : MonoBehaviour
{
    //public GameObject me;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponentInParent<Animator>();
    }

    public void PlayInAnim()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("In");
    }
    public void PlayOutAnim()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Out");
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
