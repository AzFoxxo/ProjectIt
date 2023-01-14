#/usr/bin/env sh

# Compile the program into a native binary using dotnet 
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishAoT=true

# Copy the binary to the usr/local/bin directory
sudo cp bin/Release/net7.0/linux-x64/native/ProjectIt /usr/local/bin/projectit