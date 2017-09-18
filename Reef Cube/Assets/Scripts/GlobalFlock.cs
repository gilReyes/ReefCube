using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {
    public GameObject boidPrefab;
    [SerializeField]public int flockQuantity = 10;
    public static int enviromentSize = 10;
    public GameObject[] flock;

    public static Vector3 goalPos;
	// Use this for initialization
	void Start () {
        flock = new GameObject[flockQuantity];
        goalPos = new Vector3(Random.Range(-enviromentSize, enviromentSize),
                                      Random.Range(-enviromentSize, enviromentSize),
                                      Random.Range(-enviromentSize, enviromentSize));
        for ( int i = 0; i < flockQuantity; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-enviromentSize, enviromentSize),
                                      Random.Range(-enviromentSize, enviromentSize),
                                      Random.Range(-enviromentSize, enviromentSize));
            flock[i] = (GameObject)Instantiate(boidPrefab, pos, Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Random.Range(0,10000) < 100)
        {
            goalPos = new Vector3(Random.Range(-enviromentSize, enviromentSize),
                                  Random.Range(-enviromentSize, enviromentSize),
                                  Random.Range(-enviromentSize, enviromentSize));
        }
	}
}
