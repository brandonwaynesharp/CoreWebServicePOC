using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebServicePOC.repo.Tests
{
    public static class ObjectExtensions
    {
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public static void SetPropertyValue<T>(this object obj, string propertyName, T value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value);
        }

        /// <summary>
        /// Compares the properties of two objects and returns true if the values are equal
        /// </summary>
        /// <param name="obj">This object</param>
        /// <param name="toCompare">The ExpandoObject to compare against</param>
        /// <param name="ignoreMissingProperties">If true, comparisons will only occur on properties which both objects contain</param>
        /// <returns>True, if all properties are equal</returns>
        public static bool ArePropertiesEqual(this object obj, ExpandoObject toCompare, bool ignoreMissingProperties = false)
        {
            var thisProperties = GetProperties(obj);
            var toCompareProperties = toCompare as IDictionary<string, object>;

            if (!ignoreMissingProperties && thisProperties.Count() != toCompareProperties.Count())
            {
                return false;
            }

            foreach (var prop in thisProperties)
            {
                var value = prop.Value.GetValue(obj);
                object toCompareValue;
                if (!toCompareProperties.TryGetValue(prop.Key, out toCompareValue))
                {
                    if (!ignoreMissingProperties)
                    {
                        return false;
                    }
                }
                else if (!value.Equals(toCompareValue))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the properties of two objects and returns true if the values are equal
        /// </summary>
        /// <param name="obj">This ExpandoObject</param>
        /// <param name="toCompare">The object to compare against</param>
        /// <param name="ignoreMissingProperties">If true, comparisons will only occur on properties which both objects contain</param>
        /// <returns>True, if all properties are equal</returns>
        public static bool ArePropertiesEqual(this ExpandoObject obj, object toCompare, bool ignoreMissingProperties = false)
        {
            var thisProperties = obj as IDictionary<string, object>;
            var toCompareProperties = GetProperties(toCompare);

            if (!ignoreMissingProperties && thisProperties.Count() != toCompareProperties.Count())
            {
                return false;
            }

            foreach (var kvp in thisProperties)
            {
                PropertyInfo toCompareProp;
                if (!toCompareProperties.TryGetValue(kvp.Key, out toCompareProp))
                {
                    if (!ignoreMissingProperties)
                    {
                        return false;
                    }
                }
                else if (!kvp.Value.Equals(toCompareProp.GetValue(toCompare)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the properties of two objects and returns true if the values are equal
        /// </summary>
        /// <param name="obj">This object</param>
        /// <param name="toCompare">The object to compare against</param>
        /// <param name="ignoreMissingProperties">If true, comparisons will only occur on properties which both objects contain</param>
        /// <returns>True, if all properties are equal</returns>
        public static bool ArePropertiesEqual(this object obj, object toCompare, bool ignoreMissingProperties = false)
        {
            if (obj == null)
            {
                return toCompare == null;
            }

            if (toCompare == null)
            {
                return false;
            }

            var thisProperties = GetProperties(obj);
            var toCompareProperties = GetProperties(toCompare);

            if (!ignoreMissingProperties && thisProperties.Count() != toCompareProperties.Count())
            {
                return false;
            }

            foreach (var prop in thisProperties)
            {
                var value = prop.Value.GetValue(obj);
                PropertyInfo toCompareProp;
                if (!toCompareProperties.TryGetValue(prop.Key, out toCompareProp))
                {
                    if (!ignoreMissingProperties)
                    {
                        return false;
                    }
                }
                else
                {
                    var toCompareValue = toCompareProp.GetValue(toCompare);
                    if (value == null)
                    {
                        if (toCompareValue != null)
                        {
                            return false;
                        }
                    }
                    else if (!value.Equals(toCompareValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static ExpandoObject ToExpandoObject(this object obj)
        {
            var properties = GetProperties(obj);

            var expando = new ExpandoObject();
            IDictionary<string, object> dict = expando;

            foreach (var prop in properties)
            {
                dict.Add(prop.Key, prop.Value.GetValue(obj));
            }

            return expando;
        }

        private static Dictionary<string, PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties(
                BindingFlags.GetProperty |
                BindingFlags.Public |
                BindingFlags.Instance).ToDictionary(m => m.Name);
        }
    }
}
