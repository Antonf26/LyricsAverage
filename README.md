# LyricsAverage

This is an MVC application that returns the average number of words in the songs of the artist whose name is passed in.

## Usage
### Docker 
This application can be run as a docker container (targetting Linux) as follows:
- Using a machine with docker installed and 
- Using a terminal (e.g.: Powershell), navigate to the root directory of the repo (containing the Dockerfile)
- Build the image: `docker build -t lyricsaverage:latest .` (first build will take a few minutes as base images need to be downloaded)
- Run the imaghe in a container: `docker run -p 8080:80 -d lyricsaverage` (replace 8080 with any available port on your machine if required)
- Browse to localhost:8080 (or port used in previous steps)
- Enter artist's name in the textbox and press "Search"

### Dotnet cli
If not using docker, the application can be built and run using the dotnet CLI. (.Net Core SDK 3.1 required)

- Using a terminal, navigate to the root directory of the repo
- Run the project using dotnet CLI: `dotnet run --project LyricsAverage\LyricsAverage.csproj`
- Browse to localhost:5000
- Enter artist's name in the textbox and press "Search"

### Visual Studio

Alternatively, open the project in Visual Studio and run it using IIS Express or Kestrel (Requires Visual Studio 2019).

## Tests
Unit tests are available in the LyricsAverage.Tests (using NUnit).
These can be run using the visual studio test runner or using dotnet test (from the repo root directory):
dotnet test .\LyricsAverage.Tests\LyricsAverage.Tests.csproj

## Configuration
The single solution-specific configuration value is the appsettings.json file: RequestTimeoutSeconds.
This sets a time out after which lyric averages are calculated and returned. 
This allows for calculating a result without waiting for a high amoutn of requests to the lyrics API to complete.
A higher value increases the likelihood of all the artists' songs being analysed but increases the potential time taken.




