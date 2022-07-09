using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Butukari : MonoBehaviour
{

    public RectTransform Rec;
    public Canvas canv;
    public Text text;

    private Vector3 GetWorldPositionFromRectPosition()
    {
        //UI座標からスクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canv.worldCamera, Rec.position);

        //ワールド座標
        Vector3 result = Vector3.zero;

        //スクリーン座標→ワールド座標に変換
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Rec, screenPos, canv.worldCamera, out result);

        return result;
    }

    private void Update()
    {


        
    }

}


