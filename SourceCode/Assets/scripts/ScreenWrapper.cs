using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour {
    float colliderRadius;
    bool xIsWrapped;
    bool yIsWrapped;

    Renderer[] renderers;

    // Use this for initialization
	void Start () {
        //get the radius of the circle collider of the object
        colliderRadius = GetComponent<CircleCollider2D>().radius;
        //get all the sprite renderers of the object 
        renderers = GetComponentsInChildren<Renderer>();
	}

    private void FixedUpdate()
    {
        ScreenWrap();
    }

    void ScreenWrap(){
        bool isVisible = CheckRenderers();
        if (isVisible) {
            xIsWrapped = false;
            yIsWrapped = false;
            return;
        }
        // prevent the object from being wrapped for multiple times
        // finish the excecution of ScreenWrap function if already wrapped
        if (xIsWrapped && yIsWrapped){
            return;
        }
        Vector2 position = transform.position;
        // check left, right, top, and bottom sides
        if (position.x + colliderRadius < ScreenUtils.ScreenLeft ||
            position.x - colliderRadius > ScreenUtils.ScreenRight)
        {
            position.x *= -1;
            xIsWrapped = true;
        }
        if (position.y - colliderRadius > ScreenUtils.ScreenTop ||
            position.y + colliderRadius < ScreenUtils.ScreenBottom)
        {
            position.y *= -1;
            yIsWrapped = true;
        }

        // move ship
        transform.position = position;
    }

    bool CheckRenderers(){
        foreach(Renderer r in renderers){
            if(r.isVisible == true){
                return true;
            }
        }
        return false;
    }
   


}
