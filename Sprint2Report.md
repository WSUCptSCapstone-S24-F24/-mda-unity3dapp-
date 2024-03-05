# Sprint 2 Report (2/4/2024 - 3/4/2024)

## Demo Video
Sprint 2 Demo Video: [Demo Video](https://youtu.be/)   

## What's New (User Facing)
 * Tin Whisker Simulation Tool: Introduction of a virtual model of a printed circuit board (PCB) within the Unity 3D Engine from the previous team.
 * Enhanced Simulation Controls: refactoring, reorganizing, and understanding the code from the previous team (As well as a few user centered optimizations).

## Work Summary (Developer Facing)
During this sprint, our team officially merged the code from previous teams. Utilizing the Unity 3D Engine, we successfully mapped a virtual model of a PCB and implemented features to identify exposed conductors. We encountered challenges in accurately modeling the whisker materials and their interactions. However, through collaborative problem-solving and leveraging advanced Monte Carlo simulation techniques, we have made substantial progress. This work not only advances our tool's capabilities but also enriches our team's understanding of complex simulation environments.

## Unfinished Work
Material Selection for Simulation: Users select from different whisker materials such as tin, zinc, and cadmium for more accurate simulation results.

## Completed Issues/User Stories
  * Size/scale is shown to user 
  * Multiple OS system file openers 
  * Node system and correct naming 
     * Each component has a name
     * Each component has nodes
     * Each node belongs to component and has a name
  * Remove unnecessary files and code 
  * Reorganize files and directories 
  * More refined simulation runner 
     * UI
     * Size of whiskers and PCB are specified and known
 
 ## Incomplete Issues/User Stories
No issues or user stories for sprint 2

## Code Files for Review
All of the code files for this project can be found here
 [WSUCapstoneS2023-MDA_TWAG](https://github.com/WSUCapstoneS2023/MDA_TWAG).
 
## Retrospective Summary
Here's what went well:
  * Successful mapping of a virtual PCB model within Unity.
  * Implementation of user-selectable whisker materials.
  * Development of an interactive interface for simulation parameters.
 
Here's what we'd like to improve:
   * Accuracy of environmental force simulations.
   * Efficiency of the Monte Carlo simulation for predicting conductor bridging.
   * User experience in defining complex simulation scenarios.
  
Here are changes we plan to implement in the next sprint:
   * Integrate more accurate models for gravity and other environmental forces.
   * Enhance the Monte Carlo simulation for faster and more accurate predictions.
   * Improve the UI/UX for easier setup and control of simulation parameters.
