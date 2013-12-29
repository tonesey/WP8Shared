using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Wp7Shared.Net
{
    public class BaseDisposable : IDisposable
    {
        // Fields
        protected bool disposed;

        // Methods
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.DisposeBaseManagedResource();
            }
            this.DisposeBaseUnManagedResources();
            this.disposed = true;
        }

        protected virtual void DisposeBaseManagedResource()
        {
        }

        protected virtual void DisposeBaseUnManagedResources()
        {
        }

        ~BaseDisposable()
        {
            this.Dispose(false);
        }
    }


}
