using Unity.VisualScripting;
using UnityEngine;

public class GenerateLand : MonoBehaviour
{
    private GameObject dunzi;
    public GameObject PresentList;
    public GameObject SpecialPresentList;
    public GameObject TrackList;
    public GameObject streetTemplate;
    public GameObject streetList;
    public const int initialLandCount = 8;
    // 后移补足
    public const int offsetCount = 20;
    public int landCount = 0;
    public int prelandCount = 20;
    public const int LAND_LENGTH = 10;
    public const int STREET_COUNT = 8;
    public const int OFFSET = -30;
    public const int MAX_LAND_RANGE = 1000;  // 最大街块数目
    private int childCount;
    private int specialChildCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        landCount = initialLandCount;
        childCount = PresentList.transform.childCount;
        specialChildCount = SpecialPresentList.transform.childCount;
        dunzi = GlobalTargetManager.dunzi;
    }

    // Update is called once per frame
    void Update()
    {
        // 到达一定数目后整体后移
        if (landCount > MAX_LAND_RANGE){
            dunzi.transform.position -= new Vector3(0,0,(MAX_LAND_RANGE - initialLandCount - offsetCount) * LAND_LENGTH);
            dunzi.GetComponent<Move>().AddOffsetMiles((MAX_LAND_RANGE - initialLandCount - offsetCount) * LAND_LENGTH);
            landCount -= MAX_LAND_RANGE - initialLandCount - offsetCount;
            for(int i = 0; i < TrackList.transform.childCount; i++){
                TrackList.transform.GetChild(i).transform.position -= new Vector3(0,0,(MAX_LAND_RANGE - initialLandCount - offsetCount) * LAND_LENGTH);
            }
            for(int i = 0; i < streetList.transform.childCount; i++){
                streetList.transform.GetChild(i).transform.position -= new Vector3(0,0,(MAX_LAND_RANGE - initialLandCount - offsetCount) * LAND_LENGTH);
            }
        }

        while (dunzi.transform.position.z + LAND_LENGTH * prelandCount > LAND_LENGTH * landCount)
        {
            if (landCount % 2 == 0 && landCount % 8 != 0)
            {
                GameObject newland = Instantiate(PresentList.transform.GetChild(Random.Range(0, childCount)).gameObject, TrackList.transform);
                newland.transform.position = new Vector3(0, 0, (landCount + 1) * LAND_LENGTH + OFFSET);
                newland.GetComponent<Land>().SetDestoryable(true);
                landCount += 1;
            }
            else
            {
                GameObject newland;
                if (Random.value < 0.035)
                {
                    // 道具
                    newland = Instantiate(SpecialPresentList.transform.GetChild(Random.Range(0, specialChildCount)).gameObject, TrackList.transform);
                }
                else
                {
                    // 空地
                    newland = Instantiate(PresentList.transform.GetChild(0).gameObject, TrackList.transform);
                }
                newland.transform.position = new Vector3(0, 0, (landCount + 1) * LAND_LENGTH + OFFSET);
                newland.GetComponent<Land>().SetDestoryable(true);
                landCount += 1;
            }
            // 背景街区
            if (landCount % STREET_COUNT == 0)
            {
                GameObject newstreet = Instantiate(streetTemplate, streetList.transform);
                newstreet.transform.position = new Vector3(0, 0, landCount * LAND_LENGTH + OFFSET);
                newstreet.GetComponent<Street>().SetDestoryable(true);
            }
        }
    }
}
