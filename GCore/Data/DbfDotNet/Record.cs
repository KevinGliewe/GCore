using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{

    [Record(FieldMapping = FieldMapping.ExplicitColumnsOnly)]
    public class Record 
    {
        private Core.RecordHolder mmHolder;

        public Record()
        {
#if DUMP_FINALIZE
            System.Diagnostics.Trace.WriteLine("New " + this.GetType().Name);
#endif
        }

        internal Core.RecordHolder mHolder
        {
            get { return mmHolder; }
        }

        internal void SetHolderForNewRecord(Core.RecordHolder value)
        {
            System.Diagnostics.Debug.Assert(mmHolder==null,"mHolder can only be set once");
#if DUMP_FINALIZE
            System.Diagnostics.Trace.WriteLine(this.GetType().Name + " #" + value.mRecordNo);
#endif
            mmHolder = value;
        }

        public UInt32 RecordNo
        {
            get { return mHolder.RecordNo; }
        }

        ~Record()
        {
#if DUMP_FINALIZE
            System.Diagnostics.Trace.WriteLine("Entering Destructor " + this.GetType().Name + " #" + mHolder.mRecordNo);
#endif
            mHolder.OnRecordFinalizing(this);
        }

        public void SaveChanges()
        {
            mHolder.SaveChanges(this);
        }
    }
}
