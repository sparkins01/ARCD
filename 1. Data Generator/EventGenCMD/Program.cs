using System;

namespace EventGenCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            SynParams p = new SynParams();
            p.numOfChains = int.Parse(args[0]);
            p.numOfEvtInChain = int.Parse(args[1]);
            p.numOfObjsInEvt = int.Parse(args[2]);
            p.simBTWEvts = int.Parse(args[3]);
            p.totalEvts = int.Parse(args[4]);
            p.totalUniqueObjs = int.Parse(args[5]);
            p.repeatChains = int.Parse(args[6]);

            new SynLogGen(p);
        }

    }
}
