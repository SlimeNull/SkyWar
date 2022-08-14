using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SkyWar.Properties;

namespace SkyWar
{
    internal static class GameSound
    {
        static WaveOut bgout = new WaveOut();
        static WaveOut efout = new WaveOut();
        public static void PlayBackground(Stream stream)
        {
            
        }
    }
}
