using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Text;

namespace JLQ.Common
{
    public class UseClearTypeGridFit : IDisposable
    {
        private Graphics _graphics;
        private TextRenderingHint _textRenderingHint;

        #region MethodsPublic
        public UseClearTypeGridFit(Graphics graphics)
        {
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics",
					string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    StaticResource.IDS_ArgumentException,
					"graphics"));
			}

			this._graphics = graphics;
            this._textRenderingHint = this._graphics.TextRenderingHint;
            this._graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

		~UseClearTypeGridFit()
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
				this._graphics.TextRenderingHint = this._textRenderingHint;
			}
		}
		#endregion
    }
}
