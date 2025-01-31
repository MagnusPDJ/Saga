using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace Saga.assets
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

        //Metode til at stoppe lydfil
        public void Stop() {
            if (outputDevice != null) {
                outputDevice.Stop();
                ws.Position = 0;
                outputDevice.Init(ws);
            }
        }

        //Instantierer et objekt som kan spille en lyd.
        public static AudioManager soundTaunt = new AudioManager(Properties.Resources.taunt);
        public static AudioManager soundBossKamp = new AudioManager(Properties.Resources.troldmandskamp);
        public static AudioManager soundLaugh = new AudioManager(Properties.Resources.laugh);
        public static AudioManager soundTypeWriter = new AudioManager(Properties.Resources.typewriter);
        public static AudioManager soundMainMenu = new AudioManager(Properties.Resources.mainmenu);
        public static AudioManager soundKamp = new AudioManager(Properties.Resources.kamp);
        public static AudioManager soundWin = new AudioManager(Properties.Resources.win);
        public static AudioManager soundGameOver = new AudioManager(Properties.Resources.gameover);
        public static AudioManager soundShop = new AudioManager(Properties.Resources.shop);
        public static AudioManager soundLvlUp = new AudioManager(Properties.Resources.levelup);
        public static AudioManager soundCampFire = new AudioManager(Properties.Resources.campfire);
        public static AudioManager soundCampMusic = new AudioManager(Properties.Resources.campmusic);
        public static AudioManager soundMimic = new AudioManager(Properties.Resources.mimic);
        public static AudioManager soundDoorOpen = new AudioManager(Properties.Resources.dooropen);
        public static AudioManager soundDoorClose = new AudioManager(Properties.Resources.doorclose);
        public static AudioManager soundTreasure = new AudioManager(Properties.Resources.treasure);
        public static AudioManager soundRuneTrap = new AudioManager(Properties.Resources.runetrap);
        public static AudioManager soundDarts = new AudioManager(Properties.Resources.darts);
        public static AudioManager soundFootsteps = new AudioManager(Properties.Resources.footsteps);

        public static byte[] StreamToBytes(System.IO.Stream stream) {
            long originalPosition = 0;

            if (stream.CanSeek) {
                originalPosition = stream.Position;
                stream.Position = 0;
            } try {
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
            } finally {
                if (stream.CanSeek) {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
