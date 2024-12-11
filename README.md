# Tin Whiskers Unity 3D App

## Project summary

This project is a software tool that allows a user to load a 3D model of a printed circuit board (PCB), identify its exposed conductors, simulate a storm of detached metal whiskers landing on the PCB, and provide analysis for recorded data.

## Demo Image
![Demo Image](ProjectDemoPic.png)

## Project Objective

Our team’s primary objective with this project is to develop a software tool that can aid in preventing failures caused by tin whiskers. To do this, we used the Unity 3D Engine. A user can import a model of a printed circuit board (PCB), and then identify the conductive components of the PCB. The user can then simulate events of many detached metal whiskers being scattered about on the PCB. The user can also include extra environment settings such as vibrations or shock. The user can also specify simulation characteristics such as the number of whiskers, and the whiskers' length and thickness distributions. The user also has control over the “storm” event that causes the movement of the tin whiskers. Once a simulation is run, the tool can show all conductive pairs bridged by a whisker during the sim through heat maps or statistical results. The probability of bridging or conductor pairs can be found using Monte Carlo simulations. The results can be used to modify the design or the build process of PCBs to prevent failures caused by metal whiskers.


## Demo Videos
* [Sprint 1 Demo Video](https://youtu.be/HgCC78tZCsM)
* [Sprint 2 Demo Video](https://youtu.be/hJ81NFluXlo)
* [Sprint 3 Demo Video](https://youtu.be/iONjIvFagGM)
* [Sprint 4 Demo Video](https://youtu.be/_qFKNdXabYY)
* [Sprint 5 Demo Video](Sprint5Demo.mp4)
* [Sprint 6 Demo Video](https://youtu.be/t8q2omL2D8Y)
* [Sprint 7 Demo Video](https://youtu.be/_-hm3ye_4Jg)
* [Sprint 8 Demo Video](https://www.youtube.com/watch?v=yBrvcmIR8_c)


## Installation

### Usage (User)
* Download the executable file for Windows or Mac and run (found here on GitHub under `Releases`).

### Prerequisites (Developer)

* Unity (version 2021.3.21f1 or newer)

### Installation Steps (Developer)

* Clone the repository with 'git clone https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-.git'
* Open Unity Hub and click on 'Add'
* Navigate to the folder with the cloned repository and find and open the folder labeled 'Tin Whisker POC'

## Functionality

Once in the application (Unity MainApp scene or the executable), click the load button to load in a PCB (printed circuit board). The load button will ask for two files, an OBJ and MTL file. Make sure these files are the same PCB or issues will occur. The conductive components, position, rotation, and size of the board can be configured in the `Board Settings` tab. (Test files can be found in the `PCB Files` folder.)

You can navigate the scene by holding the Right Mouse Button and pressing `WASD` for direction movements and `SPACE` and `Control` for up and down movements respectively.

After the PCB has been loaded in, it will be visible in the simulation. This is where changes to the parameters in the simulation may be changed (under `Simulation Settings`). This includes the spawn area size, the number of whiskers in that spawn area, sigma, and mu for the lengths and widths of the whiskers, among other details.

Once satisfied by the parameters, click the `Run Sim` button or `Run` under the `Monte Carlo` button.

After the simulation has run, the "Preview Results" button will show options to preview different simulation outputs (along with the input parameters for the simulation). The actual results files can be found in the 'SimulationResults' folder. 


## Future Work
* Save the set of conductive materials for each board, so the components don’t need to be set every load
* Scene UI fixes for organization and workflow
* Use built-in features for the results previewer UI/scroller to improve efficiency with previewing large files
* Create formal unit and system-level tests (and fix resulting bugs)
* Use GPU or other forms of parallel processing to speed up the Monte Carlo simulation and allow for more sims
* Optimize viable variable ranges so that users cannot break the system
* Fix the application side UI so that it scales better and is more visually appealing and user-friendly 
* Have shock and vibrate only show up in results (sim state) if the shock and vibrate were enabled for the simulation run
* Fix/add a tutorial page and tooltips


## Additional Documentation
  * [LICENSE](LICENSE.txt)
  * [Client Reports](ClientReports.md)
  * [Sprint 1 Report](Sprint1Report.md)
  * [Sprint 2 Report](Sprint2Report.md)
  * [Sprint 3 Report](Sprint3Report.md)
  * [Sprint 4 Report](Sprint4Report.md)
  * [Sprint 5 Report](Sprint5Report.md)
  * [Sprint 6 Report](Sprint6Report.md)
  * [Sprint 7 Report](Sprint7Report.md)
  * [Sprint 8 Report](Sprint8Report.md)
