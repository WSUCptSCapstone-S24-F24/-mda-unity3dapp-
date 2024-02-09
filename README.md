# Tin Whiskers Unity 3D App

## Project summary

It is this project's goal to develop a software tool that can capture a 3D model of a printed circuit board (PCB), identify its exposed conductors, simulate a storm of detached metal whiskers landing on the PCB, and provide analysis for recorded data.

Our team’s primary objective with this project is to develop a software tool that will aid in preventing failures caused by tin whiskers. To do this we will use the Unity 3D Engine to map virtual a model of a printed circuit board (PCB). Then we will identify the exposed conductors on the PCB. Lastly, we can then simulate a problematic event of many detached metal whiskers being scattered about on the PCB and moving around due to airflow, vibrations, or current. The user will be able to define the composition of the whiskers; selecting from different materials such as tin, zinc, and cadmium. The user will also be able to specify simulation characteristics such as the number of whiskers, and the whisker’s length and thickness distributions. Additionally, the user will also have control over the “storm” event that causes the movement of the tin whiskers. Once the simulation is run, the tool will show all conductor pairs that became bridged by a whisker during the event. The probability of bridging or conductor pairs can be found using Monte Carlo simulations. Enhancements of the tool will add time-dependent forces such as gravity, zero gravity, air flow, vacuum, vibration, shock, and electrostatic forces (E-fields) all of which can affect the motion of the detached metal whiskers.

## Demo Videos
Sprint 1 Demo Video: [Demo Video](https://youtu.be/HgCC78tZCsM)

## Installation

### Prerequisites

* Unity (version 2021.3.21f1 or newer)

### Add-ons

* Python heat map generator

### Installation Steps

* Clone the repository with 'git clone https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-.git'
* Open Unity Hub and click on 'Add'
* Navigate to folder with cloned repository and find and open the folder labeled 'Tin Whisker POC'

## Functionality

Open a scene and start a simulation. Once in the simulation, click the load button to load in a PCB (printed circuit board). The load button will ask for two files, an OBJ and MTL file. Make sure these files are the same PCB or issues will occure.

After the PCB has been loaded in, it will be visible in the simulation. This is where changes to the parameters in the simulation may be change. This includes the spawn area size, the denisty of the whiskers in that spawn area, sigma and mu for the lengths and widths of the whiskers.

By clicking the "Get Results" button, an output will be generated from the current parameters and called sim_0.

Once satisfied by the parameters, click the start button, this will lock in the parameters to be used in the monte carlo simulation.

To start the Monte Carlo, look at the bottom right to see the two input boxes. These inputs represent how many simulations are to be ran and how long these simulations run for.

Once all the simulations have completed, the outputs will be sent to the "BridgedComponentsResults" folder and will be numbered starting from sim_1 to sim_n where n is the number of simulations set by the user.

To produce a heat map from the results, a valid python install must be downloaded. After this, use the pip command to install the requirements.txt: pip install -r requirements.txt.

After calling pip install, in the same console, call: python heatmap.py. This will open a popup asking for the location of the simulation outputs. The outputs should be located in the "BridgedComponentsResults" folder.

Once the folder is found, select it. This will open a window containing the heatmap. *IMPORTANT* The heatmap is not automatically saved so be sure to save the heatmap to a desired location.

FUNCTIONALITY DIRECTIONS SOURCE: [WSUCapstoneS2023-MDA_TWAG](https://github.com/WSUCapstoneS2023/MDA_TWAG)

## Known Problems

If the program will not close, press Alt-F4.

## Additional Documentation
  * [LICENSE](LICENSE.txt)
  * [Client Reports](ClientReports.md)
  * [Sprint 1 Report](Sprint1Report.md)
