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

## Running Evaluations
To run the unit tests and code coverage start a WSL command prompt and run the command:

```
./lCodeEvaluation.sh
```

Note that you will need docker running for this.  Once the command runs if any unit tests fail you'll see errors in the command run and you need to fix those first.  If all unit tests pass a browser window will open to show the code coverage of the unit tests.