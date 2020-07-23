using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GCore.Logging;
using GCore.WinForms.Controls.GracialList;
using GCore.WinForms.Controls.RuntimeProperty;
using GCore.Threading;
using GCore.Extensions;
using GCore.WinForms.Extensions;

namespace GCore.WinForms.Controls {
    public partial class LogList : UserControl {

        public class LogListQueue{
            public delegate void OnEntryRemovedHandler(LogEntry logEntry);
            public event OnEntryRemovedHandler OnEntryRemoved;

            public delegate void OnEntryAddedHandler(LogEntry logEntry);
            public event OnEntryAddedHandler OnEntryAdded;

            private List<LogEntry> _entrys = new List<LogEntry>();

            private int _maxEntrys = 20000;

            private LogEntry.LogTypes _logFilter = 
#if DEBUG
            LogEntry.LogTypes.All
#else
            LogEntry.LogTypes.All //LogEntry.LogTypes.NoDebug
#endif
;

            public int MaxEntrys {
                get{ return this._maxEntrys; } 
                set{
                    this._maxEntrys = value;
                    this._updateListSize();
                }
            }

            private void _updateListSize() {
                while (_entrys.Count > _maxEntrys) {
                    if(OnEntryRemoved != null) OnEntryRemoved(_entrys[0]);
                    _entrys.RemoveAt(0);
                }
            }

            private bool _isInFilter(LogEntry.LogTypes type) {
                return (type & this._logFilter) != 0;
            }

            public LogListQueue() {
                Log.OnLog += new Log.LogHandler(AddEntry);
            }

            ~LogListQueue() {
                Log.OnLog -= new Log.LogHandler(AddEntry);
            }

            public void RefreshWithFilter(LogEntry.LogTypes filter) {
                _logFilter = filter;
                if (OnEntryAdded != null)
                    foreach (LogEntry entry in _entrys)
                        if (_isInFilter(entry.LogType))
                            OnEntryAdded(entry);
            }

            public void AddEntry(LogEntry entry) {
                if (OnEntryAdded != null)
                    if (_isInFilter(entry.LogType))
                        OnEntryAdded(entry);
                _entrys.Add(entry);
                _updateListSize();
            }

            public void Clear() {
                _entrys.Clear();
            }

        }

        private LogListQueue _logQueue = new LogListQueue();

        private Dictionary<LogEntry, GLItem> _logItemMap = new Dictionary<LogEntry, GLItem>();

        private bool _doNotUpdate = false;

        public LogList() {
            InitializeComponent();
        }

        void _logQueue_OnEntryRemoved(LogEntry logEntry) {
            Async.UI(() => {
                if (_logItemMap.ContainsKey(logEntry)) {
                    int itemIndex = this.glLogList.Items.FindItemIndex(_logItemMap[logEntry]);
                    if (itemIndex >= 0)
                        this.glLogList.Items.Remove(itemIndex);
                }
            }, glLogList, true);
        }

        void _logQueue_OnEntryAdded(LogEntry logEntry) {
            Async.UI(() => {
                GLItem item = new GLItem();
                item.Tag = logEntry;

                GLSubItem itemTime = new GLSubItem();
                itemTime.Text = logEntry.TimeStamp.ToString("yyyy-MM-dd H:mm:ss.fff");

                GLSubItem itemType = new GLSubItem();
                itemType.Text = logEntry.LogType.ToString();

                GLSubItem itemMesstage = new GLSubItem();
                itemMesstage.Text = logEntry.Message;

                GLSubItem itemParams = new GLSubItem();
                itemParams.Text = "";
                foreach (object o in logEntry.Params)
                    itemParams.Text += o.ToString() + "; ";

                item.SubItems.Add(itemTime);
                item.SubItems.Add(itemType);
                item.SubItems.Add(itemMesstage);
                item.SubItems.Add(itemParams);

                switch (logEntry.LogType) {
                    case LogEntry.LogTypes.Success:
                        item.ForeColor = Color.Green;
                        break;

                    case LogEntry.LogTypes.Warn:
                        item.BackColor = Color.Orange;
                        break;

                    case LogEntry.LogTypes.Debug:
                        item.ForeColor = Color.Gray;
                        break;

                    case LogEntry.LogTypes.Exception:
                        item.ForeColor = Color.Red;
                        break;

                    case LogEntry.LogTypes.Error:
                        item.BackColor = Color.Red;
                        break;

                    case LogEntry.LogTypes.Fatal:
                        item.BackColor = Color.DarkRed;
                        break;

                }

                _logItemMap.Add(logEntry, item);
                glLogList.Items.Add(item);

                if (glLogList.vPanelScrollBar.Visible && cbAutoscroll.Checked) glLogList.vPanelScrollBar.Value = glLogList.vPanelScrollBar.Maximum;
                if (!_doNotUpdate) {
                    glLogList.Invalidate();
                }
            }, glLogList, true);
        }

        private void cbSuccess_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void cbInfo_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void cbWarn_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void cbException_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void cbError_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void cbFatal_CheckedChanged(object sender, EventArgs e) {
            _updateList();
        }

        private void _updateList() {
            LogEntry.LogTypes typeFilter = 0;

            if (cbSuccess.Checked) typeFilter |= LogEntry.LogTypes.Success;
            if (cbInfo.Checked) typeFilter |= LogEntry.LogTypes.Info;
            if (cbDebug.Checked) typeFilter |= LogEntry.LogTypes.Debug;
            if (cbWarn.Checked) typeFilter |= LogEntry.LogTypes.Warn;
            if (cbException.Checked) typeFilter |= LogEntry.LogTypes.Exception;
            if (cbError.Checked) typeFilter |= LogEntry.LogTypes.Error;
            if (cbFatal.Checked) typeFilter |= LogEntry.LogTypes.Fatal;

            _doNotUpdate = true;
            glLogList.Items.Clear();
            _logItemMap.Clear();

            _logQueue.RefreshWithFilter(typeFilter);

            _doNotUpdate = false;

            glLogList.Invalidate();
        }

        private void glLogList_SelectedIndexChanged(object source, ClickEventArgs e) {
            rPropertyGrid.SelectedObject = glLogList.FocusedItem.Tag;

            cbParams.Items.Clear();
            cbParams.Text = "";
            foreach (Object o in ((LogEntry)glLogList.FocusedItem.Tag).Params) {
                cbParams.Text += o.ToString() + "; ";
                cbParams.Items.Add(new ComboboxItem(o.ToString(), o));
            }
        }


        private void cbParams_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbParams.SelectedItem == null) return;
            if (!(cbParams.SelectedItem is ComboboxItem)) return;
            RuntimeEditor re = new RuntimeEditor();
            re.SelectedObject = ((ComboboxItem)cbParams.SelectedItem).Value;
            re.EnableWindowFinder = false;
            re.ShowDialog();
        }

        private void LogList_Load(object sender, EventArgs e) {
            _logQueue.OnEntryAdded += new LogListQueue.OnEntryAddedHandler(_logQueue_OnEntryAdded);
            _logQueue.OnEntryRemoved += new LogListQueue.OnEntryRemovedHandler(_logQueue_OnEntryRemoved);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            glLogList.Items.Clear();
            _logItemMap.Clear();
            _logQueue.Clear();
            glLogList.Invalidate();
        }
    }
}
