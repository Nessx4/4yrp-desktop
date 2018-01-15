/*	TileHistory provides the undo and redo stacks that denote tile placement
 *	history. Also provides functions for performing undo/redo.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileHistory : MonoBehaviour
{
	private void Start()
	{
		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();

		CheckUndoRedo();
	}

	public void AddOperations(List<TileOperation> operations)
	{
		undoStack.Push(operations);
		redoStack.Clear();

		CheckUndoRedo();
	}

	public void Clear(List<TileOperation> operations)
	{
		undoStack.Push(operations);
		redoStack.Clear();

		CheckUndoRedo();
	}

	public void DeleteUndoHistory()
	{
		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();
	}

	// Revert the state of the level back to before an operation.
	public void Undo()
	{
		if (undoStack.Count > 0)
		{
			List<TileOperation> ops = undoStack.Pop();

			foreach (TileOperation op in ops)
				op.Undo();

			redoStack.Push(ops);

			CheckUndoRedo();
		}
	}

	// Reapply the last change that was undone.
	public void Redo()
	{
		if (redoStack.Count > 0)
		{
			List<TileOperation> ops = redoStack.Pop();

			foreach (TileOperation op in ops)
				op.Redo();

			undoStack.Push(ops);

			CheckUndoRedo();
		}
	}

	// Revert the state of the level back to before an operation.
	public void UndoMobile()
	{
		if (undoStackMob.Count > 0)
		{
			List<TileOperation> ops = undoStackMob.Pop();

			foreach (TileOperation op in ops)
				op.Undo();

			redoStackMob.Push(ops);
		}
	}

	// Check if the Undo and Redo buttons need to be greyed out.
	public void CheckUndoRedo()
	{
		if (undoStack.Count > 0)
			undoButton.Show();
		else
			undoButton.Hide();

		if (redoStack.Count > 0)
			redoButton.Show();
		else
			redoButton.Hide();
	}
}
