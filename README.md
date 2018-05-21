# meeting-tool-backend

This project is the back-end of the Meeting Rooms Services application. 

Is an.Net Core RESTFul API.

## Getting Started

To get you started you can simply clone the `meeting-tool-backend` repository.

### Prerequisites

You need [git][git] to clone the meeting-tool-backend repository.
In order to build the application, you will need Visual Studio 2017 and [.Net Core][dotnet].

### Clone `meeting-tool-backend`

Clone the `meeting-tool-backend` repository using git:

```bash
git clone https://github.com/systelab/meeting-tool-backend.git
cd meeting-tool-backend
```
### Open the Visual Studio solution

Once you have the repository cloned, open the visual studio solution 'main.csproj'

The solution contains the Web API.

The testing is not implemented yet.

### Configuration

Set the parameters in the [Appsettings][appsettings] for the meetings service to retrieve the meetings scheduled.
- url
- redirectto
- username
- password

### Run

#### Use Visual Studio
To run the project, press the run button provided by Visual Studio. The browser will be opened with the included swagger page. The start point can be changed in the 'launchSettings.json' [launchsettings].

[launchsettings]:https://github.com/systelab/meeting-room-service-backend/blob/master/src/main/Properties/launchSettings.json
[appsettings]:https://github.com/systelab/meeting-room-service-backend/blob/master/src/main/appsettings.json
[git]: https://git-scm.com/
[dotnet]:https://www.microsoft.com/net/download/windows
