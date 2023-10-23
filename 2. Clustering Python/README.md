This Python code was used during the experimental part of this project. 

The first file 'All Algorithms.py' was used to benchmark 5 algorihtms (Optics_RI, DBSCAN_RI, Mean Shift_RI, Affinity_RI, Agglomerative_RI) against a small subset of data generated using the '1.Data Generator' code.

The second file 'DBScan.py' uses only the DBScan algorithm on the full set of benchmarking data (generated using 'EvtGenScript.BAT' in '1.Data Generator') to establish how well it can detect event-chains. The Adjusted Rand Index (ARI) is used to calculate the match and the full results are available in Paper 2.
