using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace EventToAction
{
    class ReadAndSplit
    {
        string[] unlabelledDataset = null, labelledDataset = null;

        List<string> allObjs = new List<string>();
        List<string> allVals = new List<string>();

        string outputFilename = "";

        public ReadAndSplit(string labelled, string unlabelled, string outputFN)
        {
            labelledDataset = File.ReadAllLines(labelled);
            unlabelledDataset = File.ReadAllLines(unlabelled);

            outputFilename = outputFN;
        }

        public void enummerateAllObjects()
        {
            foreach (string line in unlabelledDataset)
            {
                string[] lineToks = line.Split(',').Skip(2).ToArray();
                foreach (string lineTok in lineToks)
                {
                    string[] objVal = lineTok.Split(':');
                    allObjs.Add(objVal[0]);
                    allVals.Add(objVal[1]);
                }
            }

            allObjs = allObjs.Distinct().ToList();
            allVals = allVals.Distinct().ToList();
        }

        public void prepareDT()
        {

            using (StreamWriter writer = new StreamWriter(@outputFilename))
            {
                writer.WriteLine(string.Format("no.,{0},label", string.Join(',', allObjs)));

                int counter = 1;

                for (int k = 0; k < unlabelledDataset.Length; k++)
                {
                    string[] lineToks = unlabelledDataset[k].Split(',').Skip(2).ToArray();

                    List<string[]> temp = new List<string[]>();
                    foreach (string lineTok in lineToks)
                        temp.Add(lineTok.Split(':'));

                    //temp[i][0] = obj name and temp[i][1] = obj value

                    int[] valueOrNot = new int[allObjs.Count];
                    for (int i = 0; i < valueOrNot.Length; i++) valueOrNot[i] = 0;

                    for (int i = 0; i < temp.Count; i++)
                    {
                        int pos = allObjs.FindIndex(a => a.Trim().Equals(temp[i][0].Trim()));
                        int val = allVals.FindIndex(a => a.Trim().Equals(temp[i][1].Trim()));

                        valueOrNot[pos] = val;

                    }

                    string[] labelledEvtToks = labelledDataset[k].Split(',');

                    writer.WriteLine(string.Format("{0},{1},{2}", (counter++),
                                                                    string.Join(',', valueOrNot),
                                                                    labelledEvtToks.Last()));


                }
            }
        }
    }
}
