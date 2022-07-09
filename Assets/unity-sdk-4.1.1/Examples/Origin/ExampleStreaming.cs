/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/
#pragma warning disable 0649

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using IBM.Cloud.SDK.Debug;
using Newtonsoft.Json;


// added this from the TONE ANALYZER . CS file
using IBM.Watson.ToneAnalyzer.V3;
using IBM.Watson.ToneAnalyzer.V3.Model;

namespace IBM.Watsson.Examples
{
    [System.Serializable]
    public class Rootobject
    {
        public Document_Tone document_tone { get; set; }
    }
    [System.Serializable]
    public class Document_Tone
    {
        public Tone[] tones { get; set; }
    }
    [System.Serializable]
    public class Tone
    {
        public float score { get; set; }
        public string tone_id { get; set; }
        public string tone_name { get; set; }
    }


    public class ExampleStreaming : MonoBehaviour
    {
        #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
        [Space(10)]
        [Tooltip("The IAM apikey.")]
        [SerializeField]
        private string iamApikey;
        [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/assistant/api\"")]
        [SerializeField]
        private string serviceUrl;
        [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD.")]
        [SerializeField]
        private string versionDate;
        #endregion

        private ToneAnalyzerService service;//この変数により、サービスの能力を使役する。
        public string stringToTestTone;//この文字列変数に文を入れる。ここに文章を入れると、その感情を読み取ってくれる。多分。

        public TextCreate txtcreate;
        private List<string> Watxt = new List<string>() { };

        private bool toneTested = false;//感情分析を複数回させないためのブレーキ

        //色変え用のobjだが、今回に適用できるかわからない
        //public Color backcol;

        public Text EmoWord;

        public Rootobject root;
        public int Page = 0;

        //背景
        public Material[] skys;
        

        public void Watsoner() {
            Watxt = txtcreate.Wsntxt;
        }

        


        public void Otameshi()
        {
            Debug.Log("Aho"+Watxt[Page]);
            stringToTestTone = Watxt[Page];
            //stringToTestTone.text = Watxt[Page];
            Debug.Log("入力したのは「" + stringToTestTone + "」だよ");
            LogSystem.InstallDefaultReactors();
            //CreateService呼び出し
            Runnable.Run(CreateService());
        }

        private IEnumerator CreateService()
        {
            //APIkeyがない時、「APIkeyを提供してね」というメッセージを送る
            if (string.IsNullOrEmpty(iamApikey))
            {
                throw new IBMException("Plesae provide IAM ApiKey for the service.");
            }

            //  Create credential and instantiate service
            IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

            //  Wait for tokendata
            while (!authenticator.CanAuthenticate())
                yield return null;

            service = new ToneAnalyzerService(versionDate, authenticator);

            if (!string.IsNullOrEmpty(serviceUrl))
            {
                service.SetServiceUrl(serviceUrl);
            }
            //Example呼び出し
            Runnable.Run(Examples());
        }

        private IEnumerator Examples()
        {
            //感情分析をする文を定義
            ToneInput toneInput = new ToneInput()
            {
                Text = stringToTestTone
            };

            List<string> tones = new List<string>()
            {
                "emotion",
                "language",
                "social"
            };
            //toneAnaのToneを発動している？
            service.Tone(callback: OnTone, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json");

            while (!toneTested)
            {
                yield return null;
            }
            
            
            
            Log.Debug("ExampleToneAnalyzerV3.Examples()", "Examples complete!");
        }




        private void OnTone(DetailedResponse<ToneAnalysis> response, IBMError error)
        {

            //root = JsonUtility.FromJson<Rootobject>(response.Response);
            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(response.Response);

            //感情ワードをEmmoに控える
            string Emmo = root.document_tone.tones[0].tone_id;
            float Emo_sco = root.document_tone.tones[0].score;
            Debug.Log("出力した感情は　"+Emmo+"　だぁぁ！！");

            
            



            if (error != null)
            {
                Log.Debug("ExampleToneAnalyzerV3.OnTone()", "Error: {0}: {1}", error.StatusCode, error.ErrorMessage);
            }
            else
            {
                //Log.Debug("ExampleToneAnalyzerV3.OnTone()", "{0}", response.Response);
                //感情スコアが0.5を超える場合に背景切り替え
                if (Emo_sco > 0.5 ) {
                    switch (Emmo)
                    {
                        case "sadness":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[0];
                            break;

                        case "anger":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[1];
                            break;

                        case "fear":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[2];
                            break;

                        case "disgust":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[3];
                            break;

                        case "joy":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[4];
                            break;

                        case "tentative":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[5];
                            break;

                        case "confident":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[6];
                            break;

                        case "analytical":
                            EmoWord.text = "I get emotion " + Emmo + " from the text";
                            RenderSettings.skybox = skys[7];
                            break;

                        default:
                            EmoWord.text = "I can't get the emotion.";
                            RenderSettings.skybox = skys[8];
                            break;
                    }
                }

            }
        }

        
        }
    }