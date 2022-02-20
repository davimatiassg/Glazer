using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlazzMoveset : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private int life = 50;
    [SerializeField] private int direction = 0;
    //0 = Down, 1 = Up, 2 = Right, 3 = Left.
    public int state = 0; 
    //0 = Idle, 1 = Moving, 2 = Melee atk, 3 = Magic atk, 4 = Dashing, 5 = stuned, 6 = cutscene.


    //movimentation Variables

    public Vector3 deltaMove;


    private Transform trs;
    [SerializeField] private Transform glz;
    private Animator anim;
    private BoxCollider2D coll;
    private Rigidbody2D rigb;

    void Start()
    {	
    	trs = this.gameObject.GetComponent<Transform>();
        anim = glz.gameObject.GetComponent<Animator>();
        coll = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        deltaMove = InputMoviment();
        deltaMove = ColisionDetect();
        trs.Translate(deltaMove * Time.fixedDeltaTime);
        PlayAnim(direction, state);
        FloatingEffect();
    }


    public Vector2 InputMoviment()
    {
    	float y = Input.GetAxisRaw("Vertical");
    	float x = Input.GetAxisRaw("Horizontal");
    	if(y*y + x*x != 0)
    	{
    		direction = Mathf.RoundToInt((1+y)/2  - Mathf.Abs(x)*(1+y)/2  + Mathf.Abs(x)*(5 - x)/2);
    	}
    	
/*
    	if(y != 0f && x == 0f)
		{
			if(y > 0)
			{
				direction = 1;		
			}
			else
			{
				direction = 0;
                
			}
		}
    	else if(x != 0f && y == 0f)
    	{
			if(x > 0)
			{
				direction = 2;               		
			}
			else
			{
				direction = 3;                
		    }
    	}
*/
        Vector2 v = new Vector2(x, y).normalized*speed;
        if(v != Vector2.zero)
        {
            state = 1;
        }
        else
        {
            state = 0;
        }
    	return v;
    }

    private Vector2 ColisionDetect()
    {
    	Vector2 V = Vector2.zero;
    	RaycastHit2D hit = Physics2D.BoxCast(trs.localPosition, coll.size, 0, deltaMove.y*Vector2.up, Mathf.Abs(deltaMove.y*Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Water"));
    	if(hit.collider == null)
    	{
    		V += Vector2.up*deltaMove;
    	}
    	hit = Physics2D.BoxCast(trs.localPosition, coll.size, 0, deltaMove.x*Vector2.right, Mathf.Abs(deltaMove.x*Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Water"));
    	if(hit.collider == null)
    	{
    		V += Vector2.right*deltaMove;
    	}
    	Debug.Log(V);
    	return V;
    }

    public void PlayAnim(int dir, int st)
    {
    	anim.SetFloat("Dir", dir);
        anim.SetFloat("State", st);
    }
    private void FloatingEffect()
    {
    	glz.localPosition = (Mathf.Sin(Time.time*2)/5)*Vector3.up;
    
    }

}
