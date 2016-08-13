using System.Collections.Generic;
using System.IO;

namespace Nana.Framework.Utility
{
    public class MemoryStreamStackHelper
    {
        private Stack<MemoryStream> _streams = null;

        public MemoryStreamStackHelper()
            : this(100)
        {

        }

        public MemoryStreamStackHelper(int capacity)
        {
            _streams = new Stack<MemoryStream>(capacity);
        }

        public MemoryStream GetMemoryStream()
        {
            MemoryStream stream = null;
            if (_streams.Count > 0)
            {
                lock (_streams)
                {
                    if (_streams.Count > 0)
                    {
                        stream = (MemoryStream)_streams.Pop();
                    }
                }
            }
            if (stream == null)
            {
                stream = new MemoryStream(0x800);
            }
            return stream;
        }


        public void ReleaseMemoryStream(MemoryStream stream)
        {
            if (stream == null)
            {
                return;
            }
            stream.Position = 0L;
            stream.SetLength(0L);
            lock (_streams)
            {
                _streams.Push(stream);
            }
        }

        ~MemoryStreamStackHelper()
        {
            foreach (MemoryStream memory in _streams)
            {
                memory.Dispose();
            }
            _streams.Clear();
            _streams = null;
        }
    }

}
