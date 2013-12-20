#r @"tools\FAKE\tools\FakeLib.dll"

open System
open System.IO
open System.Linq
open System.Text
open System.Text.RegularExpressions
open Fake
open Fake.MSTest

// --------------------------------------------------------------------------------------
// Definitions

let binProjectName = "NPerf.Fixtures"
let netVersion = "NET45"

let srcDir  = @".\src\"
let deploymentDir  = @".\deployment\"
let packagesDir = deploymentDir @@ "packages"

let dllDeploymentDir = packagesDir @@ @"lib" @@ netVersion
let nuspecTemplatesDir = deploymentDir @@ "templates"

let nugetPath = srcDir @@ @".nuget\nuget.exe"
let nugetPackagesDir = srcDir @@ @".\packages\"
let nugetAccessKey = if File.Exists(@".\Nuget.key") then File.ReadAllText(@".\Nuget.key") else ""
let version = File.ReadAllText(@".\version.txt")

let solutionAssemblyInfo = srcDir @@ "SolutionInfo.cs"
////let binProjectDependencies:^string list = ["NPerf"; "fastJSON"; "Orc"; "C5"; "Langman.TreeDictionary"]
let binProjectDependencies:^string list = ["JSON"; "Newtonsoft.Json"; "ServiceStack.Text"; "SimpleJson"; "Autofac"; "Castle.Core"; "Castle.Windsor";
                                           "Dynamo.Ioc"; "Ninject"; "structuremap"; "Unity";]
let binProjects = ["NPerf.Fixture.IDictionary"; "NPerf.Fixture.IIOCContainer"; "NPerf.Fixture.IList"; "NPerf.Fixture.ISerializer"; "NPerf.Fixture.ISorter"]

let outputDir = @".\output\"
let outputReleaseDir = outputDir @@ "release"//// @@ netVersion
let outputBinDir = outputReleaseDir
let testResultsDir = srcDir @@ "TestResults"

let ignoreBinFiles = "*.vshost.exe"
let ignoreBinFilesPattern = @"\**\" @@ ignoreBinFiles
let outputBinFiles = !! (outputBinDir @@ @"\**\*.*")
                            -- ignoreBinFilesPattern

let tests = srcDir @@ @"\**\*.Test.csproj" 
let allProjects = srcDir @@ @"\**\*.csproj" 

let testProjects  = !! tests
let otherProjects = !! allProjects
                        -- tests
                        -- @"\**\*.ISorterInt.csproj"
                        -- @"\**\*.IAccountForEfficiencies.csproj"
                        -- @"\**\*.AList.csproj"
                        -- @"\**\*.IIntervalContainer.csproj"

////let ignoreProjects = ["NPerf.Fixture.ISorterInt"; "NPerf.Fixture.IAccountForEfficiencies"; "NPerf.Fixture.AList"; "NPerf.Fixture.IIntervalContainer"]
////let getCsprojFilePath projectName = sprintf @"%s\**\%s.csproj" srcDir projectName
////let ignoreProjectsFiles = ignoreProjects |> List.map(fun d -> getCsprojFilePath d)
////otherProjects.Excludes = ignoreProjectsFiles

// --------------------------------------------------------------------------------------
// Clean build results

Target "CleanPackagesDirectory" (fun _ ->
    CleanDirs [packagesDir]
)

Target "DeleteOutputFiles" (fun _ ->
    !! (outputDir + @"\**\*.*")
        -- ignoreBinFilesPattern
    |> DeleteFiles
)

Target "DeleteOutputDirectories" (fun _ ->
    CreateDir outputDir
    DirectoryInfo(outputDir).GetDirectories("*", SearchOption.AllDirectories)
    |> Array.filter (fun d -> not (d.GetFiles(ignoreBinFiles, SearchOption.AllDirectories).Any()))
    |> Array.map (fun d -> d.FullName)
    |> DeleteDirs
)

// --------------------------------------------------------------------------------------
// Build projects

Target "UpdateAssemblyVersion" (fun _ ->
      let pattern = Regex("Assembly(|File)Version(\w*)\(.*\)")
      let result = "Assembly$1Version$2(\"" + version + "\")"
      let content = File.ReadAllLines(solutionAssemblyInfo, Encoding.Unicode)
                    |> Array.map(fun line -> pattern.Replace(line, result, 1))
      File.WriteAllLines(solutionAssemblyInfo, content, Encoding.Unicode)
)

Target "BuildOtherProjects" (fun _ ->    
    otherProjects
      |> MSBuildRelease "" "Rebuild" 
      |> Log "Build Other Projects"
)

Target "BuildTests" (fun _ ->    
    testProjects
      |> MSBuildRelease "" "Build"
      |> Log "Build Tests"
)

// --------------------------------------------------------------------------------------
// Run tests

Target "RunTests" (fun _ ->
    ActivateFinalTarget "CloseMSTestRunner"
    CleanDir testResultsDir
    CreateDir testResultsDir

    !! (outputDir + @"\**\*.Test.dll") 
      |> MSTest (fun p ->
                  { p with
                     TimeOut = TimeSpan.FromMinutes 20.
                     ResultsDir = testResultsDir})
)

FinalTarget "CloseMSTestRunner" (fun _ ->  
    ProcessHelper.killProcess "mstest.exe"
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    let nugetAccessPublishKey = getBuildParamOrDefault "nugetkey" nugetAccessKey
    let getOutputFile projectName ext = sprintf @"%s\%s.%s" outputBinDir projectName ext
    let getBinProjectFiles projectName =  [(getOutputFile projectName "dll")
                                           (getOutputFile projectName "xml")]
    let binProjectFiles = binProjects
                             |> List.collect(fun d -> getBinProjectFiles d)
                             |> List.filter(fun d -> File.Exists(d))

    let nugetDependencies = binProjectDependencies
                              |> List.map (fun d -> d, GetPackageVersion nugetPackagesDir d)
    
    let getNupkgFile = sprintf "%s\%s.%s.nupkg" dllDeploymentDir binProjectName version
    let getNuspecFile = sprintf "%s\%s.nuspec" nuspecTemplatesDir binProjectName

    let preparePackage filesToPackage = 
        CreateDir packagesDir
        CreateDir dllDeploymentDir
        CopyFiles dllDeploymentDir filesToPackage

    let cleanPackage name = 
        MoveFile packagesDir getNupkgFile
        DeleteDir (packagesDir @@ "lib")

    let doPackage dependencies =   
        NuGet (fun p -> 
            {p with
                Project = binProjectName
                Version = version
                ToolPath = nugetPath
                OutputPath = dllDeploymentDir
                WorkingDir = packagesDir
                Dependencies = dependencies
                Publish = not (String.IsNullOrEmpty nugetAccessPublishKey)
                AccessKey = nugetAccessPublishKey })
                getNuspecFile
    
    let doAll files depenencies =
        preparePackage files
        doPackage depenencies
        cleanPackage ""

    doAll binProjectFiles nugetDependencies
)

// --------------------------------------------------------------------------------------
// Combined targets

Target "Clean" DoNothing
"CleanPackagesDirectory" ==> "DeleteOutputFiles" ==> "DeleteOutputDirectories" ==> "Clean"

Target "Build" DoNothing
"UpdateAssemblyVersion" ==> "BuildOtherProjects" ==> "Build"

Target "Tests" DoNothing
"BuildTests" ==> "RunTests" ==> "Tests"

Target "All" DoNothing
"Clean" ==> "All"
"Build" ==> "All"
"Tests" ==> "All"

Target "Release" DoNothing
"All" ==> "Release"
"NuGet" ==> "Release"
 
RunTargetOrDefault "All"
