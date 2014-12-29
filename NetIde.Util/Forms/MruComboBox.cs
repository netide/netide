using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class MruComboBox : ComboBox
    {
        private bool _includeEmptyItem;
        private bool _loaded;

        public event MruHistoryEventHandler LoadHistory;

        protected virtual void OnLoadHistory(MruHistoryEventArgs e)
        {
            var handler = LoadHistory;
            if (handler != null)
                handler(this, e);
        }

        public event MruHistoryEventHandler SaveHistory;

        protected virtual void OnSaveHistory(MruHistoryEventArgs e)
        {
            var handler = SaveHistory;
            if (handler != null)
                handler(this, e);
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool IncludeEmptyItem
        {
            get { return _includeEmptyItem; }
            set
            {
                if (_includeEmptyItem != value)
                {
                    _includeEmptyItem = value;

                    if (value)
                    {
                        if (Items.Count == 0 || ((string)Items[0]).Length > 0)
                            Items.Insert(0, "");
                    }
                    else
                    {
                        if (Items.Count > 0 && ((string)Items[0]).Length == 0)
                            Items.RemoveAt(0);
                    }
                }
            }
        }

        [Category("Data")]
        [DefaultValue(10)]
        public int MruMaximumSize { get; set; }

        public MruComboBox()
        {
            MruMaximumSize = 10;
        }

        public void AddTextToMru()
        {
            if (Text.Length == 0)
                return;

            foreach (string item in Items)
            {
                if (item == Text)
                    return;
            }

            Items.Insert(0, Text);

            DoSaveHistory();
        }

        private void DoSaveHistory()
        {
            var result = new List<string>();

            if (Text.Length > 0)
                result.Add(Text);

            for (int i = _includeEmptyItem ? 1 : 0; i < Items.Count; i++)
            {
                string item = (string)Items[i];

                if (item != Text)
                    result.Add(item);
                if (result.Count >= MruMaximumSize)
                    break;
            }

            OnSaveHistory(new MruHistoryEventArgs(result));
        }

        protected override void OnDropDown(EventArgs e)
        {
            DoLoadHistory();

            base.OnDropDown(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            DoLoadHistory();

            base.OnEnter(e);
        }

        private void DoLoadHistory()
        {
            if (_loaded)
                return;

            _loaded = true;

            var history = new List<string>();
            OnLoadHistory(new MruHistoryEventArgs(history));

            int offset = _includeEmptyItem ? 1 : 0;

            while (Items.Count > offset)
            {
                Items.RemoveAt(Items.Count - 1);
            }

            Items.AddRange(history);
        }
    }
}
