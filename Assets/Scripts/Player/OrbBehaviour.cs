using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour
{

	//movimentation
	
	[SerializeField] float acel = 0f;
	[SerializeField] float rotspeed = 0f;
	[SerializeField] float orbitrange = 0f;
	private Vector2 target;
	private Vector2 dest;

	//self ref
	[SerializeField] Transform destination;
	private Transform trs;
	private Rigidbody rigb;
	private CircleCollider2D col;
	[SerializeField] private TrailRenderer trail;
	private GameObject Owner;

	//atk
	[SerializeField] int damage = 5;
	[SerializeField]private bool atk = false;
	[SerializeField]private bool charge = false;
	public string atktype;

	[SerializeField] private float cd = 0;
	[SerializeField] private Vector2 storedTRG = Vector2.zero;

	private int atkform = 0;
	void Start()
	{
		trs = this.gameObject.GetComponent<Transform>();
		rigb = this.gameObject.GetComponent<Rigidbody>();
		col = this.gameObject.GetComponent<CircleCollider2D>();
		//trail = this.gameObject.GetComponent<TrailRenderer>();
	}
	public void getOwner(GameObject g)
	{
		Owner = g;
	}

	public void LoadAtk()
	{
		if(!atk && !charge)
		{	
			trs.position = (Vector2)destination.position;
			charge = true;
		}
		trail.emitting = true;
	}
	public void UnLoadAtk()
	{
		if(!atk && charge)
		{	
			trail.emitting = false;
			charge = false;
		}
		
	}

	public void Attack(Vector2 trg, int index)
    {   
    	if(!atk && charge)
    	{
    		col.enabled = true;
    		target = trg;
    		atkform = index;
	    	if(index == 0)
	    	{
	    		
		    	trs.position = (Vector2)destination.position - Vector2.Perpendicular(new Vector2(trg.x*0.9961f - trg.y*0.0871f, trg.y*0.9961f + trg.x*0.0871f));
		    	dest = Vector2.Perpendicular(trg);
	    	}
	    	else if(index == 1)
	    	{
	    		
		    	trs.position = (Vector2)destination.position + Vector2.Perpendicular(new Vector2(trg.x*0.9961f + trg.y*0.0871f, trg.y*0.9961f - trg.x*0.0871f))*1.25f;
		    	dest = -Vector2.Perpendicular(trg)*1.25f;
	    	}
	    	else if(index == 2)
	    	{
	    		trs.position = (Vector2)destination.position - trg;
	    		dest = trg*2.5f;
	    	}
	    	charge = false;
	    	atk = true;
	    	trail.widthMultiplier = col.radius/4;
	    	
	    	//Debug.DrawLine(trs.position, dest, Color.green, 5, false);
	    	//Debug.DrawLine((Vector2)destination.position, (Vector2)destination.position + trg, Color.green, 5, false);
	    }
    }

    public void Unleash(int index)
    {
    	
    	Vector2 v = (trs.position - destination.position);

    	if(index < 2)
    	{	    		
    		trs.position = destination.position + Vector3.RotateTowards(v, dest, 4*acel*Time.deltaTime, 0.05f);
    	}
    	else 
    	{
    		trs.position = Vector2.MoveTowards(trs.position, (Vector2)destination.position + dest, 5*acel*Time.deltaTime);
    	}
    	Vector2 p = (Vector2)destination.position + dest;
		if(Vector2.Distance(trs.position, p) < 0.2f)
		{
			atk = false;
			col.enabled = false;
			trail.emitting = false;
			trail.widthMultiplier = trs.localScale.x;
		}
    }



	void Update()
	{	
		if(charge)
		{
			target = destination.position;
			Orbitate(target, orbitrange/5, rotspeed*8);
		}
		else if(atk)
		{
			Unleash(atkform);
		}
		else
		{
			target = destination.position;
			Orbitate(target, orbitrange, rotspeed);
		}
		
		//trs.position = Vector3.Slerp(trs.position, destination.position+ new Vector3(orbitrange, -orbitrange, orbitrange), acel/100);
    }

    public void Orbitate(Vector3 tg, float range, float vel)
    {
    	float x = Mathf.Cos(Time.time*vel)*range;
		float y = Mathf.Sin(Time.time*vel)*range;
		Vector3 rot = new Vector3(x, y/2, y);
		trs.position = Vector3.MoveTowards(trs.position, tg + rot, acel*Time.fixedDeltaTime/1.5f);
    }

   

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(!col.gameObject.tag.Equals("Player") && !col.isTrigger)
    	{
    		HitableObj p = col.gameObject.GetComponent<HitableObj>();
    		if(p != null)
    		{	
    			p.takeDamage(damage, target, atktype, this.gameObject);
    		}
    	}
    }

   public Vector2 SlerpTowards(Vector2 start, Vector2 end, Vector2 center, float maxDistanceDelta, int clockdir) 
   {	
   		
        Vector2 ray = (start - center);
        end = (end - center);
        float d = Mathf.Sign(clockdir);

		//float dot = Vector2.Dot(start-center, end-center);
		//dot = Mathf.Clamp(dot, -1f, 1f);
		
		float dot =  d * Vector2.Angle(ray, end)/180 - (d-1)/2;
		float ang = dot * maxDistanceDelta;
	   	if(ang <= 0.01f)
		{
			return end + center;
		}
	    float c = Mathf.Cos(ang);

	    float s = Mathf.Sin(ang);
	    Vector2 result = new Vector2(ray.x*c - ray.y*s, c*ray.y + ray.x*s);
	    Debug.DrawLine(center, center + result, Color.blue, 5f, false);
	    Debug.DrawLine(center + result, center + end, Color.red, 5f, false);

	    return result  + center;

       
	}

	
/*
	public Vector3 EvaluateSlerpPoints(Vector3 sta, Vector3 dst, float off, float spd)
	{
		var cpiv = (sta + dst) *0.5f;
		cpiv -= Vector3.down*off;
		var strelc = sta - cpiv;
		var dsrelc = dst - cpiv;

		var f = spd/100;
		f
		return Vector3.Slerp(strelc, dsrelc, f) + cpiv;
	}
	*/

}
