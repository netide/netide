using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;
using NetIde.Util.Forms;

namespace NetIde.Core.ToolWindows.ProjectExplorer
{
    public partial class ProjectExplorerControl : NetIde.Util.Forms.UserControl
    {
        private INiProjectManager _projectManager;
        private ResourceManager _resourceManager;
        private Listener _listener;
        private ImageListManager _imageList;
        private readonly IResource _defaultContainer;
        private readonly IResource _defaultFile;

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                _projectManager = (INiProjectManager)GetService(typeof(INiProjectManager));
                _listener = new Listener(this);

                if (_projectManager.ActiveProject != null)
                    ReloadProject();
            }
        }

        public ProjectExplorerControl()
        {
            InitializeComponent();

            _treeView.ApplyExplorerTheme();

            _resourceManager = new ResourceManager();

            _imageList = new ImageListManager();
            _treeView.ImageList = _imageList.ImageList;

            _defaultFile = LoadResource(_imageList.GetIndexForFileName(
                Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
            ));
            _defaultContainer = LoadResource(_imageList.GetIndexForDirectory(Path.GetTempPath()));

            Disposed += ProjectExplorerControl_Disposed;
        }

        private IResource LoadResource(int imageIndex)
        {
            using (var stream = new MemoryStream())
            {
                _imageList.ImageList.Images[imageIndex].Save(
                    stream, ImageFormat.Png
                );
                return ResourceUtil.FromByteArray(stream.ToArray());
            }
        }

        void ProjectExplorerControl_Disposed(object sender, EventArgs e)
        {
            if (_listener != null)
            {
                _listener.Dispose();
                _listener = null;
            }
            if (_imageList != null)
            {
                _imageList.Dispose();
                _imageList = null;
            }
            if (_resourceManager != null)
            {
                _resourceManager.Dispose();
                _resourceManager = null;
            }
        }

        private void ReloadProject()
        {
            _treeView.BeginUpdate();

            try
            {
                if (_treeView.Nodes.Count > 0)
                {
                    DestroyNodes(_treeView.Nodes);
                    _treeView.Nodes.Clear();
                }

                if (_projectManager.ActiveProject != null)
                {
                    LoadNode(_treeView.Nodes, _projectManager.ActiveProject);
                    _treeView.Nodes[0].Expand();
                }
            }
            finally
            {
                _treeView.EndUpdate();
            }
        }

        private void DestroyNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                DestroyNodes(node.Nodes);

                ((IDisposable)node.Tag).Dispose();
            }
        }

        private void LoadNode(TreeNodeCollection nodes, INiHierarchy item)
        {
            var treeNode = new TreeNodeManager(this, item).TreeNode;

            nodes.Add(treeNode);

            foreach (var child in item.GetChildren())
            {
                LoadNode(treeNode.Nodes, child);
            }
        }

        private void _treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                var manager = (TreeNodeManager)e.Node.Tag;
                var projectItem = manager.Item as INiProjectItem;

                if (projectItem != null)
                {
                    NiShellUtil.Checked(Site, () =>
                    {
                        INiWindowFrame unused;
                        ErrorUtil.ThrowOnFailure(projectItem.Open(out unused));
                    });
                }
            }
        }

        public HResult GetSelectedHierarchy(out INiHierarchy hier)
        {
            hier = null;

            try
            {
                var node = _treeView.SelectedNode;

                if (node != null)
                    hier = ((TreeNodeManager)node.Tag).Item;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class Listener : NiEventSink, INiProjectManagerNotify
        {
            private readonly ProjectExplorerControl _owner;

            public Listener(ProjectExplorerControl owner)
                : base(owner._projectManager)
            {
                _owner = owner;
            }

            public void OnActiveProjectChanged()
            {
                _owner.ReloadProject();
            }
        }

        private class TreeNodeManager : IDisposable
        {
            private readonly ProjectExplorerControl _owner;
            private Listener _listener;
            private readonly Dictionary<Image, int> _images = new Dictionary<Image, int>();
            private bool _disposed;

            public INiHierarchy Item { get; private set; }
            public TreeNode TreeNode { get; private set; }

            public TreeNodeManager(ProjectExplorerControl owner, INiHierarchy item)
            {
                _owner = owner;
                Item = item;

                _listener = new Listener(this);

                TreeNode = new TreeNode
                {
                    Text = (string)Item.GetPropertyEx(NiHierarchyProperty.Name),
                    Tag = this
                };

                UpdateImage();
            }

            private void Reorder()
            {
                throw new NotImplementedException();
            }

            private void UpdateImage()
            {
                var itemType = ((NiHierarchyType?)Item.GetPropertyEx(NiHierarchyProperty.ItemType)).GetValueOrDefault(NiHierarchyType.VirtualItem);

                int imageIndex = GetImageIndex(
                    (IResource)Item.GetPropertyEx(NiHierarchyProperty.Image) ??
                    (
                        itemType == NiHierarchyType.Directory || itemType == NiHierarchyType.VirtualContainer
                        ? _owner._defaultContainer
                        : _owner._defaultFile
                    )
                );
                    
                var overlayImage = (IResource)Item.GetPropertyEx(NiHierarchyProperty.OverlayImage);
                if (overlayImage != null)
                {
                    imageIndex = _owner._imageList.GetIndexForOverlay(
                        imageIndex,
                        GetImageIndex(overlayImage)
                    );
                }

                TreeNode.ImageIndex = TreeNode.SelectedImageIndex = imageIndex;
            }

            private int GetImageIndex(IResource resource)
            {
                var image = _owner._resourceManager.GetImage(resource);

                int imageIndex;
                if (!_images.TryGetValue(image, out imageIndex))
                {
                    imageIndex = _owner._imageList.ImageList.Images.Count;
                    _images.Add(image, imageIndex);
                    _owner._imageList.ImageList.Images.Add(image);
                }

                return imageIndex;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    if (_listener != null)
                    {
                        _listener.Dispose();
                        _listener = null;
                    }

                    _disposed = true;
                }
            }

            private class Listener : NiEventSink, INiHierarchyNotify
            {
                private readonly TreeNodeManager _manager;

                public Listener(TreeNodeManager manager)
                    : base(manager.Item)
                {
                    _manager = manager;
                }

                public void OnChildAdded(INiHierarchy hier)
                {
                    throw new NotImplementedException();
                }

                public void OnChildRemoved(INiHierarchy hier)
                {
                    throw new NotImplementedException();
                }

                public void OnPropertyChanged(INiHierarchy hier, int property)
                {
                    switch ((NiHierarchyProperty)property)
                    {
                        case NiHierarchyProperty.Name:
                            _manager.TreeNode.Text = (string)hier.GetPropertyEx(NiHierarchyProperty.Name);
                            _manager.Reorder();
                            break;

                        case NiHierarchyProperty.SortPriority:
                            _manager.Reorder();
                            break;

                        case NiHierarchyProperty.Image:
                        case NiHierarchyProperty.OverlayImage:
                        case NiHierarchyProperty.ItemType:
                            _manager.UpdateImage();
                            break;
                    }
                }
            }
        }
    }
}
