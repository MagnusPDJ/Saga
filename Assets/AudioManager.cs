using System;
using System.IO;
using System.Collections.Generic;
using NAudio.Wave;
using System.Threading;

namespace Saga.Assets
{
    public class AudioManager {

        //Aflæser pc'ens lydenhed.
        private readonly WaveOutEvent outputDevice;
        private WaveOutEvent outputDevice1;
        private WaveOutEvent outputDevice2;
        private WaveOutEvent outputDevice3;
        private WaveOutEvent outputDevice4;
        private WaveOutEvent outputDevice5;
        private WaveFileReader waveFile;

        //Metode til set/get lydniveau.
        public float Volume
        {
            get { return outputDevice.Volume; }
            set { outputDevice.Volume = value; }
        }

        //Lydbibliotek
        public Dictionary<string, byte[]> SoundLibrary { get; set; } =
            new Dictionary<string, byte[]> { 
                {"taunt", StreamToBytes(Properties.Resources.taunt)},
                {"troldmandskamp", StreamToBytes(Properties.Resources.troldmandskamp)},
                {"laugh", StreamToBytes(Properties.Resources.laugh)},
                {"typewriter", StreamToBytes(Properties.Resources.typewriter)},
                {"mainmenu", StreamToBytes(Properties.Resources.mainmenu)},
                {"kamp", StreamToBytes(Properties.Resources.kamp)},
                {"win", StreamToBytes(Properties.Resources.win)},
                {"gameover", StreamToBytes(Properties.Resources.gameover)},
                {"shop", StreamToBytes(Properties.Resources.shop)},
                {"levelup", StreamToBytes(Properties.Resources.levelup)},
                {"campfire", StreamToBytes(Properties.Resources.campfire)},
                {"campmusic", StreamToBytes(Properties.Resources.campmusic)},
                {"mimic", StreamToBytes(Properties.Resources.mimic)},
                {"dooropen", StreamToBytes(Properties.Resources.dooropen)},
                {"doorclose", StreamToBytes(Properties.Resources.doorclose)},
                {"treasure", StreamToBytes(Properties.Resources.treasure)},
                {"runetrap", StreamToBytes(Properties.Resources.runetrap)},
                {"darts", StreamToBytes(Properties.Resources.darts)},
                {"footsteps", StreamToBytes(Properties.Resources.footsteps)},
            };        

        //Konstruktor til ny lydkontroller.
        public AudioManager() {
            outputDevice = new WaveOutEvent();
        }

        //Metode til at afspille lydfil.
        public void Play(string sound) {
            if (outputDevice1 == null) {
                outputDevice1 = new WaveOutEvent();
                MemoryStream ms = new(SoundLibrary[sound]);
                waveFile = new WaveFileReader(ms);
                outputDevice1.Init(waveFile);            
                outputDevice1.Play();
            } else if (outputDevice2 == null) {
                outputDevice2 = new WaveOutEvent();
                MemoryStream ms = new(SoundLibrary[sound]);
                waveFile = new WaveFileReader(ms);
                outputDevice2.Init(waveFile);             
                outputDevice2.Play();
            } else if (outputDevice3 == null) {
                outputDevice3 = new WaveOutEvent();
                MemoryStream ms = new(SoundLibrary[sound]);
                waveFile = new WaveFileReader(ms);
                outputDevice3.Init(waveFile);               
                outputDevice3.Play();
            } else if (outputDevice4 == null) {
                outputDevice4 = new WaveOutEvent();
                MemoryStream ms = new(SoundLibrary[sound]);
                waveFile = new WaveFileReader(ms);
                outputDevice4.Init(waveFile);
                outputDevice4.Play();
            } else {
                outputDevice5 ??= new WaveOutEvent();
                MemoryStream ms = new(SoundLibrary[sound]);
                waveFile = new WaveFileReader(ms);
                outputDevice5.Init(waveFile);               
                outputDevice5.Play();
            }
        }

        //Metode til at stoppe lydfil
        public void Stop() {
            if (outputDevice1 != null) {
                outputDevice1?.Stop();
                outputDevice1?.Dispose();
                outputDevice1 = null;
            } 
            if (outputDevice2 != null) {
                outputDevice2?.Stop();
                outputDevice2?.Dispose();
                outputDevice2 = null;
            } 
            if (outputDevice3 != null) {
                outputDevice3?.Stop();
                outputDevice3?.Dispose();
                outputDevice3 = null;
            } 
            if (outputDevice4 != null) {
                outputDevice4?.Stop();
                outputDevice4?.Dispose();
                outputDevice4 = null;
            } if (outputDevice5 != null) {
                outputDevice5?.Stop();
                outputDevice5?.Dispose();
                outputDevice5 = null;
            }          
            waveFile?.Dispose();
            waveFile = null;
        }

        public static byte[] StreamToBytes(Stream stream) {
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
