# MDA_TWAG
Tin Whiskers group repository

Matthew Sienkiewich - 011685126
Gage Unruh - 11678602
Trevor Wakefield - 11693919

# To Run Program
  ## Start Program
    In order to start the simulation, simply double click on the start batch file, this will launch Unity
  ## In simulation
    Once in the simulation, click the load button to load in a PCB (printed circuit board). The load button will ask for two files, an OBJ and MTL file. Make sure these files are the same PCB or issues will occure.
    After the PCB has been loaded in, it will be visible in the simulation. This is where changes to the parameters in the simulation may be change. This includes the spawn area size, the denisty of the whiskers in that spawn area, sigma and mu for the lengths and widths of the whiskers.
    By clicking the "Get Results" button, an output will be generated from the current parameters and called sim_0.
    Once satisfied by the parameters, click the start button, this will lock in the parameters to be used in the monte carlo simulation. 
    To start the Monte Carlo, look at the bottom right to see the two input boxes. These inputs represent how many simulations are to be ran and how long these simulations run for.
    Once all the simulations have completed, the outputs will be sent to the "BridgedComponentsResults" folder and will be numbered starting from sim_1 to sim_n where n is the number of simulations set by the user.
    To produce a heat map from the results, a valid python install must be downloaded. After this, use the pip command to install the requirements.txt: pip install -r requirements.txt.
    After calling pip install, in the same console, call: python heatmap.py. This will open a popup asking for the location of the simulation outputs. The outputs should be located in the "BridgedComponentsResults" folder.
    Once the folder is found, select it. This will open a window containing the heatmap. *IMPORTANT* The heatmap is not automatically saved so be sure to save the heatmap to a desired location.
  ## To close program
    Alt-F4
