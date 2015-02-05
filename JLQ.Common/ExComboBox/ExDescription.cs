namespace JLQ.Common
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    internal class ExDescription : DescriptionAttribute
    {
        private string attribute;

        public ExDescription(string attribute)
        {
            this.attribute = attribute;
        }

        public override string Description
        {
            get
            {
                return Properties.Resources.ResourceManager.GetString(this.attribute, CultureInfo.CurrentUICulture);
            }
        }
    }
}

