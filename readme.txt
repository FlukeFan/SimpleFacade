
Simple Facade
=============

A small library to help separate logic into commands and queries.


Building
========

To build, open CommandPrompt.bat, and type 'b'.

Build commands:

b                               : build
bt <part-test-name>             : run test where "test =~ <part-test-name>"
b /t:clean                      : clean
b /t:setApiKey /p:apiKey=[key]  : set the api key
b /t:push                       : Push packages to NuGet and publish them (setApiKey before running this)
