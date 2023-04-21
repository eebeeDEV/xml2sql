using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Xml2SqlImport.Data.XmlModels;
using Xml2SqlImport.Interfaces;

namespace Xml2SqlImport.Services
{
    internal class XsdService : IXsdService
    {
        public bool readXsdSchemas(string fileName)
        {


            var xsdPath = Path.Combine("C:\\ProgramData\\Internal\\Repos", "Files", fileName);


            XmlSchemaSet schemaSet = new XmlSchemaSet();
            //schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
            schemaSet.Add("http://www.tempuri.org", xsdPath);
            schemaSet.Compile();

            // Retrieve the compiled XmlSchema object from the XmlSchemaSet
            // by iterating over the Schemas property.
            XmlSchema? schema;
            foreach (XmlSchema customerSchema in schemaSet.Schemas())
            {
                schema = customerSchema;



                // Load the XSD _file into an XmlSchema object
                //XmlSchema? schema = XmlSchema.Read(new XmlTextReader(xsdPath), null);


                if (schema != null)
                {

                    // Create a dictionary to store the tables and fields
                    Dictionary<string, List<XmlTableField>> tables = new Dictionary<string, List<XmlTableField>>();

                    // Iterate over the schema's global elements
                    foreach (XmlSchemaElement element in schema.Elements.Values)
                    {
                        // Get the element's type
                        XmlSchemaType? type = element.ElementSchemaType;

                        // If the type is a complex type
                        if (type is XmlSchemaComplexType)
                        {
                            _readComplexType("",element, type, tables);
                        }
                    }

                    // Iterate over the tables in the dictionary
                    foreach (KeyValuePair<string, List<XmlTableField>> table in tables)
                    {
                        Console.WriteLine("Table: " + table.Key);
                        Console.WriteLine("Fields: ");
                        foreach (XmlTableField field in table.Value)
                        {
                            Console.WriteLine(" - " + field.fieldName + ": " + field.fieldType);
                        }
                    }
                }
            }

            return true;

        }



        void _readComplexType(string parentName,XmlSchemaElement element, XmlSchemaType type, Dictionary<string, List<XmlTableField>> tables)
        {
            // Get the complex type
            XmlSchemaComplexType complexType = (XmlSchemaComplexType)type;

            // Get the complex type's sequence (if it has one)
            XmlSchemaSequence? sequence = complexType.Particle as XmlSchemaSequence;

            // If the complex type has a sequence
            if (sequence != null)
            {
                // Create a list to store the fields for this table
                List<XmlTableField> fields = new List<XmlTableField>();

                // Iterate over the sequence's items
                foreach (XmlSchemaElement field in sequence.Items)
                {
                    // Get the element's type
                    XmlSchemaType? fType = field.ElementSchemaType;

                    // Get the parent element in case of same table name
                    //XmlSchemaSequence? parent = field.;
                    //var i = parent.i

                    if (fType is XmlSchemaComplexType)
                    {
                        _readComplexType($"{parentName}_{element.Name!}", field, fType, tables);
                    } else
                    {
                        XmlSchemaType tpe = field.ElementSchemaType!;
                        
                        // Add the field to the list
                        var fld = new XmlTableField { fieldName = field.Name!, fieldType = tpe!.TypeCode.ToString() };
                        fields.Add(fld);
                    }

                }

                // Add the table and fields to the dictionary
                tables.Add($"{parentName}_{element.Name!}", fields);
            }
        }
    }
}
