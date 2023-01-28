# RPA FRAMEWORK

## PURPOSE
The purpose of the RPA FRAMEWORK project is providing a framework that helps the development of RPA Applications using based web systems. The framework uses the [Selenium test suit](https://www.selenium.dev/) to do the browser automation.

Using this framework developers can focus on the general steps of the RPA Automation and its bussiness rules.
All the selenium driver interactions are done by the framework.

## PRINCIPLES
The main component of the Framework it's a state machine that goes over all the steps of the automation. Each state of this machine it's represented by a C# class with a method named Execute. This method is reponsible for perfoming the automation actions. Each automation action its performed by invoking a robot request. The are several kinds of requests like performing a click or setting a text value.
At the end of the state processing the machine transitions to next state. This transition its defined by one or more "guards". 
Each guards contains a condition that must be satisfied to the machine gets on the next state. Several guards can be definied for the same origin state, so the machine can follow diferent sequences of states according some conditions found during the state processing.
The state machine has 2 watchdogs to prevent its become freezed at some state:
The first watchdog it's responsible to monitor the delay beetween transitions, so if a transition does not occur ata
