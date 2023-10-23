goto comment

1 - # of chains
2 - # of events in each chain
3 - # of objects in event
4 - % similarity among event in each chain
5 - total # of event required in the dataset
6 - total # of unique objects
7 - number of times to repeat each chain

:comment
::::::::::::::::::::::::::::::::::::::::::::
SET numOfChains1=1
FOR /l %%x in (1, 1, 10)  DO (call :subroutine1 "%%x")

:subroutine1
	::1, 2, ... 10 chains of 10 events each in total of 1000 events
	.\EventGenCMD.exe %numOfChains1% 10 10 70 1000 100 1
	set /a numOfChains1+=1
	

::::::::::::::::::::::::::::::::::::::::::::
SET numOfChains2=1
SET eventsPerChain2=5
FOR /l %%x in (1, 1, 10)  DO (call :subroutine2 "%%x")

:subroutine2
	::1, 2, ... 10 chains of 5, 10, ... 50 events each in total of 1000 events
	.\EventGenCMD.exe %numOfChains2% %eventsPerChain2% 10 70 1000 100 2
	set /a numOfChains2+=1
	set /a eventsPerChain2+=5
	

::::::::::::::::::::::::::::::::::::::::::::	
SET numOfChains3=1
SET eventsPerChain3=5
SET uniqueObjs=10
FOR /l %%x in (1, 1, 10)  DO (call :subroutine3 "%%x")

:subroutine3
	::1, 2, ... 10 chains of 5, 10, ... 50 events each, generated from varying object pool, in a total of 1000 events
	.\EventGenCMD.exe %numOfChains3% %eventsPerChain2% 10 70 1000 %uniqueObjs% 3
	set /a numOfChains3+=1
	set /a eventsPerChain3+=5
	set /a uniqueObjs3+=10
	
	
::::::::::::::::::::::::::::::::::::::::::::	
SET similarity4=10
FOR /l %%x in (1, 1, 10)  DO (call :subroutine4 "%%x")

:subroutine4
	::10%, 20%, ... 90% similarity among events in each chain of 10 events
	.\EventGenCMD.exe 10 10 10 %similarity4% 1000 100 4
	set /a similarity4+=10
	

::::::::::::::::::::::::::::::::::::::::::::
SET uniqueObjs5=10
SET similarity5=10
FOR /l %%x in (1, 1, 10)  DO (call :subroutine5 "%%x")

:subroutine5
	::10, 20, ... 100 unique objects with varying similarity among events in each chain 
	.\EventGenCMD.exe 10 10 10 %similarity5% 1000 %uniqueObjs5% 5
	set /a similarity5+=5
	set /a uniqueObjs5+=10