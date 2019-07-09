using UnityEngine;

[RequireComponent(typeof(EnemyPathfinding))]
public class EnemyAI : MonoBehaviour {

	private Area area;
	private EnemyPathfinding pathfinding;

	//[Header("Patrol")]
	//[Range(0, 200)]
	//public float MinPatrolDistance = 5;
	//[Range(0, 200)]
	//public float MaxPatrolDistance = 20;

	// Use this for initialization
	void Start () {
		area = transform.parent.GetComponent<Area>();
		if (area == null)
		{
			area = transform.parent.GetComponentInChildren<Area>();
			//findArea();
			if (area == null)
			{
				Debug.LogError("No area found in enemy AI");
			}
		}

		pathfinding = GetComponent<EnemyPathfinding>();
	}

	// Update is called once per frame
	void Update () {
		if (pathfinding.HasNoTarget || pathfinding.HasReachedTarget)
		{
			//// Look for a patrol direction
			//Vector3 patrolDirection = (transform.position - randPosition()).normalized;
			//// Getting a random distance in specified range
			//float distance = MinPatrolDistance + Random.value * (MinPatrolDistance - MaxPatrolDistance);
			//// Compute the patrol point
			//Vector3 patrolPoint = patrolDirection * distance;
			//Debug.Log("Temp patrolPoint = " + patrolPoint);
			
			// Checking that we will stay in the area
			//Vector3 patrolFromAreaCenter = patrolPoint - area.Center;
			//bool inArea = patrolFromAreaCenter.sqrMagnitude < area.SqrRadius;
			//if (!inArea)
			//{
			//	Vector3 dir = patrolFromAreaCenter.normalized;
			//	patrolPoint = dir * area.Radius;
			//	Debug.Log("!inArea patrolPoint = " + patrolPoint);
			//}

			Vector3 patrolPoint = randPosition();
			pathfinding.Target = patrolPoint;
		}
	}

	private Vector3 randPosition()
	{
		Vector3 pos = Random.insideUnitSphere;
		pos *= area.Radius;
		pos += area.transform.position;
		pos.z = transform.position.z;
		//Debug.Log("randPosition = " + pos);
		return pos;
	}

	
	private void findArea()
	{
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Area"))
		{
			Area goArea = go.GetComponent<Area>();
			if (goArea != null && isInArea(goArea))
			{
				area = goArea;
				break;
			}
		}
	}

	private bool isInArea(Area goArea)
	{
		return (goArea.transform.position - transform.position).sqrMagnitude < goArea.SqrRadius;
	}
}
