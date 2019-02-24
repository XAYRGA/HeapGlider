using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XAYRGA.HeapGlider
{
    class ProcessMemoryStream : Stream
    {
        private Stream BufferReader;
        private long InternalPosition = 0;
        private ProcessMemoryHandle Handle; 


        public ProcessMemoryStream(ProcessMemoryHandle ProcessHandle)
        {
            Handle = ProcessHandle;
        }

          public override bool CanRead
        {
            get
            {
                return Handle.ProcessRunning();
            }
        }

        public override bool CanSeek
        {
            get
            {
                return Handle.ProcessRunning();
            }
        }

        public override bool CanWrite
        {
            get
            {
                return Handle.ProcessRunning();
            }
        }

        public override long Length
        {
            get
            {
                return Handle.BaseProcess.WorkingSet;
            }
        }

        public override long Position
        {
            get
            {
                return InternalPosition;
            }

            set
            {
                InternalPosition = value;
            }
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            bool error = false;
            if (offset > 0)
            {
                Seek(offset, SeekOrigin.Current);
            }


            var trn = Handle.ReadMemory((int)this.Position, count, out read, out error);
            Console.WriteLine("{0} -- {1} ", read, trn.Length);
            for (int i=0; i < trn.Length;i++)
            {
                buffer[i] = trn[i]; //?
            }

            Seek(count, SeekOrigin.Current); // Advance amount read. 
            return read;
        }


        public override long Seek(long offset, SeekOrigin origin)
        {

            var newOrigin = 0L;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newOrigin = 0L + offset;
                    break;
                case SeekOrigin.Current:
                    newOrigin = Position + offset;
                    break;
                case SeekOrigin.End:
                    newOrigin = Length - offset;
                    break;

            }
            Position = newOrigin;            
            return newOrigin;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException("Can't resize a process' memory.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
 
            bool error = false;
            if (offset > 0)
            {
                Seek(offset, SeekOrigin.Current);
            }

           var written = Handle.WriteMemory(buffer,0,count , out error);
        }


        public override void Flush()
        {

        }


    }
}
