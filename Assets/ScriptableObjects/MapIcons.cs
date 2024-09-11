using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapIconsConfig", menuName = "ScriptableObjects/MapIcons", order = 1)]
public class MapIcons : ScriptableObject
{
    public MapIconBlueprint[] icons;
}

[System.Serializable]
public struct MapIconBlueprint
{
    public NodeType nodeType;
    public Sprite sprite;
};