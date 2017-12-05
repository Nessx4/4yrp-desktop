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

	private Camera cam;

    public static PointerController control;

	private void Start()
	{
        control = this;

		cam = GetComponent<Camera>();

        if (pointerRoot == null)
            pointerRoot = new GameObject("PointerRoot");

        DontDestroyOnLoad(pointerRoot.gameObject);
        mobilePointers = new List<Pointer>();

        if (desktopPointer == null)
        {
            desktopPointer = Instantiate(pointerPrefab, pointerRoot.transform);
            desktopPointer.SetPointerType(PointerType.DESKTOP, 0);
        }
    }

	// Move the desktop Pointer to mouse position.
	private void Update()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10.0f;
		Vector3 pos = cam.ScreenToWorldPoint(mousePos);
		desktopPointer.SetPosition(pos);
	}

	private void LateUpdate()
	{
		foreach (Pointer mobilePointer in mobilePointers)
			mobilePointer.BoundPosition(cam);
	}

	public int CreateMobilePointer()
	{
		Pointer mobilePointer = Instantiate(pointerPrefab, pointerRoot.transform);
		mobilePointer.SetPointerType(PointerType.MOBILE, mobilePointers.Count);

		mobilePointers.Add(mobilePointer);
        return mobilePointers.Count - 1;
	}

	public void MovePointer(int id, Vector2 move)
	{
		mobilePointers[id].Move(move);
	}

    public Vector2 GetPointerPos(int id)
    {
		if(CreatorCamera.creatorCam != null)
		{
			return mobilePointers[id].transform.position - cam.transform.position + CreatorCamera.creatorCam.transform.position;
		}

        return mobilePointers[id].transform.position;
    }
}
