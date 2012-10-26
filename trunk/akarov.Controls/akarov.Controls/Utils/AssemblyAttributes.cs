using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace akarov.Controls.Utils
{
    public static class AssemblyAttributes
    {
        private  const string propertyNameTitle = "Title";
        private  const string propertyNameDescription = "Description";
        private  const string propertyNameProduct = "Product";
        private  const string propertyNameCopyright = "Copyright";
        private  const string propertyNameCompany = "Company";

      
        /// Gets the title property, which is display in the About dialogs window title.
        /// </summary>
        public static string ProductTitle
        {
            get
            {
                string result = CalculatePropertyValue<AssemblyTitleAttribute>(propertyNameTitle);
                if (string.IsNullOrEmpty(result))
                {
                    // otherwise, just get the name of the assembly itself.
                    result = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the application's version information to show.
        /// </summary>
        public static string Version
        {
            get
            {
                string result = string.Empty;
                // first, try to get the version string from the assembly.
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                if (version != null)
                {
                    result = version.ToString();
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the description about the application.
        /// </summary>
        public static string Description
        {
            get { return CalculatePropertyValue<AssemblyDescriptionAttribute>(propertyNameDescription); }
        }

        /// <summary>
        ///  Gets the product's full name.
        /// </summary>
        public static string Product
        {
            get { return CalculatePropertyValue<AssemblyProductAttribute>(propertyNameProduct); }
        }

        /// <summary>
        /// Gets the copyright information for the product.
        /// </summary>
        public static string Copyright
        {
            get { return CalculatePropertyValue<AssemblyCopyrightAttribute>(propertyNameCopyright); }
        }

        /// <summary>
        /// Gets the product's company name.
        /// </summary>
        public static string Company
        {
            get { return CalculatePropertyValue<AssemblyCompanyAttribute>(propertyNameCompany); }
        }

       
        /// <summary>
        /// Gets the specified property value either from a specific attribute, or from a resource dictionary.
        /// </summary>
        /// <typeparam name="T">Attribute type that we're trying to retrieve.</typeparam>
        /// <param name="propertyName">Property name to use on the attribute.</param>
        /// <param name="xpathQuery">XPath to the element in the XML data resource.</param>
        /// <returns>The resulting string to use for a property.
        /// Returns null if no data could be retrieved.</returns>
        private static string CalculatePropertyValue<T>(string propertyName)
        {
            string result = string.Empty;
            // first, try to get the property value from an attribute.
            object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
            {
                T attrib = (T)attributes[0];
                PropertyInfo property = attrib.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    result = property.GetValue(attributes[0], null) as string;
                }
            }
            return result;
        }
    }
}
