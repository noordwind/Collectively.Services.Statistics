#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Services.Statistics
dotnet run --urls "http://*:10004"