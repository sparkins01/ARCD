This C# application provides a parameterised event dataset generation with the inclusion of synthetic event chains. An event chain is a group of events sharing common object occurrence.

Once compiled, the application takes the following 7 command line inputs:

1. Number of chains
2. Number of events in each chain
3. Number of objects in each event
4. Similarity between objects in events (0-100%)
5. Total events available to the generation process
6. Total unique objects available to the generation process
7. Number of times to repeat a chain

A full description of the parameters is available in Paper 2 in the 'Research Papers' folder.

In the Debug\netcoreapp3.1 folder, there is the EvtGenScript.BAT which will generate all the datafiles used for benchmarking purposes in Paper 2.
