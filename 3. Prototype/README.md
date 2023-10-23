This C# project has been created to demonstrate how event generation, event-chain identification, conversion to an action model, to exploitation using model-based reasoning can be achieved. The purpose of the prototype is to demonstrate how this can work and to serve as a starting point for anyone wanting to build on the research of implement a specific use case.

The application is driven by a primitive user interface consisting of four tabs. The four tabs providing the following features:
 
1. Dataset Generator: a user interface version of the '1. Data Generator' application
2. Cluster -> Event Chains: utilises a C# implementation of DBSCAN to identify event chains through clustering.
3. Event Chains -> Actions: Enables the user to select which parts of the event chain form an action's precondition.
4. Planner: Uses the Local Planning Graph tool to deliberate over which actions can be applied to the event dataset.
