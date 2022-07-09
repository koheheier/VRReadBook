using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Text;
using UnityEngine.UI;
using System;

public class PythonGo : MonoBehaviour
{
    //pythonの場所
    private string pyExePath = @"C:\Users\81909\AppData\Local\Programs\Python\Python37\python.exe";
    

    //実行したいスクリプトがある場所
    private string pyCodePath = @"C:\Users\81909\Desktop\MeCab\MeCabPython\MeCab_unity.py";

    private string txtOtameshi = @"C:\Users\81909\Desktop\MeCab\MeCab_text\otameshimemo.txt";


    
    
    public List<string> arrtext = new List<string>();
    //public TextCreate txtcre;
    public GameObject txtcre;
    

    // Start is called before the first frame update
    private void Start()
    {
        StreamReader sr = new StreamReader(txtOtameshi, Encoding.GetEncoding("shift_jis"));
        string texter = sr.ReadToEnd();
        sr.Close();

        

        //print(texter);

        
        //外部プロセス設定
        ProcessStartInfo proStaInfo = new ProcessStartInfo()
        {

            //実行するpythonファイル
            FileName = pyExePath,

            //WorkingDirectory = @cd,

            //シェルを使うかどうか
            UseShellExecute = false,

            //ウィンドゥを開くかどうか
            CreateNoWindow = true,

            //テキスト出力をStandardOutputストリームに書き込むかどうか
            RedirectStandardOutput = true,

            //実行スクリプト 引数（複数可能）
            //この引数は、実際のテキストである事を想定する。
            Arguments = pyCodePath + " " + texter,

            //エンコード
            StandardOutputEncoding = Encoding.GetEncoding("shift_jis") //←追加

        };


        //外部プロセス開始
        Process process = Process.Start(proStaInfo);

        //ストリームからの出力を得る
        StreamReader streamReader = process.StandardOutput;



        string str = streamReader.ReadLine();

        //外部プロセス終了
        process.WaitForExit();
        process.Close();
        //print(str);

        
        //配列になる前の文字列の両端を消す

        str = str.Trim('[', ']');
        //配列を区切る。
        string[] arr = str.Split(',');
        //配列の要素毎の両端を消す
        arr[0] = arr[0].Trim('\'', '\'');
        //print(arr[0]);
        for (int i = 1; i < arr.Length; i++)
        {
            arr[i] = arr[i].Trim(' ', '\'', '\'');
            //print(arr[i]);
        }


        //動詞、助動詞、固有名詞時対応込みの処理
        
        for (int i = 0; i < arr.Length; i++)
        {
            //iに加算することで、文中の単語だけを控えよう
            if (arr[i] != "EOS")
            {

                if (arr[i + 3].Contains("動詞") && !arr[i + 3].Contains("形") && !arr[i + 3].Contains("語幹") && !arr[i + 3].Contains("感"))
                {
                    //動詞、助動詞の時の配列調整、形容動詞は除外
                    arrtext.Add(arr[i]);
                    //print(arrtext[arrtext.Length - 1]);
                    
                    i += 5;

                }
                else if (arr[i + 3].Contains("形容詞")) {

                    //形容詞の時に+5
                    arrtext.Add(arr[i]);
                    
                    i += 5;
                }
                
                else
                {
                    //その他の単語は普通に格納。
                    arrtext.Add(arr[i]);
                    
                    //print(arrtext[arrtext.Length - 1]);
                    i += 3;
                }
            }
            else
            {
                //出力結果には必ず「EOS」が入っているその時は何もせず終わり。
            }
            
        }
        //ここでテキストをUI表示
        txtcre.GetComponent<TextCreate>().LineUp();
    }

    













}
