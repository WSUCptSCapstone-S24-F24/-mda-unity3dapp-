# Sprint 5 Report (8/19/2024 - 9/13/2024)

## Demo Video
Sprint 5 Demo Video: [Demo Video] https://youtu.be/JierWA-c8kA

## What's New (User Facing)
 * Created their own simple 2D PCB layout OR find examples online using ALTIUM
 * Identify “Bridges” between exposed conductors on the BARE PCB
 * Do Monte Carlo Simulations of the above and capture statistics of bridged conductor pairs on the BARE PCB
 * Enhanced Simulation Controls: refactoring, reorganizing, and understanding the code from the previous team (As well as a few user centered optimizations).
 * Diagram for PCB Converstion
 * Correct Lognormal Distribution

## Work Summary (Developer Facing)
During this sprint, we imported the PCB’s TOP LAYER from STEP files or OBJ files or other format that will INCLUDE:
ID #, Location, and shape of every individual SOLDER PAD, Ground connection, power connection, etc and other “exposed conductor” on this 2D plane of the PCB
WITHOUT actual COMPONENTS mounted to the PCB. This work not only advances our tool's capabilities but also enriches our team's understanding of complex simulation environments.

## Unfinished Work
 * A follow on goal would be to "ADD COMPONENTS" to the PCB to create a 3D problem. Defining the INDIVIDUAL CONDUCTORS for each COMPONENT is not yet a problem that has been solved.
 * Add “physics” such as vibe/shock

## Completed Issues/User Stories
  * https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/35
  * https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/36
  * https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/63
 
 ## Incomplete Issues/User Stories
  * Define individual conductors
  * For issues 35/36, We decided that the feature would not enough value for the resources we had.

## Code Files for Review
All of the code files for this project can be found here
 [WSUCapstoneS2023-MDA_TWAG](https://github.com/WSUCapstoneS2023/MDA_TWAG).
 
## Retrospective Summary
Here's what went well:
  * Successful example of Altium Board
  * Implementation of Monte Carlo Simulations
  * Correct Lognormal Distribution
 
Here's what we'd like to improve:
   * Add “physics” such as vibe/shock
  
Here are changes we plan to implement in the next sprint:
   * Refined shake PCB (show in output and more control)
   * The load in functionality for STEP files is similar to the parser itself, we can load in STEP files using our current file browsing methods however it is not worth pursing unless the parser can change the file type to OBJ and MTL.
   * Tilt PCB
   * Probability of a whisker bridging (num bridged whiskers/num whiskers)
   * Probability of bridging (num bridges/num whiskers)
