using System;
using AudioToolbox;

namespace TcpPoc.Apple
{
    public class WavStreamPlayer : IDisposable
    {
        bool outputStarted;

        AudioFileStream afs;
        OutputAudioQueue oaq;

        public event Action<OutputAudioQueue> OutputReady = delegate { };

        public WavStreamPlayer()
        {
            afs = new AudioFileStream(AudioFileType.WAVE);

            // event handlers, these are never triggered
            afs.PacketDecoded += OnPacketDecoded;
            afs.PropertyFound += OnPropertyFound;
        }

        public void Parse(byte[] bytes)
        {
            afs.ParseBytes(bytes, 0, bytes.Length, false);
        }

        // event handler - never executed
        void OnPropertyFound(object sender, PropertyFoundEventArgs e)
        {
            if (e.Property == AudioFileStreamProperty.ReadyToProducePackets)
            {
                oaq = new OutputAudioQueue(afs.StreamBasicDescription);
                oaq.BufferCompleted += OnBufferCompleted;
                OutputReady(oaq);
            }
        }

        // another event handler never executed
        void OnPacketDecoded(object sender, PacketReceivedEventArgs e)
        {
            IntPtr outBuffer;
            oaq.AllocateBuffer(e.Bytes, out outBuffer);
            AudioQueue.FillAudioData(outBuffer, 0, e.InputData, 0, e.Bytes);
            oaq.EnqueueBuffer(outBuffer, e.Bytes, e.PacketDescriptions);

            // start playing if not already
            if (!outputStarted)
            {
                var status = oaq.Start();
                if (status != AudioQueueStatus.Ok)
                    System.Diagnostics.Debug.WriteLine("Could not start audio queue");
                outputStarted = true;
            }
        }

        void OnBufferCompleted(object sender, BufferCompletedEventArgs e)
        {
            oaq.FreeBuffer(e.IntPtrBuffer);
        }

        public void Dispose()
        {
            oaq.Dispose();
            afs.Dispose();
        }
    }
}