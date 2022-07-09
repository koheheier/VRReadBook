using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IBM.Watsson.Examples;




    public class TextCreate : MonoBehaviour
    {
        //ここでは、テキストオブジェクトをUIに生成する練習を行う

        public GameObject canvas;//キャンバス
        public GameObject txtobj;
        public NMeCabTest NMC;
        RectTransform RecFor;
        List<List<GameObject>> Pager = new List<List<GameObject>>() { };
        int pgnum = 0;
        int pg = 0;
        //最初ページかどうか
        bool first = false;
        private int UInum = 0;
        public GameObject book = null;
        //Watson感情分析用文字列（1ページ分の文字列リスト）
        public List<string> Wsntxt = new List<string>() { };

        List<string> paperadd = new List<string>() { };

        //背景
        

        public ExampleStreaming Exam;


        private int Skynt = 0;


        //Pagerの大きい方とWsntxtのindexは同じなので、それを留意


        //テキストobjの原点
        Vector3 Sta = new Vector3(-230.0f, 130.0f, 0.0f);

       

        public void LineUp()
        {

            List<string> Booktxt = NMC.Words;

            List<GameObject> Worder = new List<GameObject>();

            // BookTextプレハブを元に、インスタンスを生成、
            for (int i = 0; i < Booktxt.Count; i++)
            {
                var honhon = txtobj.GetComponent<Text>();
                honhon.text = Booktxt[i];

                //y - i*27.0f
                //テキスト幅調整
                if (i == 0 && pgnum == 0)
                {
                    //最初
                    Sta = new Vector3(Sta.x + Booktxt[i].Length * 10.0f, Sta.y, 0.0f);
                }

                else if (first)
                {
                    //テキストを左上に更新
                    Sta = new Vector3(-250.0f + Booktxt[i].Length * 10.0f, 130.0f, 0.0f);
                    first = false;
                }

                else
                {
                    //ワード並べる時
                    //日本語バージョン
                    //Sta = new Vector3(Sta.x + Booktxt[i].Length * 10.0f + Booktxt[i - 1].Length * 10.0f + 2.0f, Sta.y, 0.0f);
                    //英語バージョン
                    Sta = new Vector3(Sta.x + Booktxt[i].Length * 6.0f + Booktxt[i - 1].Length * 5.0f + 10.0f, Sta.y, 0.0f);
                }

                //改行処理
                //日本語バージョン
                /*
                if (Sta.x + Booktxt[i].Length * 10.0f > 250.0f)
                {
                    Sta.y -= 27.0f;
                    Sta.x = -250.0f + Booktxt[i].Length * 10.0f;
                }
                */
                //英語バージョン
                if (Sta.x + Booktxt[i].Length * 5.0f > 250.0f)
                {
                    Sta.y -= 27.0f;
                    Sta.x = -250.0f + Booktxt[i].Length * 10.0f;
                }


                var obj = Instantiate(txtobj, Sta, Quaternion.identity);
                obj.transform.SetParent(canvas.transform, false);
                RecFor = obj.GetComponent<RectTransform>();
                //sizeDelta = new Vector2(width,height)

                //テキストの物理的幅調整
                //日本語バージョン
                //RecFor.sizeDelta = new Vector2(Booktxt[i].Length * 20.0f, 25.0f);
                //英語バージョン
                RecFor.sizeDelta = new Vector2(Booktxt[i].Length * 10.0f + 10.0f, 25.0f);
                obj.name = Booktxt[i];

                //配列格納
                Worder.Add(obj);
                paperadd.Add(Booktxt[i] + " ");
                //ページが満タンになったら、次のページになるように配置
                if (Sta.y < -150.0f || i == Booktxt.Count - 1)
                {

                    var Word = Worder.GetRange(pg, Worder.Count - pg);
                    Pager.Add(Word);

                    List<string> paper = new List<string>() { };
                    //paperaddとWorderのindexが同じなため
                    paper = paperadd.GetRange(pg, Worder.Count - pg);

                    //Watsonが読み込むためのページ毎の文字列控え
                    Wsntxt.Add(String.Join("", paper));

                    //原点更新
                    pg = Worder.Count;

                    //ページナンバー更新
                    pgnum++;

                    //Worderページの表示非表示操作
                    first = true;
                }


                //最初のページ以外を非アクティブ
                if (i == Booktxt.Count - 1)
                {
                    //ここでforeach
                    for (int j = 1; j < Pager.Count; j++)
                    {
                        for (int k = 0; k < Pager[j].Count; k++)
                        {
                            Pager[j][k].SetActive(false);
                        }
                    }
                }
            }
            //感情分析用の文章リスト格納をする関数起動
        Exam.Watsoner();
        Exam.Otameshi();
    }











        public void PageUp()
        {
            //ページ単位で、表示非表示をする

            UInum++;
            
            if (0 <= UInum && UInum < Pager.Count)
            {

                //ページ進め
                for (int i = 0; i < Pager[UInum].Count; i++)
                {
                    Pager[UInum][i].SetActive(true);
                }
                for (int i = 0; i < Pager[UInum - 1].Count; i++)
                {
                    Pager[UInum - 1][i].SetActive(false);
                }

            }
            else
            {

                //ページ超過
                for (int i = 0; i < Pager[UInum - 1].Count; i++)
                {
                    Pager[UInum - 1][i].SetActive(false);
                }
                UInum = 0;
                for (int i = 0; i < Pager[UInum].Count; i++)
                {
                    Pager[UInum][i].SetActive(true);
                }
            }
            
            Exam.Page = UInum;
            Exam.Otameshi();
            //感情分析トリガーのタイミング
        }

        public void PageDown()
        {

            UInum--;


            if (0 <= UInum && UInum < Pager.Count)
            {

                //ページ後退
                for (int i = 0; i < Pager[UInum].Count; i++)
                {
                    Pager[UInum][i].SetActive(true);
                }

                for (int i = 0; i < Pager[UInum + 1].Count; i++)
                {
                    Pager[UInum + 1][i].SetActive(false);
                }

            }
            else
            {

                for (int i = 0; i < Pager[UInum + 1].Count; i++)
                {
                    Pager[UInum + 1][i].SetActive(false);
                }
                UInum = Pager.Count - 1;
                for (int i = 0; i < Pager[UInum].Count; i++)
                {
                    Pager[UInum][i].SetActive(true);
                }
            }
            
        //感情分析トリガーのタイミング
            Exam.Page = UInum;
            Exam.Otameshi();
    }



        private void Update()
        {
            //ページ切り替え

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PageUp();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PageDown();
            }

        }

    }

