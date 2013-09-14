using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIAutomationWrapper
{
    [DebuggerTypeProxy(typeof(DebugView))]
    [DebuggerDisplay("Count = {Count}")]
    public class AutomationWrapperCollection : ICollection, ICollection<AutomationWrapper>
    {
        private readonly AutomationWrapper _owner;
        public AutomationElementCollection AutomationElementCollection { get; private set; }

        public AutomationWrapper this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException("name");

                return AutomationWrapper.Wrap(
                    _owner.AutomationElement.FindFirst(
                        TreeScope.Children,
                        new PropertyCondition(AutomationElement.NameProperty, name)
                    )
                );
            }
        }

        public AutomationWrapper this[string name, ControlType controlType]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException("name");

                return AutomationWrapper.Wrap(
                    _owner.AutomationElement.FindFirst(
                        TreeScope.Children,
                        new AndCondition(
                            new PropertyCondition(AutomationElement.NameProperty, name),
                            new PropertyCondition(AutomationElement.ControlTypeProperty, AutomationWrapper.GetControlType(controlType))
                        )
                    )
                );
            }
        }

        public AutomationWrapper this[ControlType controlType]
        {
            get
            {
                return AutomationWrapper.Wrap(
                    _owner.AutomationElement.FindFirst(
                        TreeScope.Children,
                        new PropertyCondition(AutomationElement.ControlTypeProperty, AutomationWrapper.GetControlType(controlType))
                    )
                );
            }
        }

        public AutomationWrapper this[int index]
        {
            get { return new AutomationWrapper(AutomationElementCollection[index]); }
        }

        public AutomationWrapperCollection(AutomationWrapper owner, AutomationElementCollection automationElementCollection)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (automationElementCollection == null)
                throw new ArgumentNullException("automationElementCollection");

            _owner = owner;
            AutomationElementCollection = automationElementCollection;
        }

        public void Add(AutomationWrapper item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(AutomationWrapper item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(AutomationWrapper[] array, int arrayIndex)
        {
            CopyTo((Array)array, arrayIndex);
        }

        public int Count
        {
            get { return AutomationElementCollection.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(AutomationWrapper item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<AutomationWrapper> GetEnumerator()
        {
            foreach (AutomationElement item in AutomationElementCollection)
            {
                yield return new AutomationWrapper(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            var elements = new AutomationElement[Count];

            AutomationElementCollection.CopyTo(elements, 0);

            for (int i = 0; i < elements.Length; i++)
            {
                array.SetValue(new AutomationWrapper(elements[i]), i + index);
            }
        }

        public bool IsSynchronized
        {
            get { return AutomationElementCollection.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return AutomationElementCollection.SyncRoot; }
        }

        internal sealed class DebugView
        {
            private readonly AutomationWrapperCollection _collection;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public AutomationWrapper[] Items
            {
                get
                {
                    var array = new AutomationWrapper[_collection.Count];

                    _collection.CopyTo(array, 0);

                    return array;
                }
            }

            public DebugView(AutomationWrapperCollection collection)
            {
                if (collection == null)
                    throw new ArgumentNullException("collection");

                _collection = collection;
            }
        }
    }
}
