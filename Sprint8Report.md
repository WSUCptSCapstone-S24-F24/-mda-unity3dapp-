# Sprint 8 Report (11/14/2024 - 12/13/2024)

## Demo Video
Sprint 8 Demo Video: https://www.youtube.com/watch?v=yBrvcmIR8_c

## What's New (User Facing)
* **Fixing component toggles**: Toggles of conductive components are preserved throughout different menus. 

## Work Summary (Developer Facing)
This print our team worked to fix obvious bugs and prepare for making a final deliverable. The most important change was the change to the conductive components selection. This change fixed the bug where the state of the toggles were not be preserved throughout different menus. The last notable change was the set up for Unity's Test Framework. This should allow future teams to integrate tests easily. 

## Unfinished Work
* **Saving and loading conductive components**: This was not completed due to it being a "best-case-scenario-if-we-have-time" type of issue. Our team was simply unable to get to this issue.

* **Tutorial and related issues** There were 3 user issues associated with this subject, and none were completed. This was due to lack of contribution from the member of our team who was assigned these issues. 

## Completed Issues/User Stories
Here are links to the issues that we completed in this sprint:
* No associated issue -- Commit link instead: [Fixing component toggles](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/commit/237420c48df91a1a7e73158b24dfb9c8d767206c)

## Incomplete Issues/User Stories
Here are links to issues we worked on but did not complete in this sprint:
* [Saving and loading conductive components](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/99)
* [Directions on how to move in the simulation and other sim directions in tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/83)
* [Viable ranges in tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/82)
* [Tutorial page](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-/issues/15)


## Code Files for Review
All of the code files for this project can be found here:  
[WSUCapstone2024-mda-unity3dapp](https://github.com/WSUCptSCapstone-S24-F24/-mda-unity3dapp-).

## Retrospective Summary
Here's what went well:
* Client demo - clients were happy with our product
* Senior Design Project Competition - our team received 1st place

Here's what we'd like to improve:
* Save the set of conductive materials for each board, so the components don’t need to be set every load
* Scene UI fixes for organization and workflow
* Use built-in features for the results previewer UI/scroller to improve efficiency with previewing large files
* Create formal unit and system-level tests (and fix resulting bugs)
* Use GPU or other forms of parallel processing to speed up the Monte Carlo simulation and allow for more sims
* Optimize viable variable ranges so that users cannot break the system
* Fix the application side UI so that it scales better and is more visually appealing and user-friendly 
* Have shock and vibrate only show up in results (sim state) if the shock and vibrate were enabled for the simulation run
* Fix/add a tutorial page and tooltips

