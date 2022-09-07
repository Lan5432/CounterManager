# CounterManagerWeb

This is a small project simulating an application stack of:
 - CounterManagerDb: Database reader that exposes an API to access the data.
 - CounterManagerApi: Middle-man application that does nothing but pass-through from the CounterManagerDb to any consuming clients.
 - CounterManagerWeb: UI for viewing, creating, updating and deleting counters.

Each project has a launch configuration to launch them separately, they will try to contact the service below them when used, which should either be in a Docker container, configured by Compose to use the same ports, or standalone in a Visual Studio runner. 

The URLs for each are as follows:
 - CounterManagerDb: http://localhost:8081
 - CounterManagerApi: http://localhost:8082
 - CounterManagerWeb: https://localhost:8083