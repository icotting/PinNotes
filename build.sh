#!/bin/bash

set -e

dotnet restore
dotnet test test/Unit/PinNotes.Accessors.Domain.Tests/

dotnet build "src/0 - Clients/PinNotes.Web" -c Release
