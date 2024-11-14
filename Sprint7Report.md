# Sprint 7 Report (10/15/2024 - 11/13/2024)

## Demo Video
Sprint 7 Demo Video: https://youtu.be/_-hm3ye_4Jg

## What's New (User Facing)
* **Environment Lighting**: Improved environment lighting for better visual clarity.
* **Mu and Sigma Positioning Switched**: Switched the positions of mu and sigma for better accuracy in simulations.
* **Toggle for Whisker Spawn Box**: Added a toggle to allow users to control the appearance of the whisker spawn box.
* **Progress Meter for Monte Carlo**: Implemented a progress meter for better tracking of Monte Carlo simulation progress.
* **Monte Carlo Results Fixes**: Resolved issues with the display of Monte Carlo simulation results.
* **Coloring Colliding Whiskers**: Introduced a feature to color colliding whiskers for better visual representation in simulations.
* **UI Menus**: Improved and reorganized user interface menus for better accessibility and workflow.
* **Multiple PCB Loading and Parsing**: Enabled the ability to load and parse multiple PCBs for efficient handling of complex projects.
* **Conductive Components Selection**: Implemented a selection feature for conductive components on the PCB board.
* **Inspection Mode**: Developed an inspection mode for users to closely examine simulation results.
* **Set Board Tilt, Position, and Size**: Added features to adjust the tilt, position, and size of the board while maintaining scale.
* **STIG Checks (Almost Fully Completed)**: Completed nearly all of the STIG checks, ensuring compliance with security requirements.

## Work Summary (Developer Facing)
During Sprint 7, the team worked on several significant features and improvements. Key work involved adding new UI functionalities such as the whisker spawn box toggle and the progress meter for Monte Carlo simulations. We also made progress on Monte Carlo results adding results that NASA requested. In terms of simulation accuracy, the team switched the mu and sigma positioning to improve the correctness of the models. The addition of environment lighting, coloring of colliding whiskers, and inspection mode provided better visual feedback for the users, improving the overall user experience. There was also a focus on enhancing the backend, specifically implementing the ability to load and parse multiple PCBs, as well as improving the selection of conductive components. Additionally, the STIG checks were almost fully completed, improving security compliance. The team overcame a few challenges related to UI organization and ensuring the stability of the Monte Carlo results, but these were addressed as part of the sprint.

## Unfinished Work
* **Scene UI Fixes for Organization and Workflow**: The team didn’t get a chance to fully organize and optimize the scene UI for better workflow.
* **Use Built-in Features for Results Previewer UI/Scroller**: The implementation of built-in features to improve efficiency in the previewer UI was not fully completed.
* **Unit and System-Level Tests**: Although tests were started, formal unit and system-level tests were not fully implemented during the sprint.
* **GPU or Parallel Processing for Monte Carlo Simulation**: The exploration of GPU or other forms of parallel processing to speed up the Monte Carlo simulations was not completed.
* **Optimize Viable Ranges**: Further work is required to optimize the viable ranges to prevent system breakage by users.
* **Fix Application UI Scaling and Visual Appeal**: The team didn't have enough time to complete the UI scaling fixes and visual enhancements for a more user-friendly interface.
* **Shock and Vibration Results**: The feature to have shock and vibration only show in the results if enabled during simulation was not fully integrated.
* **Fix Conductive Component Toggles**: The issue with toggling conductive components when leaving and returning to the board settings page was not fully resolved.

## Completed Issues/User Stories
Here are links to the issues that we completed in this sprint:
* [NASA Request from Beta Deliverables: Environment Lighting](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/123)
* [Mu and Sigma Positioning Switched](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/75)
* [Whisker Spawn Box Toggle](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/76)
* [Progress Meter for Monte Carlo](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/79)
* [Monte Carlo Results Fixes](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/88)
* [Coloring Colliding Whiskers](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/86)
* [UI Menus](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/72)
* [Multiple PCB Loading and Parsing](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/87)
* [Conductive Components Selection](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/96)
* [Inspection Mode](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/)
* [Set Board Tilt, Position, and Size](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/)
* [STIG Checks](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/92)

## Incomplete Issues/User Stories
Here are links to issues we worked on but did not complete in this sprint:
* [Tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/15)

* [Viable ranges in tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/82)

* [Directions on how to move in the simulation and other sim directions in tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/83)

* [Saving and loading conductive components](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/99)

* [Unit Tests](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/98)

## Code Files for Review
All of the code files for this project can be found here:  
[WSUCapstone2024-mda-unity3dapp](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-).

## Retrospective Summary
Here's what went well:
* The integration of new features such as the Monte Carlo progress meter and whisker spawn box toggle was smooth.
* We successfully implemented several visual improvements, including environment lighting and coloring colliding whiskers.
* Significant progress was made on the STIG checks, with nearly all completed.

Here's what we'd like to improve:
* Time management for UI-related tasks, as some improvements had to be pushed to the next sprint.
* Better planning for implementing tests, as some unit and system tests were left incomplete.

Here are changes we plan to implement in the next sprint:
* Focus on completing the unit and system-level tests.
* Continue improving the UI for better scaling and usability.
* Work on optimizing the Monte Carlo simulation through parallel processing or GPU acceleration.
* Address the incomplete features like shock and vibration integration and UI organization.
