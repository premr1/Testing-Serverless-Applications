﻿using System;

using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;

using Microsoft.Practices.ServiceLocation;

using Sample.Extensions;

namespace Sample.Functions.ServiceLocators
{
    /// <summary>
    /// This represents the builder entity for service locator.
    /// </summary>
    public class ServiceLocatorBuilder : IServiceLocatorBuilder
    {
        private ContainerBuilder _containerBuilder;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorBuilder"/> class.
        /// </summary>
        public ServiceLocatorBuilder()
        {
            this._containerBuilder = new ContainerBuilder();
        }

        /// <summary>
        /// Creates a service locator with the component registrations.
        /// </summary>
        /// <returns>Returns the <see cref="IServiceLocator"/>.</returns>
        public IServiceLocator Build()
        {
            var container = this._containerBuilder.Build();

            return new AutofacServiceLocator(container);
        }

        /// <summary>
        /// Registers a module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="handler"><see cref="RegistrationHandler"/> instance.</param>
        /// <returns>Returns the <see cref="IServiceLocatorBuilder"/> instance.</returns>
        public IServiceLocatorBuilder RegisterModule<TModule>(RegistrationHandler handler = null) where TModule : IModule, new()
        {
            this._containerBuilder.RegisterModule<TModule>();

            if (handler.IsNullOrDefault())
            {
                return this;
            }

            if (handler.RegisterTypeAsInstancePerDependency.IsNullOrDefault())
            {
                return this;
            }

            handler.RegisterTypeAsInstancePerDependency.Invoke(this._containerBuilder);

            return this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value that determines whether TODO.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.ReleaseManagedResources();
            }

            this.ReleaseUnmanagedResources();

            this._disposed = true;
        }

        /// <summary>
        /// Releases managed resources during the disposing event.
        /// </summary>
        protected virtual void ReleaseManagedResources()
        {
            // Release managed resources here.
        }

        /// <summary>
        /// Releases unmanaged resources during the disposing event.
        /// </summary>
        protected virtual void ReleaseUnmanagedResources()
        {
            this._containerBuilder = null;
        }
    }
}