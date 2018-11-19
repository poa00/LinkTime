
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var buildDirectory = Directory("./Build") + Directory(configuration);
var solutionFile = "LinkTime.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(MakeAbsolute(buildDirectory));
});

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      MSBuild(solutionFile, settings => settings.SetConfiguration(configuration).WithProperty("OutDir", MakeAbsolute(buildDirectory).FullPath));
    }
    else
    {
      XBuild(solutionFile, settings => settings.SetConfiguration(configuration).WithProperty("OutDir", MakeAbsolute(buildDirectory).FullPath));
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);