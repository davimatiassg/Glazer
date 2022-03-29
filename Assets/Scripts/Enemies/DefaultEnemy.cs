using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour, HitableObj
{
    [SerializeField] protected int life = 50;
    [SerializeField] protected Transform trs;
    [SerializeField] protected Vector2 velR;
    [SerializeField] protected int weight = 1;
    [SerializeField] protected int speed = 1; 
    [SerializeField] protected bool damaged = true;
    [SerializeField] protected Transform player;
    [SerializeField] protected float visionRange = 3f;

    [SerializeField] protected float scale;
  
    [SerializeField] protected Animator anim;
    private protected float sqrV;

    void Start()
    {   
        anim = this.gameObject.GetComponent<Animator>();
        trs = this.gameObject.GetComponent<Transform>();
        if(weight == 0)
        {
        	weight = 1;
        }
        player = GameObject.FindWithTag("Player").transform;
        sqrV = visionRange*visionRange;
        scale = trs.localScale.y;
    }

    public void takeDamage(int dmg, Vector2 knkback, string type, GameObject dmg_origin)
    {
    	life -= dmg;
    	velR = knkback/weight;
        damaged = true;
    	if(life <= 0)
    	{
    		die(dmg_origin);
    	}
    }

	public void die(GameObject dmg_origin)
	{
		Debug.Log("killed by " + dmg_origin.ToString());
	}
}
