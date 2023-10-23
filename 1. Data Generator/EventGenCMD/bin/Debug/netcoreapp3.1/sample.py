threads = [1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 22, 33, 44, 55, 66, 77, 88, 99, 111, 222, 333, 444, 555, 666, 777, 888, 999, 1111]
batchsize = 9
for i in range(0, len(threads), batchsize):
	threadBatch = threads[i:i+batchsize]
	print (str(i) + ' to ' + str(i+batchsize))
	for th in threadBatch:		
		print (th)
	
	per = (i+batchsize) / len(threads)
	print ('\n Progress: ', per)