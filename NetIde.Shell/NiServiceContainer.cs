using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetIde.Shell
{
    public sealed class NiServiceContainer : ServiceObject, IServiceContainer, IDisposable
    {
        private ServiceCollection _services;
        private readonly IServiceProvider _parentProvider;
        private bool _disposed;

        public NiServiceContainer()
        {
        }

        public NiServiceContainer(IServiceProvider parentProvider)
        {
            _parentProvider = parentProvider;
        }

        private IServiceContainer Container
        {
            get
            {
                IServiceContainer container = null;
                if (_parentProvider != null)
                    container = (IServiceContainer)_parentProvider.GetService(typeof(IServiceContainer));
                return container;
            }
        }

        private ServiceCollection Services
        {
            get { return _services ?? (_services = new ServiceCollection()); }
        }

        public void AddService(Type serviceType, object serviceInstance)
        {
            AddService(serviceType, serviceInstance, false);
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            if (promote)
            {
                var container = Container;

                if (container != null)
                {
                    container.AddService(serviceType, serviceInstance, true);
                    return;
                }
            }

            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (serviceInstance == null)
                throw new ArgumentNullException("serviceInstance");

            if (
                !(serviceInstance is ServiceCreatorCallback) &&
                !serviceInstance.GetType().IsCOMObject &&
                !serviceType.IsInstanceOfType(serviceInstance)
            )
                throw new ArgumentException(Labels.ServiceNotOfValidType);

            if (Services.ContainsKey(serviceType))
                throw new ArgumentException(Labels.ServiceAlreadyRegistered, "serviceType");

            Services[serviceType] = serviceInstance;
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            AddService(serviceType, callback, false);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            if (promote)
            {
                var container = Container;

                if (container != null)
                {
                    container.AddService(serviceType, callback, true);
                    return;
                }
            }

            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (callback == null)
                throw new ArgumentNullException("callback");

            if (Services.ContainsKey(serviceType))
                throw new ArgumentException(Labels.ServiceAlreadyRegistered, "serviceType");

            Services[serviceType] = callback;
        }

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            if (serviceType.IsEquivalentTo(typeof(IServiceContainer)))
                return this;

            object service;
            Services.TryGetValue(serviceType, out service);

            if (service is ServiceCreatorCallback)
            {
                service = ((ServiceCreatorCallback)service)(this, serviceType);
                if (
                    service != null &&
                    !service.GetType().IsCOMObject &&
                    !serviceType.IsInstanceOfType(service)
                )
                    service = null;

                Services[serviceType] = service;
            }

            if (service == null && _parentProvider != null)
                service = _parentProvider.GetService(serviceType);

            return service;
        }

        public void RemoveService(Type serviceType)
        {
            RemoveService(serviceType, false);
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            if (promote)
            {
                var container = Container;

                if (container != null)
                {
                    container.RemoveService(serviceType, true);
                    return;
                }
            }

            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            Services.Remove(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                var serviceCollection = _services;
                _services = null;

                if (serviceCollection != null)
                {
                    foreach (object service in serviceCollection.Values)
                    {
                        var disposable = service as IDisposable;

                        if (disposable != null)
                            disposable.Dispose();
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private sealed class ServiceCollection : Dictionary<Type, object>
        {
            private static readonly EmbeddedTypeAwareTypeComparer _serviceTypeComparer = new EmbeddedTypeAwareTypeComparer();

            private sealed class EmbeddedTypeAwareTypeComparer : IEqualityComparer<Type>
            {
                public bool Equals(Type x, Type y)
                {
                    return x.IsEquivalentTo(y);
                }

                public int GetHashCode(Type obj)
                {
                    return obj.FullName.GetHashCode();
                }
            }

            public ServiceCollection()
                : base(_serviceTypeComparer)
            {
            }
        }
    }
}
