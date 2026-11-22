#!/bin/sh
set -e
export PATH="$PATH:/root/.dotnet/tools"
find /app/Output/CodeEvaluation -mindepth 1 -delete
cd /src/Solutions/Ddd.App
dotnet test --collect:"XPlat Code Coverage" --results-directory /app/Output/CodeEvaluation/TestResults --logger "html" --settings CodeCoverage.runsettings.xml Ddd.App.sln
cd /app/Output/CodeEvaluation/TestResults
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"/app/Output/CodeEvaluation/TestResults/coveragereport" -reporttypes:Html