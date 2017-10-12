using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    [Record(FieldMapping = FieldMapping.ExplicitColumnsOnly)]
    internal class NdxPage : Record
    {
        private List<NdxEntry> mEntries;
        internal NdxPage mParentPage;
        internal bool mIsModified;
        //internal int mNbChildEntries;

        public NdxPage()
        {
            EntriesClear();
        }

        internal NdxPage ParentPage
        {
            get
            {
                return mParentPage;
            }
            set
            {
                if (mParentPage == value) return;
                mParentPage = value;
            }
        }

        void EntriesClear()
        {
            if (mEntries == null) mEntries = new List<NdxEntry>();
            else mEntries.Clear();
#if DUMP_INSERTS
            if (mHolder == null) return;
            System.Diagnostics.Trace.WriteLine("Entries.Clear "
                + " in page #" + this.RecordNo);
            Dump("      ");
#endif
        }

        private void EntriesRemove(int pos)
        {
            mEntries.RemoveAt(pos);
        }


        private void EntriesRemoveRange(int pos, int count)
        {
            mEntries.RemoveRange(pos, count);
#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("Entries.RemoveRange @" + pos + " count:" + count
                + " in page #" + this.RecordNo);
            Dump("      ");
#endif
        }


        private void EntriesInsert(int pos, NdxEntry newEntry)
        {
#if DEBUG
            var ndxFile = (NdxFile)mHolder.mFile;
            int[] cmpArray = new int[ndxFile.mSortFieldsCount];
            int cmp;
            if (pos > 0)
            {
                NdxEntry previousEntry = GetEntry(pos - 1);
                cmp = newEntry.ChainCompare(cmpArray, ndxFile, previousEntry);
                if (cmp < 0)
                {
                    Dump("NdxPage.EntriesInsert:", "");
                    System.Diagnostics.Debug.Assert(false, "Node should be bigger than previous node.");
                }
            }
            if (pos < mEntries.Count)
            {
                NdxEntry nextEntry = GetEntry(pos);
                cmp = newEntry.ChainCompare(cmpArray, ndxFile, nextEntry);
                if (cmp > 0)
                {
                    Dump("NdxPage.EntriesInsert:", "");
                    System.Diagnostics.Debug.Assert(false, "Node should be less than previous node.");
                }
            }
#endif
            mEntries.Insert(pos, newEntry);
#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("Entries.Insert @" + pos + ":" + newEntry.Fields.ToString() + " #" + newEntry.DbfRecordNo 
                + " in page #" + this.RecordNo);
            Dump("      ");
#endif
        }
      
        internal void OnReadRecord(byte[] buffer)
        {
            var ndxFile = (NdxFile)mHolder.mFile;

#if DUMP_READ_RECORD
            System.Diagnostics.Trace.WriteLine("NdxPage.OnReadRecord Page#" + this.RecordNo);
            System.Diagnostics.Trace.WriteLine(Utils.HexDump(buffer));
#endif
            EntriesClear();
            int nbEntries = BitConverter.ToInt32(buffer, 0);
            int pos = 4;
            Byte[] fieldsBuffer = ndxFile.mSortFieldsReadBuffer;
            for (int i = 0; i < nbEntries; i++)
            {
                var ndxEntry = new NdxEntry();
                ndxEntry.mNdxNativeLowerPageNo = BitConverter.ToUInt32(buffer, pos);
                pos += 4;
                ndxEntry.DbfRecordNo = BitConverter.ToUInt32(buffer, pos);
                pos += 4;
                object fields = ndxFile.mSortFieldsConstructor.Invoke(null);
                Array.Copy(buffer, pos, fieldsBuffer, 0, fieldsBuffer.Length);
                ndxFile.mSortFieldsWriter.mQuickReadMethod(ndxFile, fieldsBuffer, fields);
                pos += fieldsBuffer.Length;
                ndxEntry.Fields = (SortFields)fields;
#if DUMP_READ_RECORD
                System.Diagnostics.Trace.WriteLine("  Entry " + i + " (Page #" + this.RecordNo + ") " + fields.ToString() + " " + ndxEntry.Fields.ToString() + " " + ndxEntry.DbfRecordNo);
#endif

                EntriesInsert(i, ndxEntry);
            }
        }

        internal void OnFillWriteBuffer(byte[] buffer)
        {
            var ndxFile = (NdxFile)mHolder.mFile;
            int nbEntries = EntriesCount;
            var bytes = BitConverter.GetBytes(nbEntries);
            bytes.CopyTo(buffer, 0);
            int pos = 4;
            // we are called by destructors in different threads
            // so we can't use a shared buffer
            Byte[] fieldsBuffer = new byte[ndxFile.mSortFieldsWriter.RecordWidth];
            for (int i = 0; i < nbEntries; i++)
            {
                var ndxEntry = GetEntry(i);
                bytes = BitConverter.GetBytes(ndxEntry.mNdxNativeLowerPageNo);
                bytes.CopyTo(buffer, pos);
                pos += 4;
                bytes = BitConverter.GetBytes(ndxEntry.DbfRecordNo);
                bytes.CopyTo(buffer, pos);
                pos += 4;

                ndxFile.mSortFieldsWriter.mQuickWriteMethod(ndxFile, fieldsBuffer, ndxEntry.Fields);
                Array.Copy(fieldsBuffer, 0, buffer, pos, fieldsBuffer.Length);
                pos += fieldsBuffer.Length;
            }
        }

        internal int EntriesCount
        {
            get
            {
                return mEntries.Count;
            }
        }

        internal NdxEntry GetEntry(int pos)
        {
            return mEntries[pos];
        }

        private void SetEntry(int pos, NdxEntry newEntry)
        {
#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("Setting entry #" + pos + ":" + newEntry.Fields.ToString() + " #" + newEntry.DbfRecordNo + " in page #" + this.RecordNo);
#endif
            mEntries[pos] = newEntry;
        }

        internal void LocateAndInsert(NdxEntry newEntry)
        {
            var ndxFile = (NdxFile)mHolder.mFile;
            int originalPos = FindInsertPos(newEntry, ndxFile);
            int pos = originalPos;

            if (pos == EntriesCount) pos--;

            bool hasLowerPage = (pos >= 0 && GetEntry(pos).LowerPageRecordNo != UInt32.MaxValue);

            if (hasLowerPage)
            {
                var ndxPage2 = ndxFile.InternalGetPage(this,
                    GetEntry(pos).LowerPageRecordNo,
                    /* returnNullIfNotInCache */ false) as NdxPage;
                ndxPage2.LocateAndInsert(newEntry);
            }
            else
            {
                InsertInThisPage(newEntry, originalPos);
            }
        }

        private void InsertInThisPage(NdxEntry newEntry, int pos)
        {

#if DUMP_INSERTS
                working++;
                System.Diagnostics.Trace.WriteLine("Inserting entry " + newEntry.Fields.ToString() + " #" + newEntry.DbfRecordNo + " in page #" + this.RecordNo);
#endif
            System.Diagnostics.Debug.Assert(this.RecordNo != UInt32.MaxValue, "RecordNo should be set.");
            var ndxFile = (NdxFile)mHolder.mFile;
            // find the
            EntriesInsert(pos, newEntry);

            if (pos == EntriesCount - 1) // we inserted in the last entry
            {
                PropagateLastEntryChanged(newEntry);
            }
            if (EntriesCount > ndxFile.mNdxHeader.NoOfKeysPerPage) SplitPage();
            mIsModified = true;
        }

        private void DeleteInThisPage(NdxEntry newEntry, int pos)
        {

#if DUMP_INSERTS
                working++;
                System.Diagnostics.Trace.WriteLine("Inserting entry " + newEntry.Fields.ToString() + " #" + newEntry.DbfRecordNo + " in page #" + this.RecordNo);
#endif
            System.Diagnostics.Debug.Assert(this.RecordNo != UInt32.MaxValue, "RecordNo should be set.");
            var ndxFile = (NdxFile)mHolder.mFile;
            // find the
            EntriesRemove(pos);

            if (EntriesCount == 0)
            {
                Utils.Nop();
            }
            else if (pos == EntriesCount) // we delete the last entry
            {
                var lastEntry = GetEntry(pos - 1);
                PropagateLastEntryChanged(lastEntry);
            }
            mIsModified = true;
        }

        private void PropagateLastEntryChanged(NdxEntry newEntry)
        {
            var ndxFile = (NdxFile)mHolder.mFile;
            var page = this;
            var parentPage = ParentPage;
            int pos;
            while (parentPage != null)
            {
                bool equal;
                parentPage.FindPos(newEntry, ndxFile, out equal, out pos);
                if (pos == parentPage.mEntries.Count)  pos--;
                var originalParentEntry = parentPage.GetEntry(pos);

                if (page.RecordNo != originalParentEntry.LowerPageRecordNo)
                {
                    ndxFile.Dump();
                    System.Diagnostics.Debug.Assert(
                        page.RecordNo == originalParentEntry.LowerPageRecordNo,
                        "invalid LowerPageRecordNo");
                }
                NdxEntry newParentEntry = new NdxEntry()
                {
                    DbfRecordNo = newEntry.DbfRecordNo,
                    LowerPageRecordNo = page.RecordNo,
                    Fields = newEntry.Fields.Clone()
                };

#if DUMP_INSERTS
                System.Diagnostics.Trace.WriteLine("Propagate LastEntry in page #" + parentPage.RecordNo + " entry " + pos);
                System.Diagnostics.Trace.WriteLine("before propagate");
                parentPage.Dump("      ");
#endif

                parentPage.SetEntry(pos, newParentEntry);

#if DUMP_INSERTS
                System.Diagnostics.Trace.WriteLine("after propagate");
                parentPage.Dump("      ");
#endif
                
                if (pos < parentPage.EntriesCount - 1) break;
                page = parentPage;
                parentPage = page.ParentPage;
            }
        }

        private int FindInsertPos(NdxEntry newEntry, NdxFile ndxFile)
        {
            int pos;
            bool equal;
            FindPos(newEntry, ndxFile, out equal, out pos);
            if (equal) throw new Exception("Duplicated entries in the Index file.");
            return pos;
        }

        private void FindPos(NdxEntry newEntry, NdxFile ndxFile, out bool equal, out int pos)
        {
            int min = 0;
            int max = EntriesCount - 1;
            int[] cmpArray = new int[ndxFile.mSortFieldsCount];
#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("Searching entry '" + newEntry.Fields.ToString() + " #" + newEntry.DbfRecordNo + "' position in page #" + this.RecordNo);
            Dump("      ");
#endif

            while (max >= min)
            {
                int mid = (min + max) / 2;
                NdxEntry other = GetEntry(mid);
                int cmp = newEntry.ChainCompare(cmpArray, ndxFile, other);
                if (cmp > 0) min = mid + 1;
                else if (cmp < 0) max = mid - 1;
                else
                {
                    equal = true;
                    pos = mid;
                    return;
                }
            }           
#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("   Result:" + min);
#endif
            equal = false;
            pos = min;
        }

        private void SplitPage()
        {

#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("Split page #" + this.RecordNo);
#endif
            var ndxFile = (NdxFile)mHolder.mFile;
            // split needed (on save)
            int mid = EntriesCount / 2;
            if (ParentPage == null)
            {
                ParentPage = (NdxPage)ndxFile.NewPage(null);

                var rootEntry = new NdxEntry();
                var lastEntry = GetEntry(EntriesCount - 1);
                rootEntry.DbfRecordNo = lastEntry.DbfRecordNo;
                rootEntry.Fields = lastEntry.Fields.Clone();
                rootEntry.LowerPageRecordNo = this.RecordNo;

                ParentPage.EntriesClear();
                ParentPage.EntriesInsert(0,rootEntry);
                ndxFile.mNdxHeader.StartingPageRecordNo = ParentPage.RecordNo;

                //ParentPage.mNbChildEntries = mNbChildEntries;
                ParentPage.mIsModified = true;
#if DUMP_INSERTS
                System.Diagnostics.Trace.WriteLine("  Create new parent page #" + mParentPage.RecordNo);
                mParentPage.Dump("      ");
#endif
            }

            var newPage = (NdxPage)ndxFile.NewPage(ParentPage);
            newPage.EntriesClear();
            newPage.mIsModified = true;
            newPage.ParentPage = ParentPage;

            for (int i = 0; i < mid; i++)
            {
                //mNbChildEntries = mNbChildEntries;
                newPage.EntriesInsert(i, GetEntry(i));
                var lowerPageRecordNo = GetEntry(i).LowerPageRecordNo;
                if (lowerPageRecordNo != UInt32.MaxValue)
                {
                    var ndxPage2 = ndxFile.InternalGetPage(
                        ParentPage,
                        lowerPageRecordNo, 
                        /* returnNullIfNotInCache */ true) as NdxPage;
                    if (ndxPage2 != null)
                    {
                        ndxPage2.ParentPage = newPage;
                    }
                }
            }
            EntriesRemoveRange(0, mid);

            var midEntry = newPage.GetEntry(mid - 1);
            var newMidEntry = new NdxEntry() {
                DbfRecordNo = midEntry.DbfRecordNo, 
                LowerPageRecordNo = newPage.RecordNo, 
                Fields = midEntry.Fields.Clone() };

#if DUMP_INSERTS
            System.Diagnostics.Trace.WriteLine("  newPage #" + newPage.RecordNo);
            newPage.Dump("      ");

            System.Diagnostics.Trace.WriteLine("  this page #" + RecordNo);
            Dump("      ");
#endif
            int midEntryPos = ParentPage.FindInsertPos(newMidEntry, ndxFile);
            ParentPage.InsertInThisPage(newMidEntry, midEntryPos);
        }

        internal void Dump(string sender, string indent)
        {
            NdxEntry lastLeaf = null;
            Dump(sender + indent, this.ParentPage, null, false, ref lastLeaf);
        }

        internal void Dump(string indent, NdxPage parentPage, NdxEntry parentEntry,  bool withChildren, ref NdxEntry lastLeaf)
        {
            var ndxFile = (NdxFile)mHolder.mFile;

            System.Diagnostics.Debug.WriteLine(indent
                + "Page #" + mHolder.RecordNo
                +" " + this.EntriesCount + " entries (parent: " + (ParentPage == null ? "none" : "#" + ParentPage.RecordNo)+")");
            System.Diagnostics.Debug.Assert(parentPage == ParentPage, "Invalid NDX Parent page");
            for (int i = 0; i < EntriesCount; i++)
            {
                var e = GetEntry(i);

                System.Diagnostics.Debug.WriteLine(
                    indent + "   "
                    + "Entry " + i + " (page #" + mHolder.RecordNo + ") - "
                    + " SubPage: " + (e.LowerPageRecordNo == UInt32.MaxValue ? "none" : "#" + e.LowerPageRecordNo)
                    + ", Key: " + e.Fields.ToString()
                    + " #" + e.DbfRecordNo
                    );
                if (e.mNdxNativeLowerPageNo != 0)
                {
                    NdxPage subPage = (NdxPage)ndxFile.InternalGetPage(this, (uint)e.LowerPageRecordNo,
                        /*returnNullIfNotInCache*/ false);
                    if (withChildren)
                    {
                        subPage.Dump(indent + "    ", this, e, true, ref lastLeaf);
                    }
                }
                else
                {

#if DEBUG
                    if (lastLeaf != null)
                    {
                        int[] cmpArray = new int[ndxFile.mSortFieldsCount];
                        int cmp = e.ChainCompare(cmpArray, ndxFile, lastLeaf);
                        if (cmp <= 0) System.Diagnostics.Trace.WriteLine("*** ERRROR HERE THE ORDER IS MESSED UP ***");
                    }
                    lastLeaf = e;
#endif
                }
            }
        }

        internal int CountChildren(int maxLevel)
        {
            if (maxLevel <= 1) return EntriesCount; // this is just a minimum approximation
            var ndxFile = (NdxFile)mHolder.mFile;
            int result=0;
            for (int i = 0; i < EntriesCount; i++)
            {
                var e = GetEntry(i);
                if (e.mNdxNativeLowerPageNo != 0)
                {
                    NdxPage subPage = (NdxPage)ndxFile.InternalGetPage(this, (uint)e.LowerPageRecordNo,
                        /*returnNullIfNotInCache*/ false);
                    result += subPage.CountChildren(maxLevel - 1);
                }
                else result++;
            }
            return result;
        }


        internal void LocateAndDelete(NdxEntry entry)
        {
            var ndxFile = (NdxFile)mHolder.mFile;
            bool equal;
            int originalPos;
            FindPos(entry, ndxFile, out equal, out originalPos);

            if (originalPos == EntriesCount) originalPos--;

            bool hasLowerPage = (originalPos >= 0 && GetEntry(originalPos).LowerPageRecordNo != UInt32.MaxValue);

            if (hasLowerPage)
            {
                var ndxPage2 = ndxFile.InternalGetPage(this,
                    GetEntry(originalPos).LowerPageRecordNo,
                    /* returnNullIfNotInCache */ false) as NdxPage;
                ndxPage2.LocateAndDelete(entry);
            }
            else
            {
                System.Diagnostics.Debug.Assert(equal == true, "Can't find and remove NDX entry.");
                DeleteInThisPage(entry, originalPos);
            }
        }
    }
}
