using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventToAction
{
    public partial class Form1 : Form
    {
        DataTable table;
        DataTable[] actionTables;
        int[] labels = null;
        string[][] data = null;
        bool actionsloaded = false;

        List<string> pddlTypes;
        List<string> pddlObjects;
        List<string> pddlPredicates;
        List<string> pddlActions;
        List<string> pddlInit;
        List<string> pddlGoal;
        string plannnerfolder;
        public Form1()
        {
            InitializeComponent();

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedIndex == 2) && (!actionsloaded))
            { //changing to action builder
                listBox1.Items.Clear();

                if (labels != null)
                {
                    //get all unique labels
                    int[] clusters = labels.Distinct().ToArray();
                    actionTables = new DataTable[clusters.Length];
                    int ai = 0;
                    foreach (int cls in clusters)
                    {
                        listBox1.Items.Add(cls);
                        actionTables[ai] = new DataTable();
                        SetTableAction(ai);
                        int c = 0;
                        //create list of relevant events
                        for (int i = 0; i < labels.Length - 1; i++)
                        {
                            if (labels[i] == cls)
                            {
                                actionTables[ai].Rows.Add();
                                for (int j = 0; j < data[i].Length; j++)
                                {
                                    actionTables[ai].Rows[c][j] = data[i][j];
                                }
                                c++;
                            }

                        }

                        ai++;
                    }
                }
                for (int i = 0; i < actionTables.Length; i++)
                {
                    //remove duplicates
                    DataView dv = actionTables[i].DefaultView;
                    actionTables[i] = dv.ToTable(true);
                }
                actionsloaded = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SynParams p = new SynParams();

            p.numOfChains = int.Parse(textBox1.Text);
            p.numOfEvtInChain = int.Parse(textBox2.Text);
            p.numOfObjsInEvt = int.Parse(textBox3.Text);
            p.simBTWEvts = int.Parse(textBox4.Text);
            p.totalEvts = int.Parse(textBox5.Text);
            p.totalUniqueObjs = int.Parse(textBox6.Text);
            p.repeatChains = int.Parse(textBox7.Text);

            string file = string.Format(textBoxDataDir.Text + @"\Numeric_{0}-{1}-{2}-{3}-{4}-{5}-{6}.csv",
                p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs, p.repeatChains);

            Cluster cluster = new Cluster(file);
            labels = cluster.PerformClustering();


            //handle UI
            data = LoadCsvIncludingHeader(string.Format(textBoxDataDir.Text + @"\SyntheticEventLogs_{0}-{1}-{2}-{3}-{4}-{5}-{6}.csv",
                p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs, p.repeatChains));
            table = GetTable(data);
            dataGridView1.DataSource = table;
            clear();
            updateTable(data);
            updateTableClusteringResults(data, labels);
            dataGridView1.AutoResizeColumns();

            actionsloaded = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SynParams p = new SynParams();

            p.numOfChains = int.Parse(textBox1.Text);
            p.numOfEvtInChain = int.Parse(textBox2.Text);
            p.numOfObjsInEvt = int.Parse(textBox3.Text);
            p.simBTWEvts = int.Parse(textBox4.Text);
            p.totalEvts = int.Parse(textBox5.Text);
            p.totalUniqueObjs = int.Parse(textBox6.Text);
            p.repeatChains = int.Parse(textBox7.Text);

            SynLogGen slg = new SynLogGen(p, textBoxDataDir.Text);
            string fn = slg.returnFN();

            string outputFilePath = string.Format(textBoxDataDir.Text + @"\Numeric_{0}-{1}-{2}-{3}-{4}-{5}-{6}.csv",
                p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs, p.repeatChains);

            ReadAndSplit rs = new ReadAndSplit(fn, //labelled
                                               fn.Replace("Labelled_", ""), //unlabelled
                                               outputFilePath);
            rs.enummerateAllObjects();
            rs.prepareDT();

            MessageBox.Show("Log file generated at: " + outputFilePath);


        }

        public DataTable GetTable(string[][] data)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < data[0].Length; i++) //process header
            {
                table.Columns.Add("Column" + i);
            }

            return table;
        }

        public void SetTableAction(int iT)
        {
            //actionTables[iT];
            //  DataTable table = (DataTable )dataGridView2.DataSource;
            for (int i = 0; i < data[0].Length; i++) //process header
            {
                actionTables[iT].Columns.Add("Column" + i);
            }
            DataColumn colum = actionTables[iT].Columns.Add("Precondition");
            colum.DataType = System.Type.GetType("System.Boolean");
            //        return table;
        }

        public void updateTable(string[][] data)
        {

            for (int i = 1; i < data.Length; i++)
            {
                table.Rows.Add();
                for (int j = 0; j < data[i].Length; j++)
                {
                    table.Rows[i - 1][j] = data[i][j];
                }
            }
        }


        public void updateTableClusteringResults(string[][] data, int[] labels)
        {
            //add new column
            table.Columns.Add("Cluster");
            for (int i = 0; i < labels.Length - 1; i++)
            {
                table.Rows[i][data[i].Length] = labels[i];

            }
        }


        private void clear()
        {
            table.Clear();

        }

        public string[][] LoadCsvIncludingHeader(string filePath)
        {
            string[][] toReturn = new string[0][];
            if (File.Exists(filePath))
            {
                string[] readText = File.ReadAllLines(filePath);
                toReturn = new string[readText.Length][];
                for (int i = 0; i < readText.Length; i++)
                {
                    String[] line = readText[i].Split(',');
                    toReturn[i] = new string[line.Length];
                    for (int j = 0; j < line.Length; j++)
                    {
                        toReturn[i][j] = line[j];

                    }
                }
            }
            return toReturn;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedNum = listBox1.SelectedItem.ToString();
            int n = int.Parse(selectedNum);
            label11.Text = selectedNum;
            dataGridView2.DataSource = actionTables[listBox1.SelectedIndex];
            //  SetTableAction(listBox1.SelectedIndex);

            /*    DataTable table = (DataTable)dataGridView2.DataSource;
                int c = 0;
                //create list of relevant events
                for (int i = 0; i < labels.Length-1; i++)
                {
                    if(labels[i] == n)
                    {
                        table.Rows.Add();
                        for (int j = 0; j < data[i].Length; j++)
                        {
                            table.Rows[c][j] = data[i][j];
                        }
                        c++;
                    }

                }
            */

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //*********DOMAIN FILE**********

            pddlTypes = new List<string>();
            pddlPredicates = new List<string>();
            pddlActions = new List<string>();
            pddlObjects = new List<string>();
            pddlInit = new List<string>();
            pddlGoal = new List<string>();
            //only one type of object
            pddlTypes.Add("(:types");
            pddlTypes.Add("  eo - object ");
            pddlTypes.Add(")");

            pddlPredicates.Add("(:predicates");


            //current action table
            int t = listBox1.SelectedIndex;
            //  int n = int.Parse(selectedNum);
            //assume first column is ID
            for (int n = 0; n < listBox1.Items.Count; n++)
            {
                pddlActions.AddRange(PDDLActionBuilder(n));
            }
            pddlPredicates.Add(")");

            //write PDDL Domain
            List<string> PDDLDomain = new List<string>();
            PDDLDomain.Add("(define(domain event)");
            PDDLDomain.Add("(:requirements :strips :fluents :typing)");
            PDDLDomain.AddRange(pddlTypes);
            PDDLDomain.AddRange(pddlPredicates.Distinct());
            PDDLDomain.AddRange(pddlActions);
            PDDLDomain.Add(")\n");
            richTextBox1.AppendText(String.Join("\n", PDDLDomain.ToArray()));
            System.IO.File.WriteAllLines(textBoxDataDir.Text + @"\planner\edomain.pddl", PDDLDomain);


            //*********PROBLEM FILE**********
            List<string> PDDLProblem = new List<string>();
            PDDLProblem.Add("(define(problem event)");
            PDDLProblem.Add("(:domain event)");
            PDDLProblem.Add("(:objects");
            PDDLProblem.Add(String.Join(" ", pddlObjects.Distinct().ToArray()) + " -eo");
            PDDLProblem.Add(")");
            PDDLProblem.Add("(:init");
            PDDLProblem.Add(String.Join("\n", pddlInit.Distinct().ToArray()));
            PDDLProblem.Add(")");
            PDDLProblem.Add("(:goal");
            PDDLProblem.Add("(and");
            PDDLProblem.Add(String.Join("\n", pddlGoal.Distinct().ToArray()));
            PDDLProblem.Add(")");
            PDDLProblem.Add(")");
            PDDLProblem.Add(")");
            richTextBox1.AppendText(String.Join("\n", PDDLProblem.ToArray()));
            System.IO.File.WriteAllLines(textBoxDataDir.Text + @"\planner\eprob.pddl", PDDLProblem);
        }



        public List<string> PDDLActionBuilder(int action_num)
        {
            List<string> action = new List<string>();
            string actionName = "action_";
            string preString = "";
            string effString = "";
            List<object[]> precond = new List<object[]>();
            List<object[]> effect = new List<object[]>();
            List<string> para = new List<string>();

            //Split into pre and effect arrays.
            foreach (DataRow row in actionTables[action_num].Rows)
            {
                bool pre = false;
                try
                {
                    pre = (Boolean)row["Precondition"];
                }
                catch
                {

                }
                if (pre)
                {
                    precond.Add(row.ItemArray.ToArray());
                }
                else
                {
                    effect.Add(row.ItemArray.ToArray());

                }
            }


            //only create action if there are both preconditions and effects
            if ((precond.Count > 0) && (effect.Count > 0))
            {
                foreach (object[] pre in precond)
                {

                    for (int i = 0; i < pre.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            actionName += pre[i].ToString() + "_";
                        }
                        else if (i == 1) { } //datetime needs to be ignored
                        else
                        {
                            string[] keyval = pre[i].ToString().Split(':');
                            preString += " (" + keyval[0] + " ?" + keyval[1] + ")";
                            pddlInit.Add(" (" + keyval[0] + " " + keyval[1] + ")");
                            para.Add(" ?" + keyval[1]);
                            pddlPredicates.Add("(" + keyval[0] + " ?eo)");
                            pddlObjects.Add(keyval[1]);

                        }
                    }
                }
                foreach (object[] eff in effect)
                {
                    for (int i = 0; i < eff.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            actionName += "_" + eff[i].ToString();
                        }
                        else if (i == 1) { } //datetime needs to be ignored
                        else
                        {
                            string[] keyval = eff[i].ToString().Split(':');
                            effString += " (" + keyval[0] + " ?" + keyval[1] + ")";
                            pddlGoal.Add(" (" + keyval[0] + " " + keyval[1] + ")");
                            para.Add(" ?" + keyval[1]);
                            pddlPredicates.Add("(" + keyval[0] + " ?eo)");
                            pddlObjects.Add(keyval[1]);
                        }
                    }
                }
                action.Add("");
                action.Add(";;**********" + actionName + "*************");
                action.Add("(:action " + actionName);
                action.Add(":parameters (" + String.Join(" - eo", para.Distinct().ToArray()) + ")");
                action.Add(":precondition");
                action.Add("(and ");
                action.Add(preString);
                action.Add(")");
                action.Add(":effect  ");
                action.Add("(and");
                action.Add(effString + "");
                action.Add(")");
                action.Add(")");
                //  Console.WriteLine(actionName);
            }
            return action;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            plannnerfolder = textBoxDataDir.Text + @"\planner\";
            deletePDDLOutputFiles();



            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            // startInfo.CreateNoWindow = true;
            startInfo.FileName = plannnerfolder + "lpg.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = @"-o " + plannnerfolder + "edomain.pddl -f " + plannnerfolder + "eprob.pddl -n 1 -cputime 60 -out " + plannnerfolder;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
            string[] files = System.IO.Directory.GetFiles(plannnerfolder, "*.SOL");



            if (files.Length > 0)
            {
                if (File.Exists(files[files.Length - 1]))
                {
                    List<string> lines = File.ReadAllLines(files[files.Length - 1]).ToList();
                    planList.Items.Clear();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (Regex.IsMatch(lines[i], @"^\d+:"))
                            planList.Items.Add(lines[i].ToString());
                    }
                }
            }

        }

        private void deletePDDLOutputFiles()
        {
            string[] files = System.IO.Directory.GetFiles(plannnerfolder, " *.SOL");
            foreach (string s in files)
            {
                System.IO.File.Delete(s);
            }

        }
    }

}
