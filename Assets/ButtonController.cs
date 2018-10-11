using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Services.Assistant.v1;
using IBM.Watson.DeveloperCloud.Utilities;

public class ButtonController : MonoBehaviour {

    string username = "9d6dcf33-630c-4a61-8b08-bd241c9c4872";
    string password = "GwjSbjnVVnJC";
    string url      = "https://stream.watsonplatform.net/text-to-speech/api";

    void Start()
    {
        Credentials credentials = new Credentials(username, password, url);
        Assistant _assistant = new Assistant(credentials);
        //GetModels();
    }

    // private void GetModels()
    // {
    //   if(!_speechToText.GetModels(HandleGetModels, OnFail)){
    //       Log.Debug("ExampleSpeechToText.GetModels()", "Failed to get models");
    //   }
    // }
    // 
    // private void HandleGetModels(ModelSet result, Dictionary<string, object> customData)
    // {
    //     Log.Debug("ExampleSpeechToText.HandleGetModels()", "Speech to Text - Get models response: {0}", customData["json"].ToString());
    // }

}
