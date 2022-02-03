using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobesBehavior : MonoBehaviour
{
	[SerializeField] Transform destination;
	[SerializeField] float acel = 0f;
	[SerializeField] float rotspeed = 0f;
	[SerializeField] float orbitrange = 0f;
	//[SerializeField] float sense = 0f;

	private Transform trs;
	private Rigidbody rigb;

	void Start()
	{
		trs = this.gameObject.GetComponent<Transform>();
		rigb = this.gameObject.GetComponent<Rigidbody>();
	}

	void Update()
	{	
		float x = Mathf.Cos(Time.time*rotspeed)*orbitrange;
		float y = Mathf.Sin(Time.time*rotspeed)*orbitrange;
		Vector3 rot = new Vector3(x, y/2, y);

		Vector3 dest = destination.position + rot - trs.position;
		rigb.velocity = acel*dest;
		//trs.position = Vector3.Slerp(trs.position, destination.position+ new Vector3(orbitrange, -orbitrange, orbitrange), acel/100);
    }
    
	
/*
	public Vector3 EvaluateSlerpPoints(Vector3 sta, Vector3 dst, float off, float spd)
	{
		var cpiv = (sta + dst) *0.5f;
		cpiv -= Vector3.down*off;
		var strelc = sta - cpiv;
		var dsrelc = dst - cpiv;

		var f = spd/100;
		return Vector3.Slerp(strelc, dsrelc, f) + cpiv;
	}
	*/

}
