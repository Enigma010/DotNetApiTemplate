#!/bin/sh
RESULTS_DIR="Output/CodeEvaluation/api-unit-test/TestResults"
for file in "$RESULTS_DIR"/TestResult__*.html; do
    if [ -f "$file" ]; then
        cmd.exe /C start "" "$file"
    fi
done
cmd.exe /C start Output/CodeEvaluation/api-unit-test/TestResults/coveragereport/index.html