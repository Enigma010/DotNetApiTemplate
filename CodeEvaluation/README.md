## Code Evaluation
Code is evaluated by both using Unit Tests and Test Coverage. Unit tests are one of the first primary code evaluation methodologies for validating the application.

## Unit Test Guidelines
The following describes guidelines for the architecture.

* Unit tests should be able to run and succeed with only the source code.  They should not rely on other infrastructure.
* Infrastructure can be simulated through the use of interfaces and mocking techniques.
* Unit tests need to be run to be effective.
* Unit tests must always succeed unless there is problem with the application.
* Unit tests that are not reliable need to be, rewritten or removed. Unit test failures must be trusted to show actual issues with the system.
* Unit tests should be run periodically during development. 
* No code should be committed until all unit tests are succeeding.
* Unit test code coverage should maintain the agreed upon coverage percentage.  

## Code Coverage Guideslines
The following describes guidelines for the architectore.

* Code coverage should meet 70%+ of the lines of code.