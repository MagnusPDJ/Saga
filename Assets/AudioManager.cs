using System;
using System.IO;
using System.Collections.Generic;
using NAudio.Wave;
using System.Threading;

namespace Saga.Assets
{
    public class AudioManager {

        //Aflæser pc'ens lydenhed.
        private WaveOutEvent outputDevice;
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
            outputDevice ??= new WaveOutEvent();
            if (waveFile == null) {
                MemoryStream ms = new(SoundLibrary[sound]);
                waveFile = new WaveFileReader(ms);
                outputDevice.Init(waveFile);
            }
            outputDevice.Play();
        }

        //Metode til at stoppe lydfil
        public void Stop() {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            outputDevice = null;
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
