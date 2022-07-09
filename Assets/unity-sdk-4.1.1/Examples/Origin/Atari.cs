using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watsson.Examples;


public class Atari : MonoBehaviour
{
    public TextCreate texcre;
    public Text txx;
    public ExampleStreaming Exam;
    public Material[] Searcher;
    private int sch = 0;    



    // Start is called before the first frame update
    void Start()
    {
     
    }

    void OnTriggerEnter(Collider gaobj)
    {
        Debug.Log(gaobj.gameObject.name); // ログを表示する
        txx.text = gaobj.gameObject.name;
        if (gaobj.tag == "Next") {

            texcre.PageUp();
        }
        else if (gaobj.tag == "Back") {
            texcre.PageDown();
        }
        else if (gaobj.tag == "Search") {
            Debug.Log("search!!");
            if (sch == 0) {
                sch = 1;
                //これまだ無理なので、次の機会に
                //gaobj.material = Searcher[1];
                
            }
        }
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
