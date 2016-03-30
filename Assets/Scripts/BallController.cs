using UnityEngine;
using System.Collections;
using System;

public class BallController : MonoBehaviour {

    private Camera cam;
    private RuntimePlatform platform = Application.platform;
    private Animator anim;
    private Animation animation;
    private GameObject player;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Boolean ballClicked = false;
    private float xValue;

    // float startTime;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        animation = GetComponent<Animation>();
	    if (cam == null)
        {
            cam = Camera.main;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && gameObjectClicked())
        {
            ballClicked = true;
            Vector3 rawData = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPosition = new Vector2(rawData.x, rawData.y);
            Debug.Log("Mouse down" + startPosition);
            //startTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0) && ballClicked == true)
        {
            Vector3 rawData = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPosition = new Vector2(rawData.x, rawData.y);
            Vector2 delta = endPosition - startPosition;

            Debug.Log("Mouse up" + endPosition);

            float dist = Mathf.Sqrt(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2));
            float angle = Mathf.Atan(delta.y/delta.x) * (180.0f/(float)Math.PI);

            // float duration = Time.time - startTime;
            
            if (startPosition.y < endPosition.y)
            {
                if (angle < 0) angle = angle * -1;
                Debug.Log("Distance: " + dist + " Angle: " + angle);

                if (dist > 1 && angle > 15)
                {
                    // Do ball throw animation
                    xValue = getX(angle);
                    throwBall(xValue);
                    Debug.Log("Ball thrown");
                }
            }
        }
    }

    private float getX(float angle)
    {
        float y = 3;
        float x;
        float missingAngle = 180 - 90 - angle;

        x = (y * (float)Math.Sin(angle)) / (float)Math.Sin(missingAngle);

        return x;
    }

    private bool gameObjectClicked()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit)
        {
            Debug.Log("Ball clicked");
            return true;
        }
        else
        {
            Debug.Log("Ball not clicked");
            return false;
        }
    }

    private void throwBall(float x)
    {
        AnimationCurve curveX = AnimationCurve.Linear(0, 0, 1, x);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);

        animation.AddClip(clip, "transformPos");
        animation.Play("transformPos");

        ballClicked = false;
        anim.SetBool("thrown", true);
        Debug.Log("Throw ball animation");
        //anim.SetBool("ballThrown", false);
    }
}
