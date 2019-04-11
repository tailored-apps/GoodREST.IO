#!/bin/bash
set -ev
dotnet restore
dotnet build GoodREST.sln
