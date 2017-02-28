#!/bin/bash
set -ev
dotnet restore
dotnet build **\project.json
