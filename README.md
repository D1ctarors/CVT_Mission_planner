
# CVT_MissionPlanner

Website : 

Download latest stable version : 


## How to compile

### On Windows

#### 1. Install software

##### Main requirements

Currently, Mission Planner needs:

Visual Studio 2022 17.8

##### IDE

### Visual Studio Community
To compile Mission Planner, we recommend using Visual Studio. You can download Visual Studio Community from the [Visual Studio Download page](https://raw.githubusercontent.com/D1ctarors/CVT_Mission_planner/main/vs2022.vsconfig "vs2022.vsconfig").

Visual Studio is a comprehensive suite with built-in Git support, but it can be overwhelming due to its complexity. To streamline the installation process, you can customize your installation by selecting the relevant "Workloads" and "Individual components" based on your software development needs.

To simplify this selection process, we have provided a configuration file that specifies the components required for MissionPlanner development. Here's how you can use it:

1. Go to "More" in the Visual Studio installer.
2. Select "Import configuration."
3. Use the following file: [vs2022.vsconfig](https://raw.githubusercontent.com/D1ctarors/CVT_Mission_planner/main/vs2022.vsconfig "vs2022.vsconfig").

By following these steps, you'll have the necessary components installed and ready for Mission Planner development.

###### VSCode
Currently VSCode with C# plugin is able to parse the code but cannot build.

#### 2. Get the code

If you get Visual Studio Community, you should be able to use Git from the IDE. 
Clone `https://github.com/D1ctarors/CVT_Mission_planner.git` to get the full code.

In case you didn't install an IDE, you will need to manually install Git. Please follow instruction in https://ardupilot.org/dev/docs/where-to-get-the-code.html#downloading-the-code-using-git

Open a git bash terminal in the MissionPlanner directory and type, "git submodule update --init" to download all submodules

#### 3. Build

To build the code:
- Open MissionPlanner.sln with Visual Studio
- From the Build menu, select "Build MissionPlanner"
