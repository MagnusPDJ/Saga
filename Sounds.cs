﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Runtime.InteropServices;

namespace Saga
{
    internal class Sounds {

        //Klump af kode som gør det muligt at justerer system lyden med Get/Set metoden.
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IAudioEndpointVolume
        {
            int _0(); int _1(); int _2(); int _3();
            int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
            int _5();
            int GetMasterVolumeLevelScalar(out float pfLevel);
            int _7(); int _8(); int _9(); int _10(); int _11(); int _12();
        }

        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDevice
        {
            int Activate(ref System.Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
        }

        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDeviceEnumerator
        {
            int _0();
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
        }

        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class MMDeviceEnumeratorComObject { }

        public class Audio
        {
            private static readonly IAudioEndpointVolume _MMVolume;

            static Audio() {
                var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
                enumerator.GetDefaultAudioEndpoint(0, 1, out IMMDevice dev);
                var aevGuid = typeof(IAudioEndpointVolume).GUID;
                dev.Activate(ref aevGuid, 1, 0, out _MMVolume);
            }

            public static int Volume
            {
                get
                {
                    _MMVolume.GetMasterVolumeLevelScalar(out float level);
                    return (int)(level * 100);
                }
                set
                {
                    _MMVolume.SetMasterVolumeLevelScalar((float)value / 100, default);
                }
            }
        }
        //Slut klump

        //Sætter System lyd volumen, bruges under indstillinger under program.
        public static void SetVolume(int volume) {
            Audio.Volume = volume;
        }
        //Returnere det indstillede lydniveau.
        public static int GetVolume() {
            return Audio.Volume;
        }

        //Genere et objekt som kan spille en lyd.
        public static SoundPlayer soundTypeWriter = new SoundPlayer("sounds/typewriter.wav");
        public static SoundPlayer soundMainMenu = new SoundPlayer("sounds/mainmenu.wav");
        public static SoundPlayer soundKamp = new SoundPlayer("sounds/kamp.wav");
        public static SoundPlayer soundWin = new SoundPlayer("sounds/win.wav");
        public static SoundPlayer soundGameOver = new SoundPlayer("sounds/gameover.wav");
        public static SoundPlayer soundShop = new SoundPlayer("sounds/shop.wav");

    }
}
