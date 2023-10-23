using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EventGenCMD
{
    class SynLogGen
    {
        List<string> uniqueObjNs = new List<string>();
        List<string> uniqueObjVs = new List<string>();

        List<string[][]> chainEvts = new List<string[][]>();
        List<string[][]> nonChainEvts = new List<string[][]>();

        List<string[][]> finalEvts = new List<string[][]>();

        int sleepWait = 10;

        string dir = @"EventDatasets";

        public SynLogGen(SynParams p)
        {
            //create a pool of unique objects
            generateUniqueObjectVNs(p.totalUniqueObjs);

            //number of non-chain events
            int numOfNonChainEvts = p.totalEvts - (p.numOfChains * p.numOfEvtInChain);
            generateNonChainEvts(numOfNonChainEvts, p.numOfObjsInEvt);

            generateChainEvts(p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts);

            MergeAllEvents(p.repeatChains);



            //WriteToFile(chainEvts, string.Format("Chains_{0}-{1}-{2}-{3}-{4}-{5}.csv",
            //    p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs));

            //WriteToFile_Labels(chainEvts, string.Format("Labelled_Chains_{0}-{1}-{2}-{3}-{4}-{5}.csv",
            //    p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs));



            //WriteToFile(nonChainEvts, string.Format("Noise_{0}-{1}-{2}-{3}-{4}-{5}.csv",
            //    p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs));

            //WriteToFile_Labels(chainEvts, string.Format("Labelled_Noise_{0}-{1}-{2}-{3}-{4}-{5}.csv",
            //    p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs));


            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (finalEvts.Count == (chainEvts.Count * p.repeatChains) + nonChainEvts.Count) 
            { 

                WriteToFile(finalEvts, string.Format(dir + @"\SyntheticEventLogs_{0}-{1}-{2}-{3}-{4}-{5}-{6}.csv",
                    p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs, p.repeatChains));

                WriteToFile_Labels(finalEvts, string.Format(dir + @"\Labelled_SyntheticEventLogs_{0}-{1}-{2}-{3}-{4}-{5}-{6}.csv",
                    p.numOfChains, p.numOfEvtInChain, p.numOfObjsInEvt, p.simBTWEvts, p.totalEvts, p.totalUniqueObjs, p.repeatChains));


            }
            else
                Console.WriteLine(string.Format("Error: {0} is not equal to {1} + {2}", finalEvts.Count, chainEvts.Count, nonChainEvts.Count));

            Console.WriteLine("Log file generated");
        }

        private void generateUniqueObjectVNs(int totalUniqueObjs)
        {
            string templateON = "ObjectName";
            string templateOV = "ObjectValue";

            for (int i = 0; i < totalUniqueObjs; i++)
            {
                uniqueObjNs.Add(templateON + (i + 1));
                uniqueObjVs.Add(templateOV + (i + 1));
            }
        }

        private void generateNonChainEvts(int numOfNonChainEvts, int numOfObjsInEvt)
        {
            DateTime rndDT = DateTime.Now;

            for (int i = 0; i < numOfNonChainEvts; i++)
            {
                //create an event template
                string[][] singleEvtTemplate = new string[numOfObjsInEvt + 3][]; //+2 for event id, timestamp, label

                //add event id
                singleEvtTemplate[0] = new string[2];
                singleEvtTemplate[0][0] = "";
                singleEvtTemplate[0][1] = new Random().Next(1000, 9999).ToString();

                //add timestamp
                singleEvtTemplate[1] = new string[2];
                string[] strDT = rndDT.ToString().Split(' ');
                singleEvtTemplate[1][0] = strDT[0];
                singleEvtTemplate[1][1] = strDT[1];

                //add object-value pairs
                int[] randomONs = generateRandomNumbers(numOfObjsInEvt);
                System.Threading.Thread.Sleep(sleepWait);
                int[] randomOVs = generateRandomNumbers(numOfObjsInEvt);

                for (int j = 2; j < singleEvtTemplate.Length - 1; j++)
                {
                    singleEvtTemplate[j] = new string[2];

                    singleEvtTemplate[j][0] = uniqueObjNs[randomONs[j - 2]];
                    singleEvtTemplate[j][1] = uniqueObjVs[randomOVs[j - 2]];
                }

                singleEvtTemplate[singleEvtTemplate.Length - 1] = new string[2];
                singleEvtTemplate[singleEvtTemplate.Length - 1][0] = "";
                singleEvtTemplate[singleEvtTemplate.Length - 1][1] = 0.ToString();

                //create log entry
                nonChainEvts.Add(singleEvtTemplate);


                //random time in next 5-min window
                rndDT = rndDT.AddMinutes(new Random().NextDouble() * (5));
            }

        }

        private int[] generateRandomNumbers(int count)
        {
            //range 0-uniqueObjNs.lenght
            Random rand = new Random();

            List<int> temp = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int randNum = rand.Next(uniqueObjNs.Count);

                while (temp.Contains(randNum))
                    randNum = rand.Next(uniqueObjNs.Count);

                temp.Add(randNum);
            }

            return temp.ToArray();
        }





        private void generateChainEvts(int numOfChains, int numOfEvtInChain, int numOfObjsInEvt, int simBTWEvts)
        {
            for (int i = 0; i < numOfChains; i++)
            {
                //find similar object-value pairs that will introduce similarity into a chain of events
                double numOfSimilarEvtObjs = numOfObjsInEvt * ((double)simBTWEvts / 100);

                int[] randomONs_sim = generateRandomNumbers((int)numOfSimilarEvtObjs);
                System.Threading.Thread.Sleep(sleepWait);
                int[] randomOVs_sim = generateRandomNumbers((int)numOfSimilarEvtObjs);


                DateTime rndDT = DateTime.Now;


                for (int j = 0; j < numOfEvtInChain; j++)
                {
                    //create an event template
                    string[][] singleEvtTemplate = new string[numOfObjsInEvt + 3][]; //+3 event id, timestamp, label

                    //add event id
                    singleEvtTemplate[0] = new string[2];
                    singleEvtTemplate[0][0] = "";
                    singleEvtTemplate[0][1] = new Random().Next(1000, 9999).ToString();

                    //add timestamp
                    singleEvtTemplate[1] = new string[2];
                    string[] strDT = rndDT.ToString().Split(' ');
                    singleEvtTemplate[1][0] = strDT[0];
                    singleEvtTemplate[1][1] = strDT[1];

                    for (int k = 2; k < randomONs_sim.Length + 2; k++)
                    {
                        singleEvtTemplate[k] = new string[2];

                        singleEvtTemplate[k][0] = uniqueObjNs[randomONs_sim[k - 2]];
                        singleEvtTemplate[k][1] = uniqueObjVs[randomOVs_sim[k - 2]];
                    }

                    //find new, different random object-value pairs, which will be different in a chain of similar objects
                    int numOfNonSimEvtObjs = numOfObjsInEvt - (int)numOfSimilarEvtObjs;
                    int[] randomONs_remaining = generateRandomNumbers((int)numOfNonSimEvtObjs);
                    System.Threading.Thread.Sleep(sleepWait);
                    int[] randomOVs_remaining = generateRandomNumbers((int)numOfNonSimEvtObjs);

                    for (int k = randomONs_sim.Length + 2, l = 0; k < singleEvtTemplate.Length-1; k++, l++)
                    {
                        singleEvtTemplate[k] = new string[2];

                        singleEvtTemplate[k][0] = uniqueObjNs[randomONs_remaining[l]];
                        singleEvtTemplate[k][1] = uniqueObjVs[randomOVs_remaining[l]];
                    }

                    singleEvtTemplate[singleEvtTemplate.Length - 1] = new string[2];
                    singleEvtTemplate[singleEvtTemplate.Length - 1][0] = "";
                    singleEvtTemplate[singleEvtTemplate.Length - 1][1] = (i+1).ToString();

                    //create log entry
                    chainEvts.Add(singleEvtTemplate);


                    //random time in next 5-min window
                    rndDT = rndDT.AddMinutes(new Random().NextDouble() * (new Random().Next(5, 10)));
                }

            }

        }

        private void MergeAllEvents(int repeatTimes)
        {
            for (int k = 0; k < repeatTimes; k++)
                for (int i = 0; i < chainEvts.Count; i++)
                    finalEvts.Add(chainEvts[i]);

            for (int i = 0; i < nonChainEvts.Count; i++)
                finalEvts.Add(nonChainEvts[i]);

            Random rnd = new Random();
            int n = finalEvts.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                string[][] value = finalEvts[k];
                finalEvts[k] = finalEvts[n];
                finalEvts[n] = value;
            }

            finalEvts = finalEvts.OrderBy(x => DateTime.Parse(x[1][0] + " " + x[1][1])).ToList();

        }








        private void WriteToFile(List<string[][]> evts, string fn)
        {
            using (StreamWriter writetext = new StreamWriter(fn))
            {
                for (int i = 0; i < evts.Count; i++)
                {
                    string temp = string.Format("{0}{1},", evts[i][0][0], evts[i][0][1]); //event id without any space
                    temp += string.Format("{0} {1},", evts[i][1][0], evts[i][1][1]); //timestamp with space only
                    for (int j = 2; j < evts[i].Length-1; j++)
                    {
                        temp += string.Format("{0}:{1},", evts[i][j][0], evts[i][j][1]);
                    }

                    writetext.WriteLine(temp.TrimEnd(','));
                }
            }
        }


        private void WriteToFile_Labels(List<string[][]> evts, string fn)
        {
            using (StreamWriter writetext = new StreamWriter(fn))
            {
                for (int i = 0; i < evts.Count; i++)
                {
                    string temp = string.Format("{0}{1},", evts[i][0][0], evts[i][0][1]); //event id without any space
                    temp += string.Format("{0} {1},", evts[i][1][0], evts[i][1][1]); //timestamp with space only
                    for (int j = 2; j < evts[i].Length-1; j++)
                    {
                        temp += string.Format("{0}:{1},", evts[i][j][0], evts[i][j][1]);
                    }

                    temp += string.Format("{0},", evts[i][(evts[i].Length - 1)][1]);

                    writetext.WriteLine(temp.TrimEnd(','));
                }
            }
        }
    }
}
