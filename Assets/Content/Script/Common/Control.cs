using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField]
    Canvas parentCanvas;
    [SerializeField]
    GameObject clickFX;
    [SerializeField]
    ParticleSystem clickParticle;
    Vector2 mousePos;
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
    }
}
