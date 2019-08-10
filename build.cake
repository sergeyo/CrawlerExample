#tool "nuget:?package=NUnit.ConsoleRunner"
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#addin nuget:?package=Cake.Sonar

var sonarAuthKey = Argument("SonarAuthKey", "");

Task("RestorePackages")
    .Does(() => {
        NuGetRestore("Crawler.sln");
    });

Task("Compile")
    .IsDependentOn("RestorePackages")
    .Does(() => {
        MSBuild("Crawler.sln");
    });

Task("RunTests")
    .Does(() => {
        NUnit3("Bin\\*.Tests.dll");
    });
 
Task("SonarBegin")
  .Does(() => {
     SonarBegin(new SonarBeginSettings{
        Url = "https://sonarcloud.io/",
        Key = "Crawler.Example",
        Name = "CrawlerExample",
        Verbose = true,
        Login = sonarAuthKey,
       Organization = "sergeyo"
     });
  });

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonarAuthKey
     });
  });

Task("RunTestsWithSonar")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Compile")
    .IsDependentOn("RunTests")
    .IsDependentOn("SonarEnd");

RunTarget("RunTestsWithSonar");