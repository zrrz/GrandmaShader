using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float moveSpeed = 3.0f;
//	public float sprintSpeedMod = 1.8f;
	public float turnSpeed = 0.6f;
	
	Animator anim;
	//	public Animator topAnimator;
	//	public Animator bottomAnimator;
	//	
//	public bool grounded;
	
//	const float GROUND_CHECK_DISTANCE = 1.0f;
	
//	public LayerMask mask;
	
//	public float jumpStrength = 3.0f;
	
//	float snowLevel = 0.0f;
	
//	public float snowAccumSpeed = 0.2f;
	
//	bool moved;
	
//	Rigidbody rigidbody;
	
	//	public GameObject enemy;
	
	//	public Material mat;
	
	//	[System.NonSerialized]					
	//	public float lookWeight;					// the amount to transition when using head look	
	//	
	//	public float lookSmoother = 3f;				// a smoothing setting for camera motion

	enum State {Idle, Patrol, Shoot};

	State state;

	const float MIN_DIST = 5.0f;
	const float MAX_DIST = 500f;

//	public Transform cameraObj;

	float timer = 0f;
	public float waitTime = 2.0f;
	float waitTimeRange = 1.8f;
	public float shootDistance = 10.0f;
	public GameObject player;

	NavMeshAgent agent;

	Vector3 startPoint;

	public float shootTime = 1.0f;

	public Transform shootPoint;

	public GameObject bullet;

	Material mat;
	
	void Start () {
		waitTime += Random.Range(-waitTimeRange, waitTimeRange);
		startPoint = transform.position;
		agent = GetComponent<NavMeshAgent>();
//		input = GetComponent<BaseInput>();
//		rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		mat = transform.GetChild(0).GetComponent<Renderer>().material;
	}
	
	void Update () {
		float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
		distance = (distance - MIN_DIST) / (MAX_DIST - MIN_DIST);
		mat.SetFloat("_DistToCamera", distance);

		Debug.DrawRay(startPoint, Vector3.up, Color.red);
		switch(state) {
		case State.Idle:
			timer += Time.deltaTime;
			if(timer > waitTime) {
				timer = 0f;
				state = State.Patrol;
				anim.SetBool("Move", true);
				agent.SetDestination(startPoint + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)));
			}
			if(Vector3.Distance(transform.position, player.transform.position) < shootDistance) {
				state = State.Shoot;
				timer = 0f;
				anim.SetBool("Shoot", true);
				mat.SetInt("_Alerted", 1);
			}
			break;

		case State.Patrol:
			if(Vector3.Distance(agent.destination, transform.position) < 1.0f) {
				state = State.Idle;
				anim.SetBool("Move", false);
			}
			if(Vector3.Distance(transform.position, player.transform.position) < shootDistance) {
				state = State.Shoot;
				timer = 0f;
				anim.SetBool("Shoot", true);
				mat.SetInt("_Alerted", 1);
				agent.Stop();
			}
			break;

		case State.Shoot:
			Vector3 rot = transform.eulerAngles;
			rot.y = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles.y;
			transform.eulerAngles = rot;

			timer += Time.deltaTime;
			if(timer > shootTime) {
				Shoot();
				timer = 0f;
			}
			if(Vector3.Distance(player.transform.position, transform.position) > shootDistance) {
				state = State.Patrol;
				anim.SetBool("Shoot", false);
				anim.SetBool("Move", true);
				mat.SetInt("_Alerted", 0);
//				agent.SetDestination(startPoint + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)));
				agent.Resume();
			}
			break;
		}
	}
	
	void Shoot() {
		Vector3 dir = player.transform.position - shootPoint.transform.position;
		dir.Normalize();
		GameObject bulletObj = (GameObject)Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(dir));
		bulletObj.GetComponent<Rigidbody>().AddForce(dir*1000f);
//		Destroy(bulletObj, 8.0f);
	}

	void Die() {
		Destroy(gameObject);
		for(int i = 0; i < 100; i++) {
			Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			dir.Normalize();
			GameObject bulletObj = (GameObject)Instantiate(bullet, shootPoint.position + dir, Quaternion.LookRotation(dir));
			bulletObj.GetComponent<Rigidbody>().AddForce(dir*600f);
			bulletObj.GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
