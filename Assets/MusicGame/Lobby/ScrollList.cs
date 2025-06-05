using UnityEngine;
using UnityEngine.UI;

public class ScrollList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float height = 0;
        height += gameObject.GetComponent<VerticalLayoutGroup>().padding.top;
        height += gameObject.GetComponent<VerticalLayoutGroup>().padding.bottom;
        for(int i = 0; i < gameObject.transform.childCount; i++){
            height += gameObject.GetComponent<VerticalLayoutGroup>().spacing;
            height += gameObject.transform.GetChild(i).GetComponent<RectTransform>().rect.height;
        }
        height -= gameObject.GetComponent<VerticalLayoutGroup>().spacing;
        Rect rec = gameObject.GetComponent<RectTransform>().rect;
        gameObject.GetComponent<RectTransform>().rect.Set(
            rec.x,
            rec.y,
            rec.width,
            height
        );
    }
}
