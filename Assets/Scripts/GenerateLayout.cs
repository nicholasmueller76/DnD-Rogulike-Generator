using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomTypes {standardCombat, eliteCombat, chanceRoom, trader, restSite, lootRoom, hazard, superEliteCombat }

public class GenerateLayout : MonoBehaviour
{
    public MapIcons mapIcons;

    public RoomProbability roomProbability;

    public List<GameObject> rooms = new List<GameObject>();

    public GameObject roomParent;

    public bool isShadeFloor;
    private List<GameObject> eliteRooms = new List<GameObject>();
    private List<GameObject> notEliteRooms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int children = roomParent.transform.childCount;
        for (int i = 0; i < children; i++)
            rooms.Add(roomParent.transform.GetChild(i).gameObject);
        GenerateFloor();
    }

   public void GenerateFloor()
    {
        eliteRooms.Clear();
        notEliteRooms.Clear();

        foreach(GameObject g in rooms)
        {
            RoomTypes randRoom = roomProbability.probabilityArray[Random.Range(0, roomProbability.probabilityArray.Length)];
            if (randRoom == RoomTypes.eliteCombat)
            {
                if (eliteRooms.Count > 3)
                {
                    randRoom = RoomTypes.standardCombat;
                    notEliteRooms.Add(g);
                }
                else eliteRooms.Add(g);
            }
            else
            {
                notEliteRooms.Add(g);
            }

            /*switch (randRoom)
            {
                case RoomTypes.standardCombat:
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.standardCombat;
                    break;
                case RoomTypes.eliteCombat:
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.eliteCombat;
                    break;
                case RoomTypes.chanceRoom:
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.chanceRoom;
                    break;
                case RoomTypes.trader:
                    int randNum = Random.Range(0, 4);
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.traders[randNum];
                    break;
                case RoomTypes.restSite:
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.restSite;
                    break;
                case RoomTypes.lootRoom:
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.lootRoom;
                    break;
                case RoomTypes.superEliteCombat:
                    g.GetComponent<SpriteRenderer>().sprite = mapIcons.superEliteCombat;
                    break;
                default:
                    break;
            }*/
        }

        if (eliteRooms.Count < 3)
        {
            for (int i = 0; i < (3 - eliteRooms.Count); i++)
            {
                GameObject eliteRoom = notEliteRooms[Random.Range(0, notEliteRooms.Count)];
                eliteRooms.Add(eliteRoom);
                //eliteRoom.GetComponent<SpriteRenderer>().sprite = mapIcons.eliteCombat;
            }
        }

        //notEliteRooms[Random.Range(notEliteRooms.Count / 2, notEliteRooms.Count)].GetComponent<SpriteRenderer>().sprite = mapIcons.restSite;

        //if (isShadeFloor) eliteRooms[Random.Range(0, eliteRooms.Count)].GetComponent<SpriteRenderer>().sprite = mapIcons.superEliteCombat;
    }
}
