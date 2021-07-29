using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarWidget : MonoBehaviour
{
    public Image starImage;
    public ParticleSystem starParticle;

    public bool visible 
    {
        set
        {
            starImage.enabled = value;
        }
    } 
    public void PlayAnimation()
    {
        StartCoroutine(PlayStarAnimation());
    }
    IEnumerator PlayStarAnimation()
    {
        starImage.enabled = false;
        starParticle.Play();
        yield return new WaitForSeconds(2);
        starImage.enabled = true;
    }
}
