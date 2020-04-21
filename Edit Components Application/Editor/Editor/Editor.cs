﻿/*MIT License

Copyright (c) 2019 Caleb Logan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Editor
{
    /// <summary>
    /// Describes how the uxEditor translates JSONs into a data grid view where the user can edit the file and save.
    /// </summary>
    public partial class uxEditor : Form
    {
        /// <summary>
        /// Auto Generated Code
        /// </summary>
        public uxEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On load we need to import every JSON file used in the project to allow the user to edit the files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Editor_Load(object sender, EventArgs e)
        {
            //The paths to all the jsons used in Grand Theft Democracy
            string pathToEventList = "../application/src/Components/Calendar/EventList.json";
            string pathToSituationsList = "../application/src/Components/Calendar/Situations.json";
            string pathToEchosList = "../application/src/Components/Echo/echo.json";
            string pathToEmailsList = "../application/src/Components/Email/EmailList.json";

            //Loads in the files into the editor

            //First we load in JSONs that have arrays
            LoadInFileArray(pathToEventList, uxEventList);
            LoadInFileArray(pathToSituationsList, uxSituationsList);

            //New load in JSONs that do not use arrays.
            LoadInFileWithoutArray(pathToEchosList, uxEchosList);
            LoadInFileWithoutArray(pathToEmailsList, uxEmailList);

        }


        /// <summary>
        /// When given a path it will load the file into the associated tab.
        /// </summary>
        /// <param name="path">Path to String</param>
        ///  <param name="NameOfTab">Name of the tab that needs the file</param>
        private void LoadInFileWithoutArray(string path, System.Windows.Forms.DataGridView grid)
        {
            using (StreamReader sr = new StreamReader(path)) // Opens the JSON File
            {
                JsonReader jr = new JsonTextReader(sr); // Reads through the JSON File
                string[] pair = new string[2]; //Holds the name of the variable and the value of the variable
                int variableCounts = 0; //The number of variables in the JSON
                string[] newRow = new string[grid.Columns.Count]; //Temporary row that is used to add the data from the file to the Data Grid View
                int cellCount = 0; //Counts the number of cells in the row

                //Reads in the JSON File
                while (jr.Read())
                {
                    if (jr.Value != null)
                    {
                        
                        pair[variableCounts % 2] = jr.Value.ToString(); //Either the variable name or the data in the variable
                        
                        //We look at only the variable data
                        if ((variableCounts % 2) != 0)
                        {
                            
                            //Add the data to the row
                            newRow[cellCount] = pair[1];

                            //Check to see if we are at the end of the row
                            if(cellCount == newRow.Length - 1)
                            {
                                //Reset the cell counter
                                cellCount = 0;

                                //Add row to the data grid view
                                grid.Rows.Add(newRow);
                                
                            }
                            else
                            {
                                //Keep moving through the cells of the row
                                cellCount++;
                            }
                            
                            
                        }
                        //Updates the number of variables in the JSON
                        variableCounts++;
                    }

                }

            }
        }


        /// <summary>
        /// Loads in the file with a JSON array
        /// </summary>
        /// <param name="path"> path to the file </param>
        /// <param name="grid"> Grid associated with the file</param>
        private void LoadInFileArray(string path, System.Windows.Forms.DataGridView grid)
        {
            using (StreamReader sr = new StreamReader(path)) // Opens the JSON File
            {
                JsonReader jr = new JsonTextReader(sr); // Reads through the JSON File
                string[] pair = new string[2]; //Holds the name of the variable and the value of the variable
                int variableCounts = 0; //Counts each variable coming in from the JSON
                string[] newRow = new string[grid.Columns.Count]; //The Row that is being read in from the JSON
                int cellCount = 0; //Counts the number of cells in the row.

                while (jr.Read())
                {
                    if (jr.Value != null)
                    {
                        if (cellCount == 0) //We are reading in the array index.
                        {
                            newRow[cellCount] = jr.Value.ToString(); //Adds the array index to the row
                            cellCount++;
                        }
                        else
                        {
                            pair[(variableCounts) % 2] = jr.Value.ToString(); //This reads in either the variable name or the actual value.

                            //Checks only the value of the variable
                            if (((variableCounts) % 2) != 0)
                            {
                                //Adds the value from the variable to the row.
                                newRow[cellCount] = pair[1];

                                //Checks to see if we have gone through all the variables in the row.
                                if (cellCount == newRow.Length - 1)
                                {
                                    //Reset cell counter
                                    cellCount = 0;
                                    //Add the row to the data grid
                                    grid.Rows.Add(newRow);

                                }
                                else
                                {
                                    //Keep Proceeding through cells
                                    cellCount++;
                                }


                            }
                            //Move on to the next variable to be read in
                            variableCounts++;
                        }
                    }


                }


            }
        }

        /// <summary>
        /// On the click of the Save Button we translate the data grid view into a JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridView grid = uxEventList; //The grid we need to save
            string path = ""; //Holds the path to the file we will edit.

            //We need to find the correct Data grid to save and the correct paths to the file
            switch (uxTabs.SelectedIndex)
            {
                case 0: // Events
                    grid = uxEventList;
                    path = "../application/src/Components/Calendar/EventList.json";
                    break;
                case 1: // Situations
                    grid = uxSituationsList;
                    path = "../application/src/Components/Calendar/Situations.json";
                    break;
                case 2: // Echos
                    grid = uxEchosList;
                    path = "../application/src/Components/Echo/echo.json";
                    break;
                case 3: // Email
                    grid = uxEmailList;
                    path = "../application/src/Components/Email/EmailList.json";
                    break;
            }




            //The string builder and String Writer are used by the JsonWriter
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented; //Formatting for the JSON File

                //All normal JSON files are stored in an array
                if (grid == uxEchosList || grid == uxEmailList)
                {
                    writer.WriteStartArray();
                }
                else
                {
                    //JSON files that are represented as elements of an array start with the index as the first object
                    writer.WriteStartObject();
                }

                //Go through each row in the grid
                for (int rowCount = 0; rowCount < grid.Rows.Count -1; rowCount++)
                {
                    DataGridViewRow row = grid.Rows[rowCount];

                    if (row.Cells.Count > 0)
                    {
                        //Check to see if the grid we are looking at needs to get the array index or is a normal JSON file
                        if ((grid == uxEventList || grid == uxSituationsList))
                        {

                            row.Cells[0].Value = row.Cells[1].Value;
                            writer.WritePropertyName(row.Cells[0].Value.ToString());

                        }
                        
                        //Adds a JSON object
                        writer.WriteStartObject();


                        foreach (DataGridViewCell cell in row.Cells)
                        {

                             //If this is not a JSON file that uses array Index we add the files to the JSON object
                            if (!((grid == uxEventList || grid == uxSituationsList) && cell.ColumnIndex == 0))
                            {

                                writer.WritePropertyName(grid.Columns[cell.ColumnIndex].HeaderText);
                                writer.WriteValue(cell.Value);

                            }

                        }

                        //Finish the creation of the JSON object
                        writer.WriteEndObject();

                    }
                }

                //The array for the JSON has been completed
                if (grid == uxEchosList || grid == uxEmailList)
                {
                    writer.WriteEndArray();
                }

                //Close JSON writer
                writer.Close();
            }

            //Close Stream Writer
            sw.Close();

            //Save the file using the sb that holds the JSON data we created and save it to the file path
            SaveFile(sb, path);
        }


        /// <summary>
        /// When given a String Builder and a path to a JSON file the function will save the data from the String Builder to the JSON file
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="path"></param>
        private void SaveFile(StringBuilder sb, string path)
        {
            using (StreamWriter sw = new StreamWriter(path)) // Opens the JSON File
            {
                //Writes the data to the file
                sw.WriteLine(sb.ToString());
                //Close the file
                sw.Close();
            }

        }

    }

}

