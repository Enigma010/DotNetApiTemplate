#!/bin/sh
set -e
export PATH="$PATH:/root/.dotnet/tools"
rm -Rf /app/Output/CodeEvaluation
mkdir -p /app/Output/CodeEvaluation
cd /app/Solutions/App
dotnet build App.sln
dotnet test --collect:"XPlat Code Coverage" --results-directory /app/Output/CodeEvaluation/TestResults --settings /app/CodeEvaluation/CodeCoverage.runsettings.xml App.sln
cd /app/Output/CodeEvaluation/TestResults
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"/app/Output/CodeEvaluation/TestResults/coveragereport" -reporttypes:Html