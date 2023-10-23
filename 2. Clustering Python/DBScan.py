# -*- coding: utf-8 -*-
"""
Created on Fri Feb 17 12:12:06 2023

@author: SCOMMR5
"""


import pandas as pd # calls pandas library 
from sklearn.cluster import  cluster_optics_dbscan # calls dbscan algorithm function from library 
#from sklearn import cluster, datasets, mixture
import glob #function that searches for files that match a specific file pattern or name
import os#
from sklearn.metrics import rand_score # calls rand index library
from sklearn import metrics # calls metrics library
from sklearn import cluster

import time # calls time function 
from datetime import datetime # calls datetime function
import csv


path = os.getcwd()#returns current working directory to the variable path

files = glob.glob(os.path.join(path, "folder1/*.*"))# files stores the location of datasets in the computer


with open('score.csv', 'w', newline='' ) as csvfile: # open new csv files score.csv in write mode 
    writer=csv.writer(csvfile)  
    writer.writerow(['Filename','Rand Index', 'Time']) # assigns header names to score.csv file
        

for f in files:

    X_=pd.read_csv(f)#assigns whole csv file to X_ variabler
    X=X_.iloc[:,1 :-1].values  # assigns all the data colomns except the last colomns which contains labels to variable X
    y=X_.iloc[:,-1].values # assigns labels to variable y

    filename=f.split("\\")[-1]# split filename
       
    startdbscan=time.time()# stores start time

    dbscan1 =cluster.DBSCAN(eps=6, min_samples=2).fit(X) # calls dbscan function from cluster library with and fit that to data
    dbscan_labels = dbscan1.labels_ # assigns labels predicted by dbscan algorihtm to dbscan_lable 
    enddbscan=time.time()# stores end time
    dbscan_time=enddbscan-startdbscan # calculates time taken by dbscan algorithm to process the data.


    rand_dbscan_labels=rand_score(y, dbscan_labels) # calculate rand index using predicted labels by dbscan algorithm and acutal labels
    
    d={'Filename':[filename],'Rand Index':[rand_dbscan_labels],  
        'Time':[dbscan_time]} 
      
    df=pd.DataFrame(data=d) # sends filename, rand index and time to dataframe df
                
    # save dataframe to score.csv file
    df.to_csv('score.csv', header=False,mode='a', index =False) #save dataframe df data to score.csv file
    #print the output on screen   
    print('Filename:',filename, 'Rand Index:',rand_dbscan_labels,  'time:',dbscan_time) 
#print end of program        
print('End of program')


