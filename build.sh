#!/bin/bash
set -ev
dotnet restore
dotnet test
dotnet build **\project.json
