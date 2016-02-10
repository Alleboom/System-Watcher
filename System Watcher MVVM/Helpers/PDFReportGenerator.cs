using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace System_Watcher_MVVM.Helpers
{
    public static class PDFReportGenerator
    {

        /// <summary>
        /// Generates a pdf report based off of one object type being passed in and a collection of properties of that object type
        /// I wouldn't reccomend using more than 4 properties for a clean report
        /// 
        /// </summary>
        /// <typeparam name="T">The object we want to generate the report on</typeparam>
        /// <param name="collection">The actuall collection we want to iterate against</param>
        /// <param name="filePath">The file path that we want to save this report to</param>
        /// <param name="HeaderText">The title of the pdf report</param>
        /// <param name="PropertyNames">The properties we want to include in the report. The key should be the property name and the value will be the String we will display on the report.</param>
        public static void GenerateReport<T>(IEnumerable<T> collection, String filePath, String HeaderText,  Dictionary<String,String> PropertyNames) 
        {
            try
            {
                Document doc = CreateDocument();

                // One primary section
                Section DataTableSection = doc.AddSection();

                // Apply our title
                AddTitle(HeaderText, DataTableSection);

                // Add a table
                Table data;
                List<Column> Columns;
                AddTable(DataTableSection, out data, out Columns);

                // Creates our columns with an equal sized width
                CreateColumns(PropertyNames, data, Columns, doc);

                // Apply our headers
                ApplyColumnHeaders(PropertyNames, data);

                // Grab all the properties
                ApplyDataToRows<T>(collection, PropertyNames, data);

                CreateFile(filePath, doc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        /// <summary>
        /// Creates the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="doc"></param>
        private static void CreateFile(String filePath, Document doc)
        {

            const bool unicode = false;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // Create a renderer for the MigraDoc document
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = doc;

            // Layout and render the docuemnt to pdf
            pdfRenderer.RenderDocument();

            //Save the document
            pdfRenderer.PdfDocument.Save(filePath);
            Process.Start(filePath);
        }

        /// <summary>
        /// Applies our data to a new row for each object in our collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="PropertyNames"></param>
        /// <param name="data"></param>
        private static void ApplyDataToRows<T>(IEnumerable<T> collection, Dictionary<String,String> PropertyNames, Table data)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {

                    List<PropertyInfo> _props = new List<PropertyInfo>();
                    var row = data.AddRow();

                    foreach (var prop_name in PropertyNames)
                    {
                        var _propinfo = item.GetType().GetProperty(prop_name.Key);
                        if (_propinfo != null)
                        {
                            _props.Add(_propinfo);
                        }
                    }

                    int i = 0;
                    foreach (PropertyInfo prop in _props)
                    {
                        var cell = row.Cells[i];
                        if (prop.GetValue(item, null) != null)
                        {
                            cell.Format.Alignment = ParagraphAlignment.Center;
                            cell.Format.Font.Size = 10;
                            var text = cell.AddParagraph(prop.GetValue(item, null).ToString());
                        }
                        i++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Null list receieved");
            }
        }

        /// <summary>
        /// Adds the title
        /// </summary>
        /// <param name="HeaderText"></param>
        /// <param name="DataTableSection"></param>
        private static void AddTitle(String HeaderText, Section DataTableSection)
        {
            var Title = DataTableSection.AddParagraph(HeaderText);
            Title.Format.Font.Size = 30;
            Title.Format.Borders.Visible = true;
            Title.Format.Alignment = ParagraphAlignment.Center;
        }

        /// <summary>
        /// Generate the value needed to even space the columns
        /// </summary>
        /// <param name="PropertyNames"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static float GetColumnWidth(Dictionary<String,String> PropertyNames, Document doc)
        {
            float sectionWidth = doc.DefaultPageSetup.PageHeight  - doc.DefaultPageSetup.BottomMargin*2 - doc.DefaultPageSetup.TopMargin*2;
            float columnWidth = sectionWidth / PropertyNames.Count;
            return columnWidth;
        }
        
        /// <summary>
        /// Adds a table to the doc
        /// </summary>
        /// <param name="DataTableSection"></param>
        /// <param name="data"></param>
        /// <param name="Columns"></param>
        private static void AddTable(Section DataTableSection, out Table data, out List<Column> Columns)
        {
            // Add our table to the doc on the body section
            data = DataTableSection.AddTable();
            DataTableSection.PageSetup.PageFormat = PageFormat.Letter;
            Columns = new List<Column>();
        }

        /// <summary>
        /// Creates our columns
        /// </summary>
        /// <param name="PropertyNames"></param>
        /// <param name="data"></param>
        /// <param name="Columns"></param>
        /// <param name="columnWidth"></param>
        private static void CreateColumns(Dictionary<String, String> PropertyNames, Table data, List<Column> Columns, Document doc)
        {
            // Get our column width
            float columnWidth = GetColumnWidth(PropertyNames, doc);
            // create our columns
            foreach (var property in PropertyNames)
            {
                Column _col = data.AddColumn();
                Columns.Add(_col);
                _col.Format.Alignment = ParagraphAlignment.Center;
                _col.Format.Font.Size = 20;
                _col.Width = (int)columnWidth;
                _col.HeadingFormat = true;
            }
        }

        /// <summary>
        /// Applies our column headers to the document
        /// </summary>
        /// <param name="PropertyNames"></param>
        /// <param name="data"></param>
        private static void ApplyColumnHeaders(Dictionary<String,String> PropertyNames, Table data)
        {
            // Apply our column headers
            var _row = data.AddRow();
            _row.HeadingFormat = true;
            int c = 0;
            foreach (var property in PropertyNames)
            {
                var colhead = _row.Cells[c].AddParagraph(property.Value);
                colhead.Format.Font.Bold = true;
                c++;
            }
        }

        /// <summary>
        /// Creates the document for us. Any Document properties should be changed in here
        /// </summary>
        /// <returns>The document with all the properties we are looking for</returns>
        private static Document CreateDocument()
        {
            // Create a document with two sections, one header and one for the grid
            Document doc = new Document();

            doc.DefaultPageSetup.PageFormat = PageFormat.Letter;
            doc.DefaultPageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            return doc;
        }

    }
}
