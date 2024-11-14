# Sprint 4 Report (4/5/2024 - 5/4/2024)


## Demo Video
Sprint 4 Demo Video: [Demo Video](https://youtu.be/_qFKNdXabYY)   

## What's New (User Facing)
 * Whisker acceleration
 * UI lockouts
 * Go-to functionality
 * Results viewing
 * Spawn region visable
 * Exit functionality
 * Unit/scale fixes


## Work Summary (Developer Facing)

In the first part of the sprint, Trevor worked on making the whiskers behave more naturally. This involved having the simulation's tin whiskers be affected by gravity and all other objects in the simulation. Gavin and Trevor both contributed to making the UI more user-friendly. These improvements included more descriptive titles and lockouts to prevent users from accidentally breaking the system. Trevor ensured that the Go-to functionality was working as intended, meaning that the user is taken to the part he or she specified. The results constituted a significant part of the work for this sprint. Trevor first investigated the previous team's results system, which consisted of generating a heatmap from the bridged components with a Python script. The problem with this approach was that the user had to go to extensive lengths to configure the Python setup, the results were very limited in detail, and the results may have been showing inaccurate data (after reviewing with our NASA expert). Our NASA expert provided what the results should look like and gave a list of the expected data that should be logged before, during, and after a simulation. Trevor then worked to implement the changes that our clients suggested by processing and logging the simulation details to a 'SimulationResults' directory and adding an additional UI page that allowed the user to preview results by reviewing each 'csv' log file. Another feature that our clients prioritized was allowing the user to have a better understanding of the spawn region, so Trevor worked on implementing a "spawn box" that showed the user where the whiskers would spawn from. A small feature that Trevor also added was a button with the ability to exit the application, which before was handled by stopping the program within the game engine. Lastly, our NASA expert prioritized ensuring that the size and scale of every object in the scene were known to the user, so Trevor and Gavin worked to ensure that all measurements were associated with their corresponding units.

## Unfinished Work

During this sprint, three main goals were achieved. These included the Monte Carlo simulation, the STEP file load-in and parsing functionality, and the tutorial page.

## Completed Issues/User Stories
Here are links to the issues that we completed in this sprint:

 * [Regular sim output](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/23)
 * [Save sim outputs](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/40)
 * [Detailed simulation outputs](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/37)
 * [Show results - UI scaling issues](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/44)
 * [SimState shown in all outputs](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/42)
 * [Units of outputs in results](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/47)
 * [Whisker acceleration](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/24)
 * [PCB load in required](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/30)
 * [Whisker spawn region refinements](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/19)
 * [Clarify names of UI inputs](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/38)
 * [Size of whiskers and their parameters](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/17)
 * [Go to functionality](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/27)
 * [Propper UI lock](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/25)
 * [Pop-up messages](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/16)
 * [Node system and correct naming](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/6)
 * [Exit sim early](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/41)
 * [Exit application button](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/49)
 
 ## Incomplete Issues/User Stories
 Here are links to issues we worked on but did not complete in this sprint:
 
 * [STEP file load in](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/35) 
 * [STEP file parser](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/36)
 * [Monte Carlo Sim Output](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/22)
 * [Tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/15)
 * [Monte Carlo - Simulation Count Lock](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/43)
 * [Show PCB size to user](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/20)
 
The STEP file integration was partially completed. The STEP files need to be processed into object files through a separate software application (e.g., Blender). The Monte Carlo simulation details were not completed because the regular simulation was not fully completed until the end, and the Monte Carlo simulations build off of the regular simulations. Lastly, the tutorial page was not completed because the assignee, John, was unable to complete it.

## Code Files for Review
All of the code files for this project can be found here [WSUCapstone2024-mda-unity3dapp](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-).
 
## Retrospective Summary
Here's what went well:
  * Made significant progress completing many user issues
  * Positive feedback from peer reviews 
  * Clients were satified with progress from client demo 
  * Prototype Project Report finished
 
Here's what we'd like to improve:
   * Full team participation
   * Complete remaining user issues
   * Correct lognormal distribution
  
Here are changes we plan to implement in the next sprint:
   * Monte Carlo Sim
   * STEP file load in
   * Fix lognormal distribution (for tin whiskers)