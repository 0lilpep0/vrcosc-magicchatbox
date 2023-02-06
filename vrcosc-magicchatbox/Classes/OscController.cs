﻿using vrcosc_magicchatbox.ViewModels;
using CoreOSC;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

namespace vrcosc_magicchatbox.Classes
{
    public class OscController
    {

        private ViewModel _VM;
        public OscController(ViewModel vm)
        {
            _VM = vm;
        }

        public UDPSender oscSender;

        public void SentOSCMessage()
        {
            try
            {
                if(_VM.OSCtoSent.Length > 0 || _VM.OSCtoSent.Length < 144)
                {
                        oscSender = new(_VM.OSCIP, _VM.OSCPortOut);
                        oscSender.Send(new OscMessage("/chatbox/typing", false ));
                        oscSender.Send(new OscMessage("/chatbox/input", _VM.OSCtoSent, true));
                }
                else
                {

                }
                
            }
            catch (System.Exception)
            {
            }

        }

        public int OSCmsgLenght(List<string> content, string add)
        {
            List<string> list = new List<string>(content);
            list.Add(add);

            byte[] utf8Bytes = Encoding.UTF8.GetBytes(String.Join(" | ", list));
            string x = Encoding.UTF8.GetString(utf8Bytes);

            return x.Length;
        }

        public void BuildOSC()
        {
            _VM.Char_Limit = "Hidden";
            _VM.Spotify_Opacity = "1";
            _VM.Window_Opacity = "1";
            _VM.Time_Opacity = "1";

            string x = null;
            var Complete_msg = "";
            List<string> Uncomplete = new List<string>();
            if(_VM.IntgrStatus == true && _VM.StatusList.Count() != 0)
            {
                if (_VM.PrefixIconStatus == true)
                {
                    x = "💬 " + _VM.StatusList.FirstOrDefault(item => item.IsActive == true)?.msg;
                }
                else
                {
                    x = _VM.StatusList.FirstOrDefault(item => item.IsActive == true)?.msg;
                }

                if (OSCmsgLenght(Uncomplete, x) < 144)
                {
                    Uncomplete.Add(x);
                }
                else
                {
                    _VM.Char_Limit = "Visible";
                    _VM.Window_Opacity = "0.5";
                }
            }
            if (_VM.IntgrScanWindowActivity == true)
            {
                if (_VM.IsVRRunning)
                {
                    x = "In VR";
                    if(OSCmsgLenght(Uncomplete, x) < 144)
                    {
                        Uncomplete.Add(x);
                    }
                    else
                    {
                        _VM.Char_Limit = "Visible";
                        _VM.Window_Opacity = "0.5";
                    }


                    
                }
                else
                {
                    x = "On desktop in '" + _VM.FocusedWindow + "'";
                    if (OSCmsgLenght(Uncomplete, x) < 144)
                    {
                        Uncomplete.Add(x);
                    }
                    else
                    {
                        _VM.Char_Limit = "Visible";
                        _VM.Window_Opacity = "0.5";
                    }             
                }
            }
            if (_VM.IntgrScanWindowTime == true & _VM.OnlyShowTimeVR == true & _VM.IsVRRunning == true | _VM.IntgrScanWindowTime == true & _VM.OnlyShowTimeVR == false)
            {
                if(_VM.PrefixTime == true)
                {
                    x = "My time: ";
                }
                else
                {
                    x = "";
                }
                x = x + _VM.CurrentTime;


                if (OSCmsgLenght(Uncomplete, x) < 144)
                {
                    Uncomplete.Add(x);
                }
                else
                {
                    _VM.Char_Limit = "Visible";
                    _VM.Time_Opacity = "0.5";
                }


            }
            if (_VM.IntgrScanSpotify == true)
            {
                if (_VM.SpotifyActive == true)
                {
                    if (_VM.SpotifyPaused)
                    {
                        x = "Music paused";
                        if(OSCmsgLenght(Uncomplete, x) < 144)
                        {
                            Uncomplete.Add(x);
                        }
                        else
                        {
                            _VM.Char_Limit = "Visible";
                            _VM.Spotify_Opacity = "0.5";
                        }
                    }

                    else
                    {                    
                        if (_VM.PrefixIconMusic == true)
                        {
                            x = "🎵 '" + _VM.PlayingSongTitle + "'";
                        }
                        else
                        {
                            x = "Listening to '" + _VM.PlayingSongTitle + "'";
                        }

                        if (OSCmsgLenght(Uncomplete, x) < 144)
                        {
                            Uncomplete.Add(x);
                        }
                        else
                        {
                            _VM.Char_Limit = "Visible";
                            _VM.Spotify_Opacity = "0.5";
                        }
                        
                    
                    }
                }
            }
            if (Uncomplete.Count > 0)
            {

                Complete_msg = String.Join(" ┆ ", Uncomplete);
                if (Complete_msg.Length > 144)
                {
                    _VM.OSCtoSent = "";
                    _VM.OSCmsg_count = Complete_msg.Length;
                    _VM.OSCmsg_countUI = "MAX/144";
                }
                else
                {
                    _VM.OSCtoSent = Complete_msg;
                    _VM.OSCmsg_count = _VM.OSCtoSent.Length;
                    _VM.OSCmsg_countUI = _VM.OSCtoSent.Length + "/144";
                }
            }
            else 
            {
                _VM.OSCmsg_count = _VM.OSCtoSent.Length;
                _VM.OSCmsg_countUI = _VM.OSCtoSent.Length + "/144";
                _VM.OSCtoSent = ""; 
            }


        }
    }
}
