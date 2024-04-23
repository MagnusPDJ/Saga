using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Saga
{
    public class AudioManager {

        //Aflæser pc'ens lydenhed.
        public WaveOutEvent outputDevice;
        public WaveStream ws;

        //Metode til set/get lydniveau.
        public float Volume
        {
            get { return outputDevice.Volume; }
            set { outputDevice.Volume = value; }
        }

        //Konstruktor til ny lydkontroller.
        public AudioManager(UnmanagedMemoryStream soundFile) {
            MemoryStream ms = new MemoryStream(StreamToBytes(soundFile));
            ws = new WaveFileReader(ms);
            outputDevice = new WaveOutEvent();
            outputDevice.Init(ws);
        }

        //Metode til at afspille lydfil.
        public void Play() {
            ws.Position = 0;
            outputDevice.Play();
        }

        public void Stop() {
            if (outputDevice != null) {

            }
            outputDevice.Stop();
            ws.Position = 0;
            outputDevice.Init(ws);
        }

        //Genere et objekt som kan spille en lyd.
        public static AudioManager soundTaunt = new AudioManager(Properties.Resources.taunt);
        public static AudioManager soundTroldmandsKamp = new AudioManager(Properties.Resources.troldmandskamp);
        public static AudioManager soundLaugh = new AudioManager(Properties.Resources.laugh);
        public static AudioManager soundTypeWriter = new AudioManager(Properties.Resources.typewriter);
        public static AudioManager soundMainMenu = new AudioManager(Properties.Resources.mainmenu);
        public static AudioManager soundKamp = new AudioManager(Properties.Resources.kamp);
        public static AudioManager soundWin = new AudioManager(Properties.Resources.win);
        public static AudioManager soundGameOver = new AudioManager(Properties.Resources.gameover);
        public static AudioManager soundShop = new AudioManager(Properties.Resources.shop);
        public static AudioManager soundLvlUp = new AudioManager(Properties.Resources.levelup);

        public static void Play(string lyd) {
            switch (lyd) {
                case "mainmenu":
                    var reader0 = new WaveFileReader(Properties.Resources.mainmenu.ToString());
                    var waveOut0 = new WaveOutEvent();
                    waveOut0.Init(reader0);
                    waveOut0.Play();
                    break;
                case "type":
                    var reader1 = new WaveFileReader(Properties.Resources.typewriter.ToString());
                    var waveOut1 = new WaveOutEvent();
                    waveOut1.Init(reader1);
                    waveOut1.Play();
                    break;
            }
        }

        public static byte[] StreamToBytes(System.IO.Stream stream) {
            long originalPosition = 0;

            if (stream.CanSeek) {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0) {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length) {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1) {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead) {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally {
                if (stream.CanSeek) {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
