using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BallController : MonoBehaviour {

    private Camera cam;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Boolean ballClicked = false;
    private Boolean ballFalling = false;
    private Rigidbody2D rb;
    private Animator anim;
    private Renderer rend;
    public GameObject floor;

    private int score;
    public Text scoreText;

    // Use this for initialization
    void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        Physics2D.gravity = new Vector2(0, -20f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = gameObject.GetComponent<Renderer>();
        floor = GameObject.Find("bck");

        score = 0;
        setScoreText();

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
        }
        if (Input.GetMouseButtonUp(0) && ballClicked == true)
        {
            Vector3 rawData = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPosition = new Vector2(rawData.x, rawData.y);
            Vector2 delta = endPosition - startPosition;

            float dist = Mathf.Sqrt(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2));
            float angle = Mathf.Atan(delta.y / delta.x) * (180.0f / (float)Math.PI);

            if (startPosition.y < endPosition.y)
            {
                if (angle < 0) angle = angle * -1;
                Debug.Log("Distance: " + dist + " Angle: " + angle);

                if (dist >= 1 && angle > 0)
                {
                    // Do ball throw animation
                    //xValue = getX(angle);
                    anim.SetBool("thrown", true);
                    throwBall(delta);
                }
                else
                {
                    ballClicked = false;
                }
            }
            else
            {
                ballClicked = false;
            }
        }

        float yPos = transform.position.y;

        if (yPos >= 3)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            ballFalling = true;
            rend.sortingOrder = 3;
        }
        if (ballFalling && yPos <= 0)
        {
            Debug.Log("ish ish");
            score = 0;
            setScoreText();
            ballFalling = false;
            floor.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (gameObject.transform.position.x < -10 || gameObject.transform.position.x > 10 || gameObject.transform.position.y < -7)
        {
            rb.isKinematic = true;
            respawn();
        }
    }

    void setScoreText()
    {
        if (score > 0)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = "";
        }
    }

    void respawn()
    {
        anim.SetBool("thrown", false);
        rend.sortingOrder = 4;
        floor.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.transform.localScale = new Vector3(1,1,1);
        gameObject.transform.position = new Vector2(UnityEngine.Random.Range(-7f, 7f), -3.46f);
        rb.isKinematic = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            Debug.Log("Gool");
            score++;
            setScoreText();
            ballFalling = false;
            floor.GetComponent<BoxCollider2D>().enabled = false;
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

    private void throwBall(Vector2 delta)
    {
        Debug.Log(delta.x);
        Debug.Log(delta.y);
        /*AnimationCurve curveX = AnimationCurve.Linear(0f, 0f, 1f, x);
        Keyframe[] ky = new Keyframe[3];
        ky[0] = new Keyframe(0f, -4f);
        ky[1] = new Keyframe(0.8f, 3f);
        ky[2] = new Keyframe(1f, 2.8f);
        AnimationCurve curveY = AnimationCurve.Linear(0f, -4f,1f, 3f);
        //AnimationCurve scale = AnimationCurve.Linear(0, 2, 0, 1.2f);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
        //clip.SetCurve("", typeof(Transform), "localScale.x", scale);
        //clip.SetCurve("", typeof(Transform), "localScale.y", scale);

        animation.AddClip(clip, "throwBall");
        animation.Play("throwBall");*/
       
        Debug.Log("Throw ball animation");

        Vector2 throwForce = new Vector2(delta.x * 10, 35f);
        rb.AddForce(throwForce, ForceMode2D.Impulse);

        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
