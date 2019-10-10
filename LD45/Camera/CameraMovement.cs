using System;
using UnityEngine;
public class CameraMovement : MonoBehaviour {

	public Transform player;

	private float speed = 0.7f;
	private Vector2 velSpeed;

	private float defaultZoom = 8f;
	private float zoomSpeed = 1f;
	private float velZoom;

	private Vector2 startPos;
	private Transform roomT;
	
	public Camera c;

	public static CameraMovement Instance { get; set; }

	private void Awake() {
		Instance = this;
		startPos = transform.position;
	}

	void FixedUpdate () {
		if (player == null) return;
		//Movement
		float playerSpeed = PlayerMovement.Instance.GetRb().velocity.magnitude; 
		Vector2 posOffset = PlayerMovement.Instance.GetRb().velocity / 1.75f;
		Vector2 myPos = new Vector2(player.transform.position.x, player.transform.position.y) + posOffset;

		//Stay within room bounds
		if (roomT != null) {
			Vector2 roomPos = roomT.position;
			float offset = 10f;
			if (myPos.x > roomPos.x + offset) myPos = new Vector2(roomPos.x + offset, myPos.y);
			else if (myPos.x < roomPos.x - offset) myPos = new Vector2(roomPos.x - offset, myPos.y);
			if (myPos.y > roomPos.y + offset) myPos = new Vector2(myPos.x, roomPos.y + offset);
			else if (myPos.y < roomPos.y - offset) myPos = new Vector2(myPos.x, roomPos.y - offset);
		}

		transform.position = Vector2.SmoothDamp(transform.position, myPos, ref velSpeed, speed);

		if (playerSpeed > 11) playerSpeed = 11;
		//Zoom
		float desiredZoom = defaultZoom + (playerSpeed / 4); //+ height;
		Camera.main.orthographicSize =
			Mathf.SmoothDamp(Camera.main.orthographicSize, desiredZoom, ref velZoom, zoomSpeed);
		transform.position = new Vector3(transform.position.x, transform.position.y, -10);
	}

	public void SetPlayer(Transform t) {
		player = t;
	}

	public void StartRound() {
		c.orthographicSize = 0.1f;
		transform.position = player.transform.position;
	}

	public void SetRoom(Transform pos) {
		roomT = pos;
	}
}
