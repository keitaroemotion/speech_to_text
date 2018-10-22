using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Services.Assistant.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Connection;

public class ButtonController : MonoBehaviour {
    [SerializeField]
    private SpeechToText m_SpeechToText;

    public AudioClip audio;
    public AudioSource audioSource;

    string cre_id        = "2edf7f72-225b-4692-832b-abb07870ac18";                  
    string cre_pw        = "ppa81Jdk6PTx";                                          
    string cre_url       = "https://stream.watsonplatform.net/speech-to-text/api";
    bool   isSoundPlayed = false;
    float MAXIMUM_LENGTH = 5.0f;

    void Awake(){
        this.audioSource = GetComponent<AudioSource>();
    }

    IEnumerator Start(){
        Debug.Log("Start");
        Credentials credentials          = new Credentials(cre_id, cre_pw, cre_url);
        m_SpeechToText                   = new SpeechToText(credentials);
        m_SpeechToText.Keywords          = new string[] { "ibm" };
        m_SpeechToText.KeywordsThreshold = 0.1f;

        yield return new WaitForSeconds(1f);

        Debug.Log("Start record");
        this.audioSource.Play();
        yield return new WaitForSeconds(0.1f);

        var audioSource  = GetComponent<AudioSource>();

        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = false;
        audioSource.spatialBlend = 0.0f;
        
        yield return new WaitForSeconds(MAXIMUM_LENGTH);

        Microphone.End(null);
        Debug.Log("Finish record");
    
        Debug.Log("Playing");
        audioSource.Play();
        Debug.Log("Play Finished");
        yield return new WaitForSeconds(1f);
    
        // SpeechToText を日本語指定して、録音音声をテキストに変換
        m_SpeechToText.RecognizeModel = "ja-JP_BroadbandModel";
        m_SpeechToText.Recognize(HandleOnRecognize, OnFail, audioSource.clip);

        yield return new WaitForSeconds(1f);
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
                    var resultText = string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence);
                    Debug.Log(resultText);
                    GameObject.Find("Result").GetComponent<Text>().text = resultText;
                }
            }
        }
    }

   private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
   {
       Debug.Log("SampleSpeechToText.OnFail() Error received: " + error.ToString());
   }
}
