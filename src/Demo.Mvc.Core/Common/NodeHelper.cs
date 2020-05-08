using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Demo.Mvc.Core.Common
{

    public class Node
    {
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public string ValueString { get; set; }
        public IList<Node> Childs { get; set; }

        public override string ToString()
        {
            var result = "<li><span>" + PropertyName + " : " +
                         (string.IsNullOrEmpty(ValueString) ? Type : ValueString) + "</span>";

            if (Childs != null)
            {
                result += "<ul>";
                foreach (var child in Childs)
                {
                    result += child.ToString();
                }
                result += "</ul>";
            }

            result += "</li>";

            return result;
        }
    }
    public class NodeHelper
    {
        /// <summary>
        ///     Retourne les clé valeur associé au membre de l'objet
        /// </summary>
        /// <param name="poco"> </param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"> </param>
        /// <returns></returns>
        public static Node GetValues(object poco, string propertyName, Type propertyType)
        {
            var node = new Node();
            node.PropertyName = propertyName;

            if (poco == null)
                return null;

            var fullName = propertyType.FullName;
            node.Type = propertyType.Name;
            if (!string.IsNullOrEmpty(fullName) && fullName.Contains("AnonymousType"))
            {
                node.Type = "AnonymousType";
                node.ValueString = poco.ToString();
            }
            else if (propertyType.IsValueType)
            {
                if (!string.IsNullOrEmpty(fullName) && fullName.Contains("System.Collections.Generic.KeyValuePair"))
                {
                    dynamic keyValue = poco;
                    //node.ValueString = keyValue.Key;
                    dynamic value = keyValue.Value;
                    if (value != null)
                    {
                        dynamic child = GetValues(value, keyValue.Key, value.GetType());
                        if (child != null)
                        {
                            node.ValueString = child.ToString();
                            /*var list = node.Childs;
                            if (list == null)
                            {
                                list = new List<Node>();
                                list.Add(child);
                                node.Childs = list;
                            }
                           // list.Add(child);*/
                        }
                    }
                }
                else
                {
                    node.ValueString = poco.ToString();
                }

                return node;
            }
            else if (poco is IDictionary<string, object>)
            {
                if (node.Childs == null)
                    node.Childs = new List<Node>();
                foreach (var keyValuePair in (IDictionary<string, object>) poco)
                {
                    var value = keyValuePair.Value;
                    if (value != null)
                    {
                        var child = GetValues(value, keyValuePair.Key, value.GetType());
                        if (child != null)
                            node.Childs.Add(child);
                    }
                }
                return node;
            }
            else if (propertyType.FullName != null && (propertyType.FullName.Contains("System.")
                                                       && !propertyType.FullName.Contains("System.Collection")
                                                       && !propertyType.FullName.Contains("System.Web")))
            {
                node.ValueString = poco as string;
                return node;
            }
            else if (poco is IEnumerable)
            {
                if (node.Childs == null)
                    node.Childs = new List<Node>();
                var enumerable = ((IEnumerable) poco).GetEnumerator();
                var index = 0;
                while (enumerable.MoveNext())
                {
                    if (enumerable.Current != null)
                    {
                        var child = GetValues(enumerable.Current, index.ToString(CultureInfo.InvariantCulture),
                            enumerable.Current.GetType());
                        if (child != null)
                            node.Childs.Add(child);
                        index += 1;
                    }
                }

                return node;
            }

            var type = poco.GetType();
            var propertyInfos =
                type.GetProperties(BindingFlags.SetField | BindingFlags.Instance | BindingFlags.GetField |
                                   BindingFlags.CreateInstance | BindingFlags.Public);

            if (propertyInfos.Length > 0)
            {
                if (node.Childs == null)
                    node.Childs = new List<Node>();

                foreach (var propertyInfo in propertyInfos)
                {
                    /*try
                    {*/
                    // Si one to one
                    var value = propertyInfo.GetValue(poco, null);
                    var childType = propertyInfo.PropertyType;
                    var child = GetValues(value, propertyInfo.Name, childType);
                    if (child != null)
                        node.Childs.Add(child);
                    /*}
                    catch (Exception ex)
                    {
                        Console.WriteLine( ex.ToString());
                    }*/
                }
            }

            return node;
        }
    }
}