#!/bin/sh
set -e
export PATH="$PATH:/root/.dotnet/tools"
rm -Rf /app/Output/CodeEvaluation
mkdir -p /app/Output/CodeEvaluation
# Delete the obj and bin folders as if it was compiled in windows then the linux build will fail due to case sensitivity issues
find /app -type d \( -name "obj" -o -name "bin" \) -exec rm -rf {} +
cd /app/Solutions/Ddd.App
dotnet build Ddd.App.sln
dotnet test --collect:"XPlat Code Coverage" --results-directory /app/Output/CodeEvaluation/TestResults --settings /app/CodeEvaluation/CodeCoverage.runsettings.xml Ddd.App.sln
cd /app/Output/CodeEvaluation/TestResults
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"/app/Output/CodeEvaluation/TestResults/coveragereport" -reporttypes:Html