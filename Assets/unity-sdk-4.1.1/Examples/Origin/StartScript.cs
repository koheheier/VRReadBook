using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class StartScript : MonoBehaviour
{
    

    //ここでは、VR画面に移る前にやる作業してもらう場所
    //スマホ内のテキスト選択、背景自動変化の有無を選択。
    //あと関係ないが、スマホのジャイロ機能をリセットする機能を追加したい。


    

    public void OnClick() {
        SceneManager.LoadScene("main");
    }

    public void OnClick2() {
        
    }

}
