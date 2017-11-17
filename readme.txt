
Simple Facade
=============

A small library to help separate logic into commands and queries.


Building
========

To build, open CommandPrompt.bat, and type 'b'.

Build commands:

br                                      Restore dependencies (execute this first)
b                                       Dev-build
bw                                      Watch dev-build
bt <test>                               Run tests with filter Name~<test>
btw <test>                              Watch run tests with filter Name~<test>
bc                                      Clean the build outputs
b /t:setApiKey /p:apiKey=[key]          Set the NuGet API key
b /t:push                               Push packages to NuGet and publish them (setApiKey before running this)
