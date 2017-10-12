using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    internal class RecordHolder
    {
        internal ClusteredFile mFile;
        private readonly UInt32 mRecordNo;
        private WeakReference mRecordWeakRef;
        private byte[] mReadBuffer;
        private byte[] mWriteBuffer;
        internal System.Threading.EventWaitHandle RecordFinalized;

        private object mHold;

        public RecordHolder(ClusteredFile file, UInt32 RecordNo, Record record)
        {
            RecordFinalized = new System.Threading.EventWaitHandle(false, 
                System.Threading.EventResetMode.ManualReset);
#if DUMP_FINALIZE
            System.Diagnostics.Trace.WriteLine(file.mOriginalFile + " New RecordHolder for #" + RecordNo);
            System.Diagnostics.Debug.Assert(record != null);
#endif

            mFile = file;
            mRecordNo = RecordNo;
            MakeWeakRef(record);
            mHold = record;
        }

        public UInt32 RecordNo
        {
            get { return mRecordNo; }
        }

        public bool Hold
        {
            get { return mHold != null; }
            set { mHold = (value ? Record : null); }
        }

        private void MakeWeakRef(Record record)
        {
            mRecordWeakRef = new WeakReference(record, false);
        }

        public Record Record
        {
            get
            {
                if (mRecordWeakRef == null) return null;
                var result = mRecordWeakRef.Target as Record;
                return result;
            }
            set
            {
                // We could be tempted to write
                // mRecordWeakRef.Target=value 
                // but this is not advisable as this property
                // could be call from a GC destructor.
                MakeWeakRef(value);
            }
        }
     
        internal void OnRecordFinalizing(Record record)
        {
            try
            {
                // first we write the buffer 
                mFile.InternalFillWriteBuffer(this, record);
            }
            finally
            {
                // This is one of the only place where we use Threading mechanism.
                // The mainthread might be waiting for it
                RecordFinalized.Set();
            }
        }

        internal byte[] GetCurrentBuffer(bool readIfNeeded)
        {
            if (mWriteBuffer!=null) return mWriteBuffer;
            if (mReadBuffer!=null) return mReadBuffer;

            if (readIfNeeded)
            {
                mReadBuffer = new byte[mFile.mRecordWidth];
                mFile.InternalReadRecordFromDisk(mRecordNo, mReadBuffer);
                return mReadBuffer;
            }
            else return null;
        }

        internal bool IsModified()
        {
            return (mWriteBuffer != null);
        }

        internal void SetNewBuffer(Byte[] newBuffer)
        {
            bool modified = false;
            if (mReadBuffer == null) modified = true;
            else
            {
                int len = mFile.mRecordWidth;
                for (int i = 0; i < len; i++)
                {
                    if (mReadBuffer[i] != newBuffer[i])
                    {
                        modified = true;
                        break;
                    }
                }
            }
            if (modified)
            {
                var currentBuffer = (mWriteBuffer != null ? mWriteBuffer : mReadBuffer);
                // we could clear the readBuffer here to save memory
                mFile.OnWriteBufferModified(mRecordNo, currentBuffer, newBuffer);
                mWriteBuffer = newBuffer;
            }

        }

        internal void Save()
        {
            if (mWriteBuffer != null)
            {
                mFile.InternalSaveRecordToDisk(mRecordNo, mWriteBuffer);
                mWriteBuffer = null;
            }
        }

        internal void SaveChanges(Record record)
        {
            mFile.InternalFillWriteBuffer(this, record);
        }
    }
}
