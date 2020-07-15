using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private bool isCutting = false;
    Rigidbody2D rb;
    Camera cam;
    public GameObject bladeTrailPrefab;
    CircleCollider2D col;

    Vector2 previousPosition;

    public float minCuttingVelocity = .001f;

    GameObject currentBladeTrail;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }else if (Input.GetMouseButtonUp(0)) {
            StopCutting();
        }

        if (isCutting)
        {
            UpdateCut();
        }
    }

     void UpdateCut()
    {

        Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPosition;


        //blade belirli bir hizin ustundeyse cut islemi gerceklesmeli aksi halde tiklayarak dahi cut yapilabilir halde oluyor
        //rb kinematic oldugu icin ordan velocityi check edemeyiz cunku kinematic rb lerde velocity sifirdir
        //bu nedenle klasik anlamda velocityi hesaplariz
        float velocity = (newPosition - previousPosition).magnitude * Time.deltaTime;
        if (velocity > minCuttingVelocity )
        {
            col.enabled = true;
        }else
        {
            col.enabled = false;
        }

        previousPosition = newPosition;
    }

    void StopCutting()
    {
        isCutting = false;
        currentBladeTrail.transform.SetParent(null);
        Destroy(currentBladeTrail, 2f); //2sn sonra destroy et
        col.enabled = false;
    }

     void StartCutting()
    {
        isCutting = true;
        //hiyerarside weird efekt oluyor tiklanan yerden baslamiyor bazen
        //transform yazarak instantiate oldugunda bu objenin child i olmasini sagladik
         currentBladeTrail = Instantiate(bladeTrailPrefab, transform);
        previousPosition = cam.ScreenToWorldPoint(Input.mousePosition);

        col.enabled = false; //ilk framede enabled olmasin velocitye gore enabled olsun
    }
}
