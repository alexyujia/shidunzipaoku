using UnityEngine;

public class GlobalTargetManager : MonoBehaviour
{
  // public GameObject dunzi;
    public static Camera camera;
    public static ParticleSystem boom;
    public static GameObject dunzi;
    public static GameObject real_dunzi;

    public Camera precamera;
    public ParticleSystem preboom;
    public GameObject predunzi;
    public GameObject prereal_dunzi;

    private void Awake() {
        camera = precamera;
        boom = preboom;
        dunzi = predunzi;
        real_dunzi = prereal_dunzi;
    }
//   public Camera getCamera(){
//     return camera;
//   }

//   public ParticleSystem getBoom(){
//     return boom;
//   }
}
