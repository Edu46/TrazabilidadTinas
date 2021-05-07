﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

namespace LogisticaERP.Clases
{
    /// <summary>
    /// Extensiones personalizadas.
    /// </summary>
    public static class Extensiones
    {
        /// <summary>
        /// Obtiene la descripción almacenada en el atributo DescriptionAttribute. 
        /// </summary>
        /// <param name="value">El texto de la descripción.</param>
        /// <returns>La descripción almacenada en el atributo DescriptionAttribute.</returns>
        public static string Descripcion(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Obtiene la descripción almacenada en el atributo EnumMemberAttribute. 
        /// </summary>
        /// <param name="value">El texto de la descripción.</param>
        /// <returns>La descripción almacenada en el atributo EnumMemberAttribute.</returns>
        public static string EnumValue(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (EnumMemberAttribute[])fi.GetCustomAttributes(typeof(EnumMemberAttribute), false);
            return (attributes.Length > 0 && attributes[0].Value != null) ? attributes[0].Value : value.ToString();
        }

        ///###############################################################
        /// <summary>
        /// Convert a List to a DataTable.
        /// </summary>
        /// <remarks>
        /// Based on MIT-licensed code presented at http://www.chinhdo.com/20090402/convert-list-to-datatable/ as "ToDataTable"
        /// <para/>Code modifications made by Nick Campbell.
        /// <para/>Source code provided on this web site (chinhdo.com) is under the MIT license.
        /// <para/>Copyright © 2010 Chinh Do
        /// <para/>Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
        /// <para/>The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
        /// <para/>THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
        /// <para/>(As per http://www.chinhdo.com/20080825/transactional-file-manager/)
        /// </remarks>
        /// <typeparam name="T">Type representing the type to convert.</typeparam>
        /// <param name="l_oItems">List of requested type representing the values to convert.</param>
        /// <returns></returns>
        ///###############################################################
        /// <LastUpdated>February 15, 2010</LastUpdated>
        public static DataTable ToDataTable<T>(this List<T> l_oItems)
        {
            DataTable oReturn = new DataTable(typeof(T).Name);
            object[] a_oValues;
            int i;

            //#### Collect the a_oProperties for the passed T
            PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //#### Traverse each oProperty, .Add'ing each .Name/.BaseType into our oReturn value
            //####     NOTE: The call to .BaseType is required as DataTables/DataSets do not support nullable types, so it's non-nullable counterpart Type is required in the .Column definition
            foreach (PropertyInfo oProperty in a_oProperties)
            {
                oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
            }

            //#### Traverse the l_oItems
            foreach (T oItem in l_oItems)
            {
                //#### Collect the a_oValues for this loop
                a_oValues = new object[a_oProperties.Length];

                //#### Traverse the a_oProperties, populating each a_oValues as we go
                for (i = 0; i < a_oProperties.Length; i++)
                {
                    a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                }

                //#### .Add the .Row that represents the current a_oValues into our oReturn value
                oReturn.Rows.Add(a_oValues);
            }

            //#### Return the above determined oReturn value to the caller
            return oReturn;
        }

        ///###############################################################
        /// <summary>
        /// Returns the underlying/base type of nullable types.
        /// </summary>
        /// <remarks>
        /// Based on MIT-licensed code presented at http://www.chinhdo.com/20090402/convert-list-to-datatable/ as "GetCoreType"
        /// <para/>Code modifications made by Nick Campbell.
        /// <para/>Source code provided on this web site (chinhdo.com) is under the MIT license.
        /// <para/>Copyright © 2010 Chinh Do
        /// <para/>Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
        /// <para/>The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
        /// <para/>THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
        /// <para/>(As per http://www.chinhdo.com/20080825/transactional-file-manager/)
        /// </remarks>
        /// <param name="oType">Type representing the type to query.</param>
        /// <returns>Type representing the underlying/base type.</returns>
        ///###############################################################
        /// <LastUpdated>February 15, 2010</LastUpdated>
        public static Type BaseType(Type oType)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                return oType;
            }
        }

        public delegate bool TryParseDelegate<T>(string str, out T value);

        public static T? TryParseAs<T>(this string str, TryParseDelegate<T> parse) where T : struct
        {
            T value;
            return parse(str, out value) ? value : (T?)null;
        }

        public static string Base64Encode(this Stream stream)
        {
            BinaryReader br = new System.IO.BinaryReader(stream);
            Byte[] bytes = br.ReadBytes((Int32)stream.Length);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string SerializeToXML<T>(this T value)
        {
            if (value == null)
                return string.Empty;

            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new Utf8StringWriter();

                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("An error occurred", exception);
            }
        }


        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
    }
}
