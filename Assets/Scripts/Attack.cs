using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public int damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            enemy.TookDamage(damage);
        } else
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TookDamage(damage);
            }
        }
    }
}
