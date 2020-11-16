using Jitbit.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace InterLink_Prob
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string PATH = $"{Environment.CurrentDirectory}\\Final File.csv";
        CsvExport csvExport = new CsvExport();
        List<string> inputNames = new List<string> { };
        List<string> inputDates = new List<string> { };
        List<double> hours = new List<double> { };
        List<string> names = new List<string> { };
        List<string> dates = new List<string> { };

        private void Form1_Load(object sender, EventArgs e)
        {
            readDoc();
            names = inputNames.Distinct().ToList();
            dates = inputDates.Distinct().ToList();
            writeDataInFile();
            csvExport.ExportToFile(PATH);
        }

        private void setUpDataGrid()
        {
            DataGridViewTextBoxColumn dtColumn = new DataGridViewTextBoxColumn();
            dtColumn.Name = "Name / Date";
            dtColumn.HeaderText = "Name / Date";
            dataGridView1.Columns.Add(dtColumn);

            foreach (var date in dates)
            {
                DataGridViewTextBoxColumn dColumn = new DataGridViewTextBoxColumn();
                dColumn.Name = $"Date {date}";
                dColumn.HeaderText = date;
                dataGridView1.Columns.Add(dColumn);
            }
        }

        private void writeDataInFile()
        {
            setUpDataGrid();

            for (int i = 0; i < names.Count; i++)
            {
                dataGridView1.Rows.Add();
                csvExport.AddRow();
                csvExport["Name / Date"] = names[i];
                dataGridView1.Rows[i].Cells[0].Value = names[i];

                for (int j = 0; j < dates.Count; j++)
                {
                    var info = foundHours_byFile(names[i], dates[j]);
                    csvExport[dates[j]] = info;
                    dataGridView1.Rows[i].Cells[j + 1].Value = info;
                }
            }
        }

        private double foundHours_byFile(string name, string date)
        {
            for (int i = 0; i < inputNames.Count; i++)
            {
                if (inputNames[i] == name && inputDates[i] == date) return hours[i];
            }

            return 0;
        }

        private string remakeDate(string date)
        {
            var data = date.Split();
            int month = 0;
            switch (data[0])
            {
                case "Jan":
                    month = 1;
                    break;
                case "Feb":
                    month = 2;
                    break;
                case "Mar":
                    month = 3;
                    break;
                case "Apr":
                    month = 4;
                    break;
                case "May":
                    month = 5;
                    break;
                case "Jun":
                    month = 6;
                    break;
                case "Jul":
                    month = 7;
                    break;
                case "Aug":
                    month = 8;
                    break;
                case "Sep":
                    month = 9;
                    break;
                case "Oct":
                    month = 10;
                    break;
                case "Nov":
                    month = 11;
                    break;
                case "Dec":
                    month = 12;
                    break;
            }

            return $"{data[2]}-{string.Format("{0:00}", month)}-{data[1]}";
        }

        private void readDoc()
        {
            string[] lines = File.ReadAllLines($"{Environment.CurrentDirectory}\\acme_worksheet.csv");

            for (int i = 1; i < lines.Length; i++)
            {
                var list = lines[i].Split(',').ToList<string>();

                inputNames.Add(list[0]);
                inputDates.Add(remakeDate(list[1]));
                hours.Add(double.Parse(list[2]));
            }
        }
    }
}
