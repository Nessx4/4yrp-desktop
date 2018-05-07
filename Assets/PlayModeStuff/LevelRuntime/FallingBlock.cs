using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    private float timer;

	[SerializeField]
    private FallingType type;

	[SerializeField]
	private Transform graphic;

	private Coroutine shakeRoutine;

	private new Rigidbody2D rigidbody;

	// Cache the rigidbody.
	private void Start ()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

    // INSTANT blocks fall after a fixed delay upon being landed on.
    private void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log("Trigger Enter");
        if (type == FallingType.INSTANT)
            Invoke("Fall", 1.0f);

		if(type == FallingType.WAIT && shakeRoutine != null)
		{
			StopCoroutine(shakeRoutine);
			shakeRoutine = null;
		}
		
		// Shake if not already falling.
		if(shakeRoutine == null && type != FallingType.FALLING)
			shakeRoutine = StartCoroutine(Shake());
    }

	// Become a falling object.
    private void Fall()
    {
		type = FallingType.FALLING;

		if(shakeRoutine == null)
			StopCoroutine(shakeRoutine);

		rigidbody.bodyType = RigidbodyType2D.Dynamic;
		gameObject.layer = 10;
    }

    // WAIT blocks fall after player has stood on it for a fixed duration.
    private void OnTriggerStay2D(Collider2D other)
    {
		Debug.Log("Trigger Stay " + Time.time);
        if (type == FallingType.WAIT /*&& other.transform.position.y > transform.position.y + 0.8f*/)
        {
			timer += Time.deltaTime;

			if (timer > 1.0f)
				Fall();
		}
    }

	// Shake to warn the player of an imminent fall.
	private IEnumerator Shake()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		Vector3 startPos = graphic.localPosition;
		float t = 0.0f;
		while(true)
		{
			Vector3 pos = graphic.localPosition;

			pos.x = Mathf.Sin(t * 50.0f) / 15.0f;
			graphic.localPosition = pos;

			t += Time.deltaTime;
			yield return wait;
		}
	}

	// Stop shaking and counting down towards a fall if this is a Wait platform.
    private void OnTriggerExit2D(Collider2D other)
    {
		Debug.Log("Trigger Exit");
		if (type == FallingType.WAIT)
		{
			timer = 0.0f;

			if(shakeRoutine != null)
			{
				StopCoroutine(shakeRoutine);
				shakeRoutine = null;
			}
		}
    }

    public enum FallingType
    {
        INSTANT, WAIT, FALLING
    }
}
