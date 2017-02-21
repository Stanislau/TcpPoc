using System.IO;
using Android.Media;

namespace TcpPoc.Droid
{
    public class NetworkDataSource : MediaDataSource
    {
        private readonly System.IO.Stream _stream;

        public NetworkDataSource(System.IO.Stream stream)
        {
            _stream = stream;
        }

        public override int ReadAt(long position, byte[] buffer, int offset, int size)
        {
            _stream.Seek(0, SeekOrigin.Begin);
            return _stream.Read(buffer, offset, size);
        }

        public override void Close()
        {
            
        }

        public override long Size => 8059904;
    }
}