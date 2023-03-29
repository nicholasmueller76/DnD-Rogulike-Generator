using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLayout : MonoBehaviour
{
    //Order is same as enum order.
    public Sprite[] sprites;

    public List<GameObject> rooms = new List<GameObject>();

    public GameObject roomParent;

    public enum RoomTypes {standard, elite, chance, trader, healing, shade }

    public RoomTypes[] probabilityArray;

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
            RoomTypes randRoom = probabilityArray[Random.Range(0, probabilityArray.Length)];
            if (randRoom == RoomTypes.elite)
            {
                if (eliteRooms.Count > 3)
                {
                    randRoom = RoomTypes.standard;
                    notEliteRooms.Add(g);
                }
                else eliteRooms.Add(g);
            }
            else
            {
                notEliteRooms.Add(g);
            }

            g.GetComponent<SpriteRenderer>().sprite = sprites[(int)randRoom];
        }

        if (eliteRooms.Count < 3)
        {
            for(int i = 0; i < (3-eliteRooms.Count); i++)
            notEliteRooms[Random.Range(0, notEliteRooms.Count)].GetComponent<SpriteRenderer>().sprite = sprites[(int)RoomTypes.elite];
        }

        notEliteRooms[Random.Range(notEliteRooms.Count / 2, notEliteRooms.Count)].GetComponent<SpriteRenderer>().sprite = sprites[(int)RoomTypes.healing];

        if (isShadeFloor) rooms[Random.Range(0, eliteRooms.Count)].GetComponent<SpriteRenderer>().sprite = sprites[(int)RoomTypes.shade];
    }
}
