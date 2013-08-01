using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MoCommon
{
   public class DataSerializerHelper
    {

       public T XmlDeserialize<T>(string xmlstring,string rootName)
       {
           //spilt the \r\n string 
           string converStr = string.Empty;
           string[] spiltArray = xmlstring.Split(new char[] {'\r','\n'});
           if (spiltArray.Length > 0)
           {
               for (int count = 0; count < spiltArray.Length; count++)
               {
                   if(!string.IsNullOrEmpty(spiltArray[count]))
                       converStr += spiltArray[count];
               }
           }

           //add root element name
           XmlRootAttribute xRoot = new XmlRootAttribute();
           xRoot.ElementName = rootName;//"supd";
           xRoot.IsNullable = true;

           //deserializer operator
           T t = default(T);
           XmlSerializer xmlSerializer = new XmlSerializer(typeof(T),xRoot);
           using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(converStr)))
           {
               using (XmlReader xmlReader = XmlReader.Create(xmlStream))
               {
                   Object obj = xmlSerializer.Deserialize(xmlReader);
                   t = (T)obj;
               }
           }
           return t;
       }
    }
}
