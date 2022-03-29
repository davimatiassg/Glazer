using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HitableObj 
{

	void takeDamage(int dmg, Vector2 knkback, string type, GameObject dmg_origin);
	//método de tomar dano

	void die(GameObject dmg_origin);
	//método de morrer/explodir/autodestruir

}
