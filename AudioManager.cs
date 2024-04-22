using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Runtime.InteropServices;
using NAudio.Wave;
using System.Configuration;

namespace Saga
{
    public class AudioManager {

        //Aflæser pc'ens lydenhed.
        public WaveOutEvent outputDevice;
        public WaveFileReader waveFileReader;

        //Metode til set/get lydniveau.
        public float Volume
        {
            get { return outputDevice.Volume; }
            set { outputDevice.Volume = value; }
        }

        //Konstruktor til ny lydkontroller.
        public AudioManager(string filepath) {
            outputDevice = new WaveOutEvent();
            waveFileReader = new WaveFileReader(filepath);
            outputDevice.Init(waveFileReader);
        }

        //Metode til at afspille lydfil.
        public void Play() {
            waveFileReader.Position = 0;
            outputDevice.Play();
        }

        public void Stop() {
            if (outputDevice != null) {

            }
            outputDevice.Stop();
            waveFileReader.Position = 0;
            outputDevice.Init(waveFileReader);
        }

        //Genere et objekt som kan spille en lyd.
        public static AudioManager soundTaunt = new AudioManager("sounds/taunt.wav");
        public static AudioManager soundTroldmandsKamp = new AudioManager("sounds/troldmandskamp.wav");
        public static AudioManager soundLaugh = new AudioManager("sounds/laugh.wav");
        public static AudioManager soundTypeWriter = new AudioManager("sounds/typewriter.wav");
        public static AudioManager soundMainMenu = new AudioManager("sounds/mainmenu.wav");
        public static AudioManager soundKamp = new AudioManager("sounds/kamp.wav");
        public static AudioManager soundWin = new AudioManager("sounds/win.wav");
        public static AudioManager soundGameOver = new AudioManager("sounds/gameover.wav");
        public static AudioManager soundShop = new AudioManager("sounds/shop.wav");
        public static AudioManager soundLvlUp = new AudioManager("sounds/levelup.wav");

        public static void Play(string lyd) {
            switch (lyd) {
                case "mainmenu":
                    var reader0 = new WaveFileReader("sounds/mainmenu.wav");
                    var waveOut0 = new WaveOutEvent();
                    waveOut0.Init(reader0);
                    waveOut0.Play();
                    break;
                case "type":
                    var reader1 = new WaveFileReader("sounds/typewriter.wav");
                    var waveOut1 = new WaveOutEvent();
                    waveOut1.Init(reader1);
                    waveOut1.Play();
                    break;
            }
        }

    }

    

}
