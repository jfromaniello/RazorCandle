using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace RazorCandle
{
    public static class Extensions
    {

        public static ExpandoObject ToExpando(this IDictionary<string, JToken> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, JToken>)
                {
                    var expandoValue = ((IDictionary<string, JToken>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, JToken>)
                        {
                            var expandoItem = ((IDictionary<string, JToken>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(new KeyValuePair<string, object>(kvp.Key, kvp.Value));
                }
            }

            return expando;
        }
    }
}