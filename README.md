# FFMPEG

sudo apt update
sudo apt install ffmpeg
ffmpeg -i test.mp4 -vcodec libx265 -crf 28 output.mp4

# Sql

dotnet tool install --global dotnet-ef

dotnet-ef

dotnet ef migrations add VideoWithPhisycalPaths

dotnet ef database update

# Docker

sudo apt-get -y update
sudo apt-get -y upgrade
sudo apt-get install -y sqlite3 libsqlite3-dev 