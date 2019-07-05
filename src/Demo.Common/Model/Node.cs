using System.Collections.Generic;

namespace Demo.Common.Model
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
}