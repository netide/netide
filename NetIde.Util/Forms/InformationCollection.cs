using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NetIde.Util.Forms
{
    public class InformationCollection : Collection<InformationItem>
    {
        private readonly InformationBar _bar;

        internal InformationCollection(InformationBar bar)
        {
            if (bar == null)
                throw new ArgumentNullException("bar");

            _bar = bar;
        }

        public void Add(InformationIcon icon, string text)
        {
            Add(new InformationItem(icon, text));
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item.Selected)
                    _bar.SelectedItem = null;

                item.Bar = null;
            }

            base.ClearItems();

            EnsureSelection();

            _bar.RebuildButtons();
        }

        protected override void InsertItem(int index, InformationItem item)
        {
            base.InsertItem(index, item);

            if (item.Bar != null)
                throw new InvalidOperationException("Information item cannot be added to two collections");

            item.Bar = _bar;

            EnsureSelection();

            _bar.RebuildButtons();
        }

        protected override void RemoveItem(int index)
        {
            if (this[index].Selected)
                _bar.SelectedItem = null;

            this[index].Bar = null;

            base.RemoveItem(index);

            EnsureSelection();

            _bar.RebuildButtons();
        }

        protected override void SetItem(int index, InformationItem item)
        {
            if (item.Bar != null)
                throw new InvalidOperationException("Information item cannot be added to two collections");

            bool wasSelected = this[index].Selected;
            if (wasSelected)
                _bar.SelectedItem = null;

            this[index].Bar = null;

            base.SetItem(index, item);

            item.Bar = _bar;

            if (wasSelected)
                item.Selected = true;
        }

        private void EnsureSelection()
        {
            if (_bar.SelectedItem == null && Count > 0)
                _bar.SelectedItem = this[0];
        }
    }
}
