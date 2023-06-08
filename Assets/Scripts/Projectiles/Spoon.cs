using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : Projectile
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Collider = GetComponentInChildren<Collider2D>();

    }

    public void RotateWithoutCollider(float degrees)
    {
        transform.Rotate(new Vector3(0,0,degrees));
        Collider.transform.Rotate(new Vector3(0,0,-1 * degrees));
    }

    public void setDirectionWithVector(Vector2 vector)
    {
        float angleDifferenceBetweenSpoonAndNeeded = Vector2.SignedAngle(transform.up, vector);
        RotateWithoutCollider(angleDifferenceBetweenSpoonAndNeeded);
    }
    
    float GetAngleToObject(Transform obj)
    {
        Vector2 spoonPointDirection = transform.up;
        Vector2 directionToObj = obj.transform.position - transform.position;
        float angle = Vector2.SignedAngle(spoonPointDirection, directionToObj);
        return angle;
    }


    // Update is called once per frame
    void Update()
    {
    }
}
