#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Services.Statistics
dotnet run --no-restore --urls "http://*:10004"