using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace JLQ.Common
{
    public class UseAntiAlias : IDisposable
    {
        private Graphics _graphics;
        private SmoothingMode _smoothingMode;

        #region MethodsPublic
        public UseAntiAlias(Graphics graphics)
        {
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics",
					string.Format(System.Globalization.CultureInfo.InvariantCulture,
					StaticResource.IDS_ArgumentException,
					"graphics"));
			}

			this._graphics = graphics;
            this._smoothingMode = _graphics.SmoothingMode;
            this._graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

		~UseAntiAlias()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region MethodsProtected
		protected virtual void Dispose(bool disposing)
		{
			if (disposing == true)
			{
				this._graphics.SmoothingMode = this._smoothingMode;
			}
		}
		#endregion
    }
}
