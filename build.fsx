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
let netVersions = ["NET45"]

let srcDir  = @".\src\"
let deploymentDir  = @".\deployment\"
let packagesDir = deploymentDir @@ "packages"

let dllDeploymentDirs = netVersions |> List.map(fun v -> v, packagesDir @@ "work" @@ "lib" @@ v) |> dict
let nuspecTemplatesDir = deploymentDir @@ "templates"

let nugetExePath = srcDir @@ @".nuget\nuget.exe"
let nugetRepositoryDir = srcDir @@ @".\packages\"
let nugetAccessKey = if File.Exists(@".\Nuget.key") then File.ReadAllText(@".\Nuget.key") else ""
let version = File.ReadAllText(@".\version.txt")

let solutionAssemblyInfoPath = srcDir @@ "SolutionInfo.cs"
////let binProjectDependencies:^string list = ["NPerf"; "fastJSON"; "Orc"; "C5"; "Langman.TreeDictionary"]
let projectsToPackageAssemblyNames = ["NPerf.Fixture.IDictionary"; "NPerf.Fixture.IIOCContainer"; "NPerf.Fixture.IList"; "NPerf.Fixture.ISerializer"; "NPerf.Fixture.ISorter"]
let projectsToPackageDependencies:^string list = ["JSON"; "Newtonsoft.Json"; "ServiceStack.Text"; "SimpleJson"; "Autofac"; "Castle.Core"; "Castle.Windsor"; "NPerf";
                                           "Dynamo.Ioc"; "Ninject"; "structuremap"; "Unity";]

let outputDir = @".\output\"
let outputReleaseDir = outputDir @@ "release"//// @@ netVersion
let outputBinDir = outputReleaseDir ////@@ binProjectName
let getProjectOutputBinDirs netVersion projectName = outputBinDir @@ netVersion @@ projectName
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
    CleanDirs [packagesDir; testResultsDir]
)

Target "DeleteOutputFiles" (fun _ ->
    !! (outputDir + @"\**\*.*")
       ++ (testResultsDir + @"\**\*.*")
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
      let content = File.ReadAllLines(solutionAssemblyInfoPath, Encoding.Unicode)
                    |> Array.map(fun line -> pattern.Replace(line, result, 1))
      File.WriteAllLines(solutionAssemblyInfoPath, content, Encoding.Unicode)
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
    let getOutputFile netVersion projectName ext = sprintf @"%s\%s.%s" (getProjectOutputBinDirs netVersion projectName) projectName ext
    let getBinProjectFiles netVersion projectName =  [(getOutputFile netVersion projectName "dll")
                                                      (getOutputFile netVersion projectName "xml")]
    let binProjectFiles netVersion = projectsToPackageAssemblyNames
                                       |> List.collect(fun d -> getBinProjectFiles netVersion d)
                                       |> List.filter(fun d -> File.Exists(d))
    let nugetDependencies = projectsToPackageDependencies
                              |> List.map (fun d -> d, GetPackageVersion nugetRepositoryDir d)
    
    let getNuspecFile = sprintf "%s\%s.nuspec" nuspecTemplatesDir binProjectName

    let preparePackage filesToPackage = 
        CreateDir (packagesDir @@ "work")
        dllDeploymentDirs.Values |> Seq.iter (fun d -> CreateDir d)
        dllDeploymentDirs |> Seq.iter (fun d -> CopyFiles d.Value (filesToPackage d.Key))

    let cleanPackage name = 
        DeleteDir (packagesDir @@ "work")

    let doPackage dependencies =   
        NuGet (fun p -> 
            {p with
                Project = binProjectName
                Version = version
                ToolPath = nugetExePath
                OutputPath = packagesDir
                WorkingDir = packagesDir @@ "work"
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
