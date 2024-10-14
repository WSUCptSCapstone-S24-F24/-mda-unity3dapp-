# Tin Whiskers Unity 3D App

## Project summary

It is this project's goal to develop a software tool that can capture a 3D model of a printed circuit board (PCB), identify its exposed conductors, simulate a storm of detached metal whiskers landing on the PCB, and provide analysis for recorded data.

Our team’s primary objective with this project is to develop a software tool that will aid in preventing failures caused by tin whiskers. To do this we will use the Unity 3D Engine to map virtual a model of a printed circuit board (PCB). Then we will identify the exposed conductors on the PCB. Lastly, we can then simulate a problematic event of many detached metal whiskers being scattered about on the PCB and moving around due to airflow, vibrations, or current. The user will be able to define the composition of the whiskers; selecting from different materials such as tin, zinc, and cadmium. The user will also be able to specify simulation characteristics such as the number of whiskers, and the whisker’s length and thickness distributions. Additionally, the user will also have control over the “storm” event that causes the movement of the tin whiskers. Once the simulation is run, the tool will show all conductor pairs that became bridged by a whisker during the event. The probability of bridging or conductor pairs can be found using Monte Carlo simulations. Enhancements of the tool will add time-dependent forces such as gravity, zero gravity, air flow, vacuum, vibration, shock, and electrostatic forces (E-fields) all of which can affect the motion of the detached metal whiskers.

## Demo Videos
* [Sprint 1 Demo Video](https://youtu.be/HgCC78tZCsM)
* [Sprint 2 Demo Video](https://youtu.be/hJ81NFluXlo)
* [Sprint 3 Demo Video](https://youtu.be/iONjIvFagGM)
* [Sprint 4 Demo Video](https://youtu.be/_qFKNdXabYY)
* [Sprint 5 Demo Video](Sprint5Demo.mp4)
* [Sprint 6 Demo Video]()


## Installation

### Prerequisites

* Unity (version 2021.3.21f1 or newer)


### Installation Steps

* Clone the repository with 'git clone https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-.git'
* Open Unity Hub and click on 'Add'
* Navigate to folder with cloned repository and find and open the folder labeled 'Tin Whisker POC'

## Functionality

Open the UI scene and start a simulation. Once in the simulation, click the load button to load in a PCB (printed circuit board). The load button will ask for two files, an OBJ and MTL file. Make sure these files are the same PCB or issues will occur.

After the PCB has been loaded in, it will be visible in the simulation. This is where changes to the parameters in the simulation may be change. This includes the spawn area size, the number of the whiskers in that spawn area, sigma and mu for the lengths and widths of the whiskers.

Once satisfied by the parameters, click the `Run Sim` button or `Run` under the `Monte Carlo` button.

After the simulation has run, the "Preview Results" button will show options to preview different simulation outputs (along with the input parameters for the simulation). The actual results files can be found in the 'SimulationResults' folder. 


## Known Problems

If the program will not close, press Alt-F4.

## Additional Documentation
  * [LICENSE](LICENSE.txt)
  * [Client Reports](ClientReports.md)
  * [Sprint 1 Report](Sprint1Report.md)
  * [Sprint 2 Report](Sprint2Report.md)
  * [Sprint 3 Report](Sprint3Report.md)
  * [Sprint 4 Report](Sprint4Report.md)
  * [Sprint 5 Report](Sprint5Report.md)
  * [Sprint 6 Report](Sprint6Report.md)
