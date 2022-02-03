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




    private Transform trs;
    [SerializeField] private Transform glz;
    private Animator anim;
    private Rigidbody2D rigb;

    void Start()
    {	
    	trs = this.gameObject.GetComponent<Transform>();
        anim = glz.gameObject.GetComponent<Animator>();
        rigb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigb.velocity = InputMoviment();
        PlayAnim(direction, state);
        FloatingEffect();
    }
    public Vector2 InputMoviment()
    {
    	float y = Input.GetAxisRaw("Vertical");
    	float x = Input.GetAxisRaw("Horizontal");
    	if(y != 0f && x == 0f)
		{
			if(y > 0)
			{
				direction = 1;
                
				//anim.Play("up-idle");			
			}
			else
			{
				direction = 0;
				//anim.Play("down-idle");
                
			}
		}
    	else if(x != 0f && y == 0f)
    	{
			if(x > 0)
			{
				direction = 2;
				//anim.Play("right-idle");	
                		
			}
			else
			{
				direction = 3;
				//anim.Play("left-idle");
                
		    }
    	}

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

    public void PlayAnim(int dir, int st)
    {
    	anim.SetFloat("Dir", dir);
        anim.SetFloat("State", st);
    }
    private void FloatingEffect()
    {
    	glz.position += Mathf.Cos(Time.time*2)*Vector3.up/500;
    }
}
