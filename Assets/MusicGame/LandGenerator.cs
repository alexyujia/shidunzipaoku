using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject land;
    private int land_count = 0;

    public const int LAND_LENGTH = 80;
    public const int OFFSET = 30;
    public const int PRE_OFFSET = 200;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (player.transform.position.z > land_count * LAND_LENGTH - OFFSET - PRE_OFFSET){
            land_count += 1;
            GameObject newland = Instantiate(land);
            newland.GetComponent<MusicLand>().setLand();
            newland.transform.position = new Vector3(0,0,land_count * LAND_LENGTH - OFFSET);
        }
    }
}
