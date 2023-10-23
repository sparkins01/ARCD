# -*- coding: utf-8 -*-
"""
Created on Tue Oct 17 10:54:52 2023

@author: SCOMMR5
"""

# -*- coding: utf-8 -*-
"""
Created on Wed Apr 19 14:39:13 2023

# -*- coding: utf-8 -*-
"""


import pandas as pd
from sklearn.cluster import OPTICS, cluster_optics_dbscan
from sklearn import cluster
import glob
import os

from sklearn.neighbors import kneighbors_graph
from sklearn.metrics import rand_score
import time
import csv
from sklearn.cluster import AgglomerativeClustering



path = os.getcwd()
csv_files = glob.glob(os.path.join(path, "objects/*.*"))


with open('multi_time.csv', 'w', newline='' ) as csvfile: # creates multi_time.csv for storing time taken by algorithm to process one file
    writer=csv.writer(csvfile)
    writer.writerow(['filename', 'Optics', 'DBSCAN', 'Mean Shift', 'Affinity', 'Agglomerative'])
    
with open('multi_score.csv', 'w', newline='' ) as csvfile: # creates file multi_score.csv for storing rand index of each algorithm for each file
    writer=csv.writer(csvfile)
    writer.writerow(['filename', 'Optics_RI', 'DBSCAN_RI', 'Mean Shift_RI', 'Affinity_RI', 'Agglomerative_RI'])


for f in csv_files: # for loop to access each file in csv_file location in computer
    #for file in path:
        X_=pd.read_csv(f) # crates dataframe with name X_ for storing data of file
        X=X_.iloc[:,1 :-1].values  # assigns X to store all columns in the file except the last label column
        y=X_.iloc[:,-1].values # y stores the last label column 
    
        filename=f.split("\\")[-1]


#OPTICS algorithm
        
        startoptics=time.time()
        
    # Building the OPTICS Clustering model
        optics_model = OPTICS(min_samples = 30, xi = 0.05, min_cluster_size = 0.1)
    
    # Training the model
        optics_model.fit(X) # fit the optics function to data X
        optics_labels = optics_model.labels_ # assigns labels predicted by optics algorithm to optics_labels
        optics_time=time.time()-startoptics # calculates total time taken by algorithm to process the file
        
        
#DBSCAN aglorithm
        startdbscan = time.time() #variable store the start time of the algorithm
    
        dbscan1 =cluster.DBSCAN(eps=0.5).fit(X) #call and fit dbscan function from cluster library
       
        dbscan_labels = dbscan1.labels_ #stores labels predicted by DBSCAN algorithm 
        enddbscan=time.time()
        dbscan_time =enddbscan-startdbscan # calculates the total time taken by dbscan algorithm to process one file
    
    
#Means Shift aglorithm
        
        n_neighbors=2 # set number of neighbour hyperparameter to 2
        connectivity = kneighbors_graph(
            X, n_neighbors=n_neighbors, include_self=False  #calculate connectivity hyperpameter
            )
        # make connectivity symmetric
        connectivity = 0.5 * (connectivity + connectivity.T)
        quantile= 0.2
        bandwidth = cluster.estimate_bandwidth(X, quantile=quantile) # calculate bandwith hyperpameter
        
        startms=time.time()# note the start time of algorithm
    
        ms = cluster.MeanShift(bandwidth=bandwidth, bin_seeding=False).fit(X)# fit Meanshift algorithm to data X
        ms_label=ms.labels_ # stores the labels predicted by means shift algorithm
                       
        ms_time=time.time()-startms# calculates the total time taken by means shigt algorithm to process one file


#Affinity algorithm
        preference=-220 #setting hyperparameter preference to -220
        
        damping=0.55 # set damping hyperparameter to 0.55
        startaffinity=time.time() # note the start time of the algorithm
        
        affinity_propagation = cluster.AffinityPropagation(
            damping=damping, preference=preference, random_state=0
            ).fit(X) #call AffinityPropagation algorithm from cluster library and fit to data X
    
        affinity_labels=affinity_propagation.labels_# stores the label predicted by affinit algorithm
    
        affinity_time=time.time()-startaffinity # evaluate the total time taken by affinity algorithm to process one file
        
    
     
#Agglomerative algorithm        
        
        start_agglo=time.time() # stores the start time of the algorithm 
        clustering_model_no_clusters = AgglomerativeClustering(n_clusters=None, distance_threshold=10) # call agglomerative algorithm 
    # #(linkage="ward")
        clustering_model_no_clusters.fit(X) # fit the agglomerative algorithm to data X
        labels_no_clusters = clustering_model_no_clusters.labels_ #stores the labels predicted by agglomerative algorithm 
        
        agglo_time=time.time()-start_agglo # calculates the total time taken by agglomerative algorithm to process one data file



        
        rand_optics=rand_score(y, optics_labels) #calculates the rand index score of optics algorith
        rand_ms_label=rand_score(y, ms_label) #calculates the rand index score of means shift algorith
        rand_affinity_labels=rand_score(y, affinity_labels) #calculates the rand index score of affinity algorith
        rand_dbscan_labels=rand_score(y, dbscan_labels) # #calculates the rand index score of dbscan algorith
        rand_aglo_label=rand_score(y, labels_no_clusters) #calculates the rand index score of agglomerative algorith
       
        
        
        d={'File_name':[filename],'RI_Optics':[rand_optics], 'RI_MS':[rand_ms_label], 
            'RI_Affinity':[rand_affinity_labels],
             'RI_DBSCAN':[rand_dbscan_labels], 
             'RI_Agglo':[rand_aglo_label] }

            
              
        df=pd.DataFrame(data=d) # stores rand index to dataframe df
        
        df.to_csv('multi_score.csv', header=False,mode='a', index =False) #sends the dataframe df to multi_score.csv file
        
        d2={'File_name':[filename], 'Optics time':[optics_time], 'dbscan time':[dbscan_time], 'MS time':[ms_time], 'Affinity time':[affinity_time], 'Agglomerative':[agglo_time]}

        df2=pd.DataFrame(data=d2) # stores time data to dataframe df2
        df2.to_csv('multi_time.csv', header=False, mode='a', index=False) # sends dataframe df2 to multi_time.csv file
        
       

print('End of Program') # print end of program


