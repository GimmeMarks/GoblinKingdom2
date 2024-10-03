using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data")]
public class Items : ScriptableObject
{
	public int id;
	public Sprite icon;
	public int price;
}
