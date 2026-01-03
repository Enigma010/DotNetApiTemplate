#!/bin/sh
export PATH="$PATH:/root/.dotnet/tools"
mkdir -p /app/Output/App.UnitTests
rm -Rf /app/Output/App.UnitTests
cd /app/Api
dotnet build Api.sln
cd /app/App.UnitTests
dotnet test --collect:"XPlat Code Coverage" --results-directory /app/Output/App.UnitTests/TestResults --settings CodeCoverage.runsettings.xml
guid=$(ls /app/Output/App.UnitTests/TestResults/)
report=$(ls /app/Output/App.UnitTests/TestResults/$guid/coverage.cobertura.xml)
reportgenerator -reports:$report -targetdir:"/app/Output/App.UnitTests/TestResults/coveragereport" -reporttypes:Html