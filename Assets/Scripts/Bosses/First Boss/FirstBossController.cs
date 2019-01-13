using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : MonoBehaviour {
    
    public GameObject orbiterDeath;
    public GameObject orbiterFamine;
    public GameObject orbiterPestilence;
    public GameObject orbiterWar;

    GameObject gameplayManager;

    List<GameObject> orbs;

    static float radius = 4.5f;

    Quaternion standardOrientationBoss = new Quaternion(0, 0, 0, 0);

	// Use this for initialization
	void Start () {
        orbs = new List<GameObject>();
        orbs.Add(Instantiate(orbiterPestilence, new Vector2(0, radius), standardOrientationBoss));
        orbs.Add(Instantiate(orbiterWar, new Vector2(radius, 0), standardOrientationBoss));
        orbs.Add(Instantiate(orbiterFamine, new Vector2(0, -radius), standardOrientationBoss));
        orbs.Add(Instantiate(orbiterDeath, new Vector2(-radius, 0), standardOrientationBoss));

        foreach (GameObject orb in orbs) {
            orb.GetComponent<OrbitingFirerer>().SetController(gameObject);
        }
	}

    public static float GetRadius() {
        return radius;
    }

    public void SetGameManager(GameObject newGameManager) {
        gameplayManager = newGameManager;
    }

    public void SetOrbDead(GameObject deadOrb) {
        orbs.Remove(deadOrb);
        if (orbs.Count == 0) {
            gameplayManager.GetComponent<GameplayManager>().EndFirstBoss();
            Destroy(gameObject);
            return;
        }

        if (orbs.Count == 2) {
            foreach(GameObject orb in orbs)
            {
                orb.GetComponent<OrbitingFirerer>().SetAsFinalOrb();
            }
            orbs[0].GetComponent<OrbitingFirerer>().GoClockwise();
            //orbs[0].GetComponent<OrbitingFirerer>().SetAsFinalOrb();
            return;
        }
        
        if (orbs.Count == 1) {
            return;
        }

        foreach (GameObject orb in orbs) {
            orb.GetComponent<OrbitingFirerer>().IncreaseDifficulty();
        }
    }
}
