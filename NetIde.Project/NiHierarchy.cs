using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Project
{
    public class NiHierarchy : ServiceObject, INiHierarchy
    {
        private const StringComparison NameComparison = StringComparison.CurrentCultureIgnoreCase;

        private IServiceProvider _serviceProvider;
        private readonly Dictionary<int, object> _properties = new Dictionary<int, object>();
        private readonly NiConnectionPoint<INiHierarchyNotify> _connectionPoint = new NiConnectionPoint<INiHierarchyNotify>();
        private readonly LinkedList<NiHierarchy> _children = new LinkedList<NiHierarchy>();
        private LinkedListNode<NiHierarchy> _position;
        private NiHierarchy _parent;
        private string _name;
        private int? _sortPriority;

        public HResult SetSite(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return HResult.OK;
        }

        public HResult GetSite(out IServiceProvider serviceProvider)
        {
            serviceProvider = _serviceProvider;
            return HResult.OK;
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiHierarchyNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        public HResult GetProperty(int property, out object value)
        {
            value = null;

            try
            {
                switch ((NiHierarchyProperty)property)
                {
                    case NiHierarchyProperty.FirstChild:
                        var first = _children.First;
                        if (first != null)
                        {
                            value = first.Value;
                            return HResult.OK;
                        }
                        return HResult.False;

                    case NiHierarchyProperty.NextSibling:
                        if (_position != null)
                        {
                            var next = _position.Next;
                            if (next != null)
                            {
                                value = next.Value;
                                return HResult.OK;
                            }
                        }
                        return HResult.False;

                    case NiHierarchyProperty.Parent:
                        if (_parent != null)
                        {
                            value = _parent;
                            return HResult.OK;
                        }
                        return HResult.False;

                    case NiHierarchyProperty.SortPriority:
                        if (_sortPriority.HasValue)
                        {
                            value = _sortPriority;
                            return HResult.OK;
                        }
                        return HResult.False;

                    case NiHierarchyProperty.Name:
                        if (_name != null)
                        {
                            value = _name;
                            return HResult.OK;
                        }
                        return HResult.False;

                    case NiHierarchyProperty.ContainingProject:
                        value = GetRoot() as INiProject;
                        return HResult.OK;

                    case NiHierarchyProperty.Root:
                        value = GetRoot();
                        return HResult.OK;

                    default:
                        if (_properties.TryGetValue(property, out value))
                            return HResult.OK;
                        return HResult.False;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private object GetRoot()
        {
            var current = this;

            while (current._parent != null)
            {
                current = current._parent;
            }

            return current;
        }

        public HResult SetProperty(int property, object value)
        {
            try
            {
                try
                {
                    if (property < 0)
                    {
                        switch ((NiHierarchyProperty)property)
                        {
                            case NiHierarchyProperty.FirstChild:
                            case NiHierarchyProperty.NextSibling:
                                throw new ArgumentOutOfRangeException("property", NeutralResources.CannotSetProperty);

                            case NiHierarchyProperty.Parent:
                                if (value != null && !(value is NiHierarchy))
                                    throw new ArgumentOutOfRangeException("value", NeutralResources.ParentMustBeHierarchy);

                                return ProcessParentChange((NiHierarchy)value);

                            case NiHierarchyProperty.Image:
                            case NiHierarchyProperty.OverlayImage:
                                if (value != null && !(value is IResource))
                                    throw new ArgumentOutOfRangeException("value", NeutralResources.InvalidPropertyType);
                                break;

                            case NiHierarchyProperty.ItemType:
                                if (value != null && !(value is NiHierarchyType))
                                    throw new ArgumentOutOfRangeException("value", NeutralResources.InvalidPropertyType);
                                break;

                            case NiHierarchyProperty.OwnerType:
                                if (value != null && !(value is Guid))
                                    throw new ArgumentOutOfRangeException("value", NeutralResources.InvalidPropertyType);
                                break;

                            case NiHierarchyProperty.Name:
                                if (value != null && !(value is string))
                                    throw new ArgumentOutOfRangeException("value", NeutralResources.InvalidPropertyType);

                                string oldName = _name;
                                _name = (string)value;

                                if (
                                    _parent != null &&
                                    !String.Equals(_name, oldName, NameComparison)
                                )
                                    _parent.Reposition(this);
                                return HResult.OK;

                            case NiHierarchyProperty.SortPriority:
                                if (value != null && !(value is int))
                                    throw new ArgumentOutOfRangeException("value", NeutralResources.InvalidPropertyType);

                                var oldSortPriority = _sortPriority;
                                _sortPriority = (int?)value;

                                if (
                                    _parent != null &&
                                    _sortPriority.GetValueOrDefault() != oldSortPriority.GetValueOrDefault()
                                )
                                    _parent.Reposition(this);
                                return HResult.OK;

                            default:
                                throw new ArgumentOutOfRangeException("property", NeutralResources.UnknownProperty);
                        }
                    }

                    if (value == null)
                        _properties.Remove(property);
                    else
                        _properties[property] = value;
                }
                finally
                {
                    _connectionPoint.ForAll(p => p.OnPropertyChanged(this, property));
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private HResult ProcessParentChange(NiHierarchy parent)
        {
            if (_parent != null)
                _parent.RemoveChild(this);

            _parent = parent;

            if (_parent != null)
                _parent.AddChild(this);

            return HResult.OK;
        }

        private void AddChild(NiHierarchy item)
        {
            item.SetSite(this);
            AddToChildren(item);

            _connectionPoint.ForAll(p => p.OnChildAdded(item));
        }

        private void AddToChildren(NiHierarchy item)
        {
            var current = _children.First;

            while (current != null)
            {
                // Copy to suppress warning CS1690: Accessing a member
                // on * may cause a runtime exception because it is a field of
                // a marshal-by-reference class.
                int? currentSortPriority = current.Value._sortPriority;

                int compare = _sortPriority.GetValueOrDefault().CompareTo(
                    currentSortPriority.GetValueOrDefault()
                );
                if (compare == 0)
                    compare = String.Compare(_name, current.Value._name, NameComparison);

                if (compare < 0)
                    break;

                current = current.Next;
            }

            item._position =
                current == null
                ? _children.AddLast(item)
                : _children.AddBefore(current, item);
        }

        private void RemoveChild(NiHierarchy item)
        {
            RemoveFromChildren(item);
            item.SetSite(null);

            _connectionPoint.ForAll(p => p.OnChildRemoved(item));
        }

        private void RemoveFromChildren(NiHierarchy item)
        {
            _children.Remove(item._position);
            item._position = null;
        }

        private void Reposition(NiHierarchy item)
        {
            RemoveFromChildren(item);
            AddToChildren(item);
        }

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public virtual HResult Close()
        {
            return HResult.OK;
        }
    }
}
