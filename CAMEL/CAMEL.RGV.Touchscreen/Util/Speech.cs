using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;

namespace CAMEL.RGV.Touchscreen.Util
{
    public static class Speech
    {
        private static SpeechSynthesizer Speechs;
        private static string ValueAlarmStr;
        private static string ValueConnect;
        private static bool IsRun=true;
        
        public static  void Voice(string content=null)
        {
            if (Speechs == null)
            {
                Speechs = new SpeechSynthesizer();
                Speechs.SetOutputToDefaultAudioDevice();
                Speechs.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            }
            Thread thread = new Thread(o =>
              {
                  Speak2(content);           
                  
                  if (IsRun)
                  {
                      while (true)
                      {
                          Speak1(Current.RGV.AlarmStr);                      Speak2(Current.RGV.IsConnected?"通讯中":"未连接");
                          IsRun = false;
                      }                  
                  }           
              });
            thread.Start();
            
        }

        private static  void Speak1(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                if (ValueAlarmStr != content)
                {
                    Prompt prompt = new Prompt(content);
                    Speechs.Speak(prompt);
                }
                Thread.Sleep(200);
            }
            ValueAlarmStr = content;
        }
        private static void Speak2(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                if (ValueConnect != content)
                {
                    Prompt prompt = new Prompt(content);
                    Speechs.Speak(prompt);
                }
                Thread.Sleep(200);
            }
            ValueConnect = content;
        }

    }
}
