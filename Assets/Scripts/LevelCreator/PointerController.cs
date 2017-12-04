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

    public static PointerController control;

	private void Start()
	{
        control = this;
        if (pointerRoot == null)
        {
            Debug.Log("making new pointer");
            pointerRoot = new GameObject("PointerRoot");
        }
        DontDestroyOnLoad(pointerRoot.gameObject);
        mobilePointers = new List<Pointer>();

        if (desktopPointer == null)
        {
            Debug.Log("making new paaasasasaointer");
            desktopPointer = Instantiate(pointerPrefab, pointerRoot.transform);
            desktopPointer.SetPointerType(PointerType.DESKTOP, 0);
        }
            DontDestroyOnLoad(desktopPointer.gameObject);

        }

	// Move the desktop Pointer to mouse position.
	private void Update()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		desktopPointer.SetPosition(pos);
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
        return mobilePointers[id].transform.position;
    }
}
