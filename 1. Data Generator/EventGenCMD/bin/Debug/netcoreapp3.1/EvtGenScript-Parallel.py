#1 - # of chains
#2 - # of events in each chain
#3 - # of objects in event
#4 - % similarity among event in each chain
#5 - total # of event required in the dataset
#6 - total # of unique objects
#7 - number of times to repeat each chain


import threading
import time
import subprocess

def singleThread( numOfChains, numEventsChain, numObjectsInEvent, simChain, totalEvents, totalNumUniqueObjects, numRepeatChain):
	print ('Starting a new thread, params: ' + numOfChains + ', ' + numEventsChain + ', ' + numObjectsInEvent + ', ' + simChain + ', ' + totalEvents + ', ' + totalNumUniqueObjects + ', ' + numRepeatChain)
	subprocess.call(['EventGenCMD.exe', numOfChains, numEventsChain, numObjectsInEvent, simChain, totalEvents, totalNumUniqueObjects, numRepeatChain])
	
	
#https://medium.com/mindful-engineering/multithreading-multiprocessing-in-python3-f6314ab5e23f

threads = []
for totalEvents in range(20000, 100000, 20000):
	for numOfChains in range(10, 100, 10):
		for numRepeatChain in range(1, 9, 2):
			for numEventsChain in range(10, 100, 10):
				for numObjectsInEvent in range(5, 20, 5):
					for totalNumUniqueObjects in range(200, 1000, 200):
						for simChain in range(10, 100, 10):
							try:
								t1 = threading.Thread(target=singleThread, args=(str(numOfChains), str(numEventsChain), str(numObjectsInEvent), str(simChain), str(totalEvents), str(totalNumUniqueObjects), str(numRepeatChain),))
								threads.append(t1)
							except:
								print ('Error: unable to start thread')

print ('# of threads created: ', len(threads))

batchsize = 3000
for i in range(0, len(threads), batchsize):
	threadBatch = threads[i:i+batchsize]
	print ('\n\t*Starting ' + str(len(threadBatch)) + ' threads in parallel*\n')
	for th in threadBatch:
		th.start()		
	print ('\n\t*Waiting to finish ' + str(len(threadBatch)) + ' threads*\n')
	for th in threadBatch:
		th.join()
	per = ((i+batchsize) / len(threads)) * 100
	print ('\n\t*Progress made: ' + str(per) + '%*\n')
		