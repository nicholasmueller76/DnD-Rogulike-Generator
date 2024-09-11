using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomProbabilityConfig", menuName = "ScriptableObjects/RoomProbability", order = 1)]
public class RoomProbability : ScriptableObject
{
    public RoomTypes[] probabilityArray;
}
