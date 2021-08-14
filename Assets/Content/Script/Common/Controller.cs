using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller : MonoBehaviour
{
    [SerializeField]
    Canvas parentCanvas;
    [SerializeField]
    GameObject clickFX;
    [SerializeField]
    ParticleSystem clickParticle;
    Vector2 mousePos;
    public static UnityEvent androidBackBtnEvent = new UnityEvent();
    private void Start()
    {
        clickFX.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            clickFX.transform.position = new Vector3(movePos.x, movePos.y, 10);
            clickParticle.Play();
        }

#if UNITY_ANDROID 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Ins.GetCurrentPanel().OnBackEvent();
        } 
#endif
    }
}
