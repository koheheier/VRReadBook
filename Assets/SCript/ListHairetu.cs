using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListHairetu : MonoBehaviour
{

    private List<string[]> str = new List<string[]>();
    private string[] str2 = new string[5];
    private string[] str3 = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++ ) {
            str2[i] = "馬鹿" + i;
            str3[i] = "あほ" + i;
        }
        string baka = string.Join("", str2);
        //print(baka);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
