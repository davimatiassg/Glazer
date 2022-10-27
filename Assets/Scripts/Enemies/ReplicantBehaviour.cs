using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplicantBehaviour : DefaultEnemy
{
	private bool isATK = false;
	private bool trigered = false;
	private bool isMOV = false;
	private int routeIndex = 0;
	private float spdBUFF = 1;
	private Vector2 destination;
	private Color SigilColor = Color.green;
	[SerializeField] private float atkRange = 1;
	[SerializeField] private List<Vector2> routePoints;

	public void FixedUpdate()
	{
		Vector3 playerP = player.position;
		Vector2 distance = ((Vector2)(playerP - trs.position));
		if(!damaged)
		{	
			if(!isATK)
			{
				if(trigered == true && distance.sqrMagnitude >= sqrV * 3f)
				{
					trigered = false;
					isMOV = false;
					SetColor(_col)
				}
				if(trigered == false && distance.sqrMagnitude <= sqrV)
				{
					if(SetTriggered() == true)
					{
						trigered = true;
					}
				}

				
				if(trigered == true)
				{
					
					velR = Chase();
					if(distance.magnitude <= atkRange)
					{
						Attack();
					}
					spdBUFF = 1f;
				}
				else
				{
					velR = WaddleAround();
					spdBUFF = 0.75f;
				}			
			}
			else
			{
				spdBUFF = 2f;
				velR = Chase();
			}
		}
		else
		{
			velR -= velR * weight/100;
			anim.SetBool("Hurt", true);
		}
		trs.position = Vector2.MoveTowards((Vector2)trs.position, (Vector2)trs.position + velR, Time.fixedDeltaTime*spdBUFF*speed/weight);
	}

	Vector2 WaddleAround()
	{
		if(Random.Range(0, 200) == 1)
		{
			routeIndex ++;
			if(routeIndex >= routePoints.Count || routeIndex < 0)
			{
				routeIndex = 0;
			}
			isMOV = true;
		}

		destination = routePoints[routeIndex] - (Vector2)trs.position;

		if(destination.magnitude <= 0.5f)
		{
			anim.SetBool("Walk", false);
			isMOV = false;
			return Vector2.zero;
		}
		Flip(destination);
		anim.SetBool("Walk", true);
		return destination.normalized * speed;
		
	}
	void Flip(Vector2 dir)
	{
		trs.localScale = scale* (new Vector3(-Mathf.Sign(dir.x), 1f, 1f));
	}
	Vector2 Chase(float spdMOD = 1.5f)
	{
		isMOV = true;
		if(!isATK)
		{
			destination = player.position - trs.position;
		}

		anim.SetBool("Walk", true);
		Flip(destination);
		return destination.normalized * speed*spdMOD;
	}

    void Attack()
    {	
        isATK = true;
        isMOV = false;
        anim.SetBool("Walk", false);
       	anim.SetBool("Atk", true);
    }
    public void StandStill()
    {
    	destination = Vector2.zero;
    	velR = Vector2.zero;
    }

    public void StopAtk()
    {
    	isATK = false;
    	anim.SetBool("Atk", false);
    }

    public void StopStun()
    {
    	damaged = false;
    	anim.SetBool("Hurt", false);
    	if(life <= 0)
    	{
    		Destroy(gameObject);
    	}
    }
    void OnDrawGizmosSelected()
    {
    	Gizmos.color = new Color(0, 1, 0, 0.75F);

    	foreach (Vector2 p in routePoints)
    	{
    		 Gizmos.DrawWireSphere(p, 0.5f);
    	}
    }
}
