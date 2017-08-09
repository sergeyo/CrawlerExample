#tool "nuget:?package=NUnit.ConsoleRunner"

Task("Compile")
    .Does(() => {
        DotNetBuild("Crawler.sln");
    });

Task("RunTests")
    .IsDependentOn("Compile")
    .Does(() => {
        NUnit3("Bin\\*.Tests.dll");
    });

Task("Default")
    .IsDependentOn("RunTests");

RunTarget("Default");