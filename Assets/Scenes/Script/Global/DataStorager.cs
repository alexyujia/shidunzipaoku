using UnityEngine;
using static DataManager;
public class DataStorager : MonoBehaviour
{
  // public GameObject dunzi;
  public static Item coin;

  public static Item maxLife;

  public static DunziSettings settings;
  private void Awake() {
    DontDestroyOnLoad(gameObject);
  }
  private void Start(){
    settings = InitSettings();
    coin = InitItem("coin",0);
    maxLife = InitItem("life",1);
  }

  private Item InitItem(string item_name,int default_count = 0){
    if(IsDataed(item_name)){
      return Load<Item>(item_name);
    } else {
      return new (){
        name = item_name,
        count = default_count,
      };
    }
  }

  private DunziSettings InitSettings(){
    if(IsDataed("settings")){
      return Load<DunziSettings>("settings");
    } else {
      return new (){
        SoundVolume = 1f,
        MusicVolume = 1f,
        hasMotionBlur = true,
        CustomMaxLife = maxLife.count,
      };
    }
  }
  // private void Update(){
  //   if(!dunzi.GetComponent<Move>().isAlive()){

  //   }
  // }

  public static void SaveStatus(){
    Save("coin", coin);
  }

  public static void SaveMaxLife(){
    Save("life", maxLife);
  }

  public static void SaveSettings(){
    Save("settings", settings);
  }
}
