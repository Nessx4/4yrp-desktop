using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Prefabs/LevelCreator/Tiles/New Tile", menuName = "Level Creator/New Tile")]
public class TileData : ScriptableObject
{
	public string tileName;
	public Sprite tileSprite;
	public GameObject tilePrefab;
}