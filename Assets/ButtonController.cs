using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Services.Assistant.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Connection;

public class ButtonController : MonoBehaviour {
    [SerializeField]
    private SpeechToText m_SpeechToText;

    string cre_id  = "2edf7f72-225b-4692-832b-abb07870ac18";                  // 資格情報より
    string cre_pw  = "ppa81Jdk6PTx";                                          // 資格情報より
    string cre_url = "https://stream.watsonplatform.net/speech-to-text/api";  // 資格情報より

    IEnumerator Start(){
        Credentials credentials = new Credentials(cre_id, cre_pw, cre_url);
        m_SpeechToText = new SpeechToText(credentials);
        m_SpeechToText.Keywords = new string[] { "ibm" };
        m_SpeechToText.KeywordsThreshold = 0.1f;

        // 音声をマイクから 3 秒間取得する
        Debug.Log("Start record"); //集音開始
        Debug.Log("Start record"); //集音開始
        Debug.Log("Start record"); //集音開始

        var audioSource  = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = false;
        audioSource.spatialBlend = 0.0f;
        yield return new WaitForSeconds(5f);
        Microphone.End(null); //集音終了
        Debug.Log("Finish record");

        Debug.Log("Playing");
        // ためしに録音内容を再生してみる
        audioSource.Play();
        Debug.Log("Play Finished");

        // SpeechToText を日本語指定して、録音音声をテキストに変換
        m_SpeechToText.RecognizeModel = "ja-JP_BroadbandModel";
        m_SpeechToText.Recognize(HandleOnRecognize, OnFail, audioSource.clip);
    }

    void Update(){
    //    Button button = GameObject.Find("Button").GetComponent<Button>();
    //    btn1.onClick.AddListener(InsertWordsIntoDatabase);
    }

    void HandleOnRecognize(SpeechRecognitionEvent result, Dictionary<string, object> customData)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = alt.transcript;
                    Debug.Log(string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));
                }
            }
        }
    }

   private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
   {
       Debug.Log("SampleSpeechToText.OnFail() Error received: " + error.ToString());
   }
}
