using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NMeCab;
using System.IO;
using UnityEngine.Networking;

public class MecabTest : MonoBehaviour
{

    MeCabParam param = new MeCabParam();

    void Update()
    {
    }

    void Start()
    {


        //端末へMecabの辞書ファイルをコピー
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "char.bin");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "dicrc");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "matrix.bin");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "unk.dic");
        CopyFile(Application.streamingAssetsPath + "/NMeCab", Application.persistentDataPath, "sys.dic");

        //Mecabの読み込み
        param.DicDir = Application.persistentDataPath;

        //テスト
        SetKeyWord("今日はとても良い天気ですね。");
    }

    /// 
    /// copy dictionary file.
    /// 
    /// void
    /// from path
    /// to path
    /// copy file name
    void CopyFile(string from, string to, string fileName)
    {
        string path = from + "/" + fileName;
        string toPath = to + "/" + fileName;

        //UNITY_EDITOR || UNITY_IPHONE
        //        FileInfo file = new FileInfo(path);
        //        file.CopyTo(toPath, true);

        //UNITY_ANDROID
        var request = UnityWebRequest.Get(path + to + fileName);
        WWW www = new WWW (path);
        while (!www.isDone) {
        }

        File.WriteAllBytes (toPath, www.bytes);

    }


    /// 
    /// Shows the key word.
    /// 
    /// The key word.
    /// Sentence.
    public void SetKeyWord(string sentence)
    {
        MeCabTagger tagger = MeCabTagger.Create(param);
        MeCabNode node = tagger.ParseToNode(sentence);

        while (node != null)
        {
            if (node.CharType > 0)
            {
                //名詞のみ抽出
                if (node.Feature.IndexOf("名詞") >= 0)
                {
                    Debug.Log("名詞：" + node.Surface);
                }
            }
            node = node.Next;
        }
    }
}