#!/bin/bash
set -ev
dotnet restore
dotnet build Wise.goodREST.sln
