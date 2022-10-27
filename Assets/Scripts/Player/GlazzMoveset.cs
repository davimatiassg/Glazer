using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlazzMoveset : MonoBehaviour, HitableObj
{
	//Other Object References

	private InputManager inPut;
	public List<OrbBehaviour> Orbs = new List<OrbBehaviour>();


	//Self References

	private Transform trs;
    [SerializeField] private Transform glz;
    private Animator anim;
    private BoxCollider2D coll;
    private Rigidbody2D rigb;
    private float aceleration;


    //Inside Variables
    	//movimentação e corrida
    [SerializeField] private float basespeed = 1f;
    private float speed = 1f;
    [SerializeField] private float runspeed = 2.5f;
    public Vector3 deltaMove;

    	//direção do olhar e estado(animação)
    private Vector2 LookDirection = Vector2.down;
    [SerializeField] private float direction; //0 = Down, 1 = Right, 2 = Up, 3 = Left.
    public int state = 0; //0 = Idle, 1 = Moving, 2 = Running, 3 = Melee atk, 4 = Magic atk, 5 = Dashing, 6 = stuned, 7 = cutscene.

    	//combate e vida
    
    [SerializeField] private int basicAtkIndex = 0;
    [SerializeField] private int life = 50;
   	[SerializeField] private float atkRange = 1;
   	[SerializeField] private float atkCD = 0.02f;
   	[SerializeField] private float actAtkCD = 0f;
   	[SerializeField] private bool storingAtk = false;


    //Movimentation Variables

    
    void passToOrbs()
    {
        foreach(OrbBehaviour o in Orbs)
        {
            o.getOwner(this.gameObject);
        } 
    }
    


    void Start()
    {	
    	inPut = InputManager.instance;
    	trs = this.gameObject.GetComponent<Transform>();
        anim = glz.gameObject.GetComponent<Animator>();
        coll = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        deltaMove = ColisionDetect(deltaMove);
        trs.Translate(deltaMove * Time.fixedDeltaTime);
        if(actAtkCD > 0)
        {
            actAtkCD -= Time.fixedDeltaTime;
        }
        else
        {
            if(storingAtk)
            {
                CastAtkOrb();
                storingAtk = false;
            }
        }
    }
    void Update()
    {
        
        deltaMove = MovimentInput();
        GeneralInputs();
        PlayAnim(direction, state);
        FloatingEffect();

    }

    private void GeneralInputs()
    {
    	//Debug.DrawLine((Vector2)trs.position, (LookDirection*3 )+ (Vector2)trs.position, Color.red, 2f);
    	//Debug.DrawLine(LookDirection*2.5f + (Vector2)trs.position - Vector2.Perpendicular(LookDirection)/2, LookDirection*2.5f + (Vector2)trs.position + Vector2.Perpendicular(LookDirection)/2, Color.red, 2f);
    	if(inPut.GetButtonUp(BindableActions.Atk))
    	{
    		if(actAtkCD <= 0)
    		{
    			CastAtkOrb();
    		}
    		else if(actAtkCD <= atkCD * 2/3)
    		{
    			storingAtk = true;
    		}
    		else
    		{
    			Orbs[basicAtkIndex].UnLoadAtk();
    		}
    	}
    	if(inPut.GetButtonDown(BindableActions.Atk) && actAtkCD <= atkCD)
    	{
    		Orbs[basicAtkIndex].LoadAtk();
    	}
    }

    private void ChangeAtkOrb(int range = 1)
    {
        basicAtkIndex += range;
    	if(basicAtkIndex >= Orbs.Count || basicAtkIndex <= 0)
    	{
    		basicAtkIndex = 0;
    	}	
    }

    private void CastAtkOrb()
    {

      	Orbs[basicAtkIndex].Attack(LookDirection*atkRange, basicAtkIndex);
      	actAtkCD = atkCD + (basicAtkIndex/5f);
      	ChangeAtkOrb();
    }

    public Vector2 MovimentInput()
    {
    	float actarget = basespeed;
    	float y = inPut.GetAxisRaw(BindableActions.Vertical);
    	float x = inPut.GetAxisRaw(BindableActions.Horizontal);
    	Vector2 v = new Vector2(x, y).normalized;
    	if(v != Vector2.zero)
    	{
    		LookDirection = v;
            //direction = (2 + x*(-1+ (y))); 
    		direction = 2*((2 - x)*Mathf.Abs(x) + (1 + y)*Mathf.Abs(y))*(1-Mathf.Abs(x*y)) + 2*((x*y/2 + 2-x)*Mathf.Abs(x*y));
    		state = 1;
    		if(inPut.GetButton(BindableActions.Run))
	    	{
	    		state = 2;
	    		actarget = runspeed;
	    	}

	    	if(inPut.GetButtonDown(BindableActions.Def))
	    	{
	    		//state = 5
	    		speed = speed*5;
	    	}
	    	//0 = Down, 1 = Right, 2 = Up, 3 = Left.     		
	    	
    	}
    	else
        {
            state = 0;
        }
        speed = Mathf.SmoothDamp(speed, actarget, ref aceleration, 0.4f);
    	return v*speed;
    }

    private Vector2 ColisionDetect(Vector2 dmove)
    {
    	Vector2 V = Vector2.zero;
    	RaycastHit2D hit = Physics2D.BoxCast(trs.localPosition, coll.size, 0, dmove.y*Vector2.up, Mathf.Abs(dmove.y*Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Water"));
    	if(hit.collider == null)
    	{
    		V += Vector2.up*dmove;
    	}
    	hit = Physics2D.BoxCast(trs.localPosition, coll.size, 0, dmove.x*Vector2.right, Mathf.Abs(dmove.x*Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Water"));
    	if(hit.collider == null)
    	{
    		V += Vector2.right*dmove;
    	}
    	return V;
    }

    public void PlayAnim(float dir, int st)
    {
    	anim.SetFloat("Dir", dir);
        anim.SetFloat("State", st);
    }
    private void FloatingEffect()
    {
    	glz.localPosition = (Mathf.Sin(Time.time*2)/5)*Vector3.up;
    
    }


    public void takeDamage(int dmg, Vector2 knkback, string type, GameObject dmg_origin)
    {
    	life -= dmg;
    	if(life <= 0)
    	{
    		die(dmg_origin);
    	}
    }

    public void die(GameObject dmg_origin)
    {
    	Debug.Log(dmg_origin);
    }

}
