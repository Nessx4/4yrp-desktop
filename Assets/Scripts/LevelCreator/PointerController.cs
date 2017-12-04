/*	This class handles all Pointer movement and spawning all Pointer objects.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PointerController : MonoBehaviour 
{
	[SerializeField]
	private Pointer pointerPrefab;

	private GameObject pointerRoot;

	private Pointer desktopPointer;

	private List<Pointer> mobilePointers;

	private void Start()
	{
		pointerRoot = new GameObject("PointerRoot");

		desktopPointer = Instantiate(pointerPrefab, pointerRoot.transform);
		desktopPointer.SetPointerType(PointerType.DESKTOP, 0);
	}

	// Move the desktop Pointer to mouse position.
	private void Update()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		desktopPointer.SetPosition((Vector2)pos);
	}

	public void CreateMobilePointer()
	{
		Pointer mobilePointer = Instantiate(pointerPrefab, pointerRoot.transform);
		mobilePointer.SetPointerType(PointerType.MOBILE, mobilePointers.Count);

		mobilePointers.Add(mobilePointer);
	}

	public void MovePointer(int id, Vector2 move)
	{
		mobilePointers[id].Move(move);
	}
}
