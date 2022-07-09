using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using NMeCab;
using UnityEngine.Networking;
using System.IO;


public static class MecabExtend
{
    public static IEnumerable<MeCabNode> ToEnumerable(this MeCabNode node)
    {
        while (node != null)
        {
            if (node.CharType > 0)
            {
                yield return node;
            }
            node = node.Next;
        }
    }
}


public class NMeCabTest : MonoBehaviour
{
    
    public List<string> Words = new List<string>();
    public GameObject txtcre;
    public TextAsset sent ;

    // Start is called before the first frame update
    void Start()
    {

        //端末へMecabの辞書ファイルをコピー
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "char.bin");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "dicrc");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "matrix.bin");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "unk.dic");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "sys.dic");

        

        string sentence = sent.text;

        MeCabParam param = new MeCabParam();
        //param.DicDir = @"Assets/NMeCab/dic/ipadic";

        //Mecabの読み込み
        param.DicDir = Application.persistentDataPath;



        MeCabTagger t = MeCabTagger.Create(param);
        MeCabNode node = t.ParseToNode(sentence);
        var wakati = string.Join(",", node.ToEnumerable().Select(n => n.Surface).ToArray());


        while (node != null)
        {
            if (node.CharType > 0)
            {
                //Debug.Log(node.Surface);
                Words.Add(node.Surface);
            }
            node = node.Next;
        }
        
        txtcre.GetComponent<TextCreate>().LineUp();
    }


    void CopyFile(string from, string to, string fileName)
    {
        string path = from + "/" + fileName;
        string toPath = to + "/" + fileName;

        //UNITY_EDITOR || UNITY_IPHONE
        //        FileInfo file = new FileInfo(path);
        //        file.CopyTo(toPath, true);

        //UNITY_ANDROID
        var request = UnityWebRequest.Get(path + to + fileName);
        WWW www = new WWW(path);
        while (!www.isDone)
        {
        }

        File.WriteAllBytes(toPath, www.bytes);

    }

}
