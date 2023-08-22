#!/bin/bash

VERSION="1.1.181"
DOCUMENTATION="false"
ANALYZERS_DURING_BUILD="true"
ANALYZERS_DURING_LIVE_ANALYSIS="true"
ANALYZERS="true"
DOTNET="7.0"

NWDVERSIONDLL_LIST=(
"NWDUnity/Assets/NWDNuGet/NWDFoundation/NWDVersionDll.cs"
"NWDNuGet/NWDDatabaseAccess/NWDVersionDll.cs"
"NWDUnity/Assets/NWDNuGet/NWDStandardModels/NWDVersionDll.cs"
"NWDUnity/Assets/NWDNuGet/NWDCustomModels/NWDVersionDll.cs"
"NWDUnity/Assets/NWDNuGet/NWDRuntime/NWDVersionDll.cs"
"NWDUnity/Assets/NWDNuGet/NWDEditor/NWDVersionDll.cs"

"NWDNuGet/NWDCrucial/NWDVersionDll.cs"
"NWDUnity/Assets/NWDNuGet/NWDTreat/NWDVersionDll.cs"
"NWDNuGet/NWDPlayerStatistic/NWDVersionDll.cs"

"NWDNuGet/NWDWebRuntime/NWDVersionDll.cs"
"NWDNuGet/NWDWebStandard/NWDVersionDll.cs"
"NWDNuGet/NWDWebEditor/NWDVersionDll.cs"
"NWDNuGet/NWDWebGitLabReport/NWDVersionDll.cs"
"NWDNuGet/NWDWebTrackException/NWDVersionDll.cs"
"NWDNuGet/NWDWebPlayerDemo/NWDVersionDll.cs"
"NWDNuGet/NWDWebStudioDemo/NWDVersionDll.cs"
"NWDNuGet/NWDWebHttpErrorSimulator/NWDVersionDll.cs"
"NWDNuGet/NWDWebTreat/NWDVersionDll.cs"
"NWDNuGet/NWDWebDownloader/NWDVersionDll.cs"
"NWDNuGet/NWDWebUploader/NWDVersionDll.cs"
"NWDNuGet/NWDHub/NWDVersionDll.cs"
"NWDNuGet/NWDIdemobi/NWDVersionDll.cs"

"NWDNuGet/NWDServerShared/NWDVersionDll.cs"
"NWDNuGet/NWDServerBack/NWDVersionDll.cs"
"NWDNuGet/NWDServerMiddle/NWDVersionDll.cs"
"NWDNuGet/NWDServerFront/NWDVersionDll.cs"

"NWDNuGet/NWDServerHumanInterface/NWDVersionDll.cs"


"NWDNuGet/NWDWebDevelopment/NWDVersionDll.cs"
)
  
NUGET_LIST=(
"NWDNuGet/NWDFoundation/NWDFoundation.csproj"
"NWDNuGet/NWDDatabaseAccess/NWDDatabaseAccess.csproj"
"NWDNuGet/NWDStandardModels/NWDStandardModels.csproj"
"NWDNuGet/NWDCustomModels/NWDCustomModels.csproj"
"NWDNuGet/NWDRuntime/NWDRuntime.csproj"
"NWDNuGet/NWDEditor/NWDEditor.csproj"


"NWDNuGet/NWDCrucial/NWDCrucial.csproj"
"NWDNuGet/NWDTreat/NWDTreat.csproj"
"NWDNuGet/NWDPlayerStatistic/NWDPlayerStatistic.csproj"

"NWDNuGet/NWDWebRuntime/NWDWebRuntime.csproj"
"NWDNuGet/NWDWebStandard/NWDWebStandard.csproj"
"NWDNuGet/NWDWebEditor/NWDWebEditor.csproj"
"NWDNuGet/NWDWebGitLabReport/NWDWebGitLabReport.csproj"
"NWDNuGet/NWDWebTrackException/NWDWebTrackException.csproj"
"NWDNuGet/NWDWebPlayerDemo/NWDWebPlayerDemo.csproj"
"NWDNuGet/NWDWebStudioDemo/NWDWebStudioDemo.csproj"
"NWDNuGet/NWDWebHttpErrorSimulator/NWDWebHttpErrorSimulator.csproj"
"NWDNuGet/NWDWebTreat/NWDWebTreat.csproj"
"NWDNuGet/NWDWebDownloader/NWDWebDownloader.csproj"
"NWDNuGet/NWDWebUploader/NWDWebUploader.csproj"
"NWDNuGet/NWDHub/NWDHub.csproj"
"NWDNuGet/NWDIdemobi/NWDIdemobi.csproj"

"NWDNuGet/NWDServerShared/NWDServerShared.csproj"
"NWDNuGet/NWDServerBack/NWDServerBack.csproj"
"NWDNuGet/NWDServerMiddle/NWDServerMiddle.csproj"
"NWDNuGet/NWDServerFront/NWDServerFront.csproj"

"NWDNuGet/NWDServerHumanInterface/NWDServerHumanInterface.csproj"

"NWDNuGet/NWDWebDevelopment/NWDWebDevelopment.csproj"
)
  
PREPROD_PROJECT_LIST=(
  "../NWDPreprodVenture/NWDServer/NWDServer.csproj"
  "../NWDPreprodVenture/NWDWebPlayerTest/NWDWebPlayerTest.csproj"
  "../NWDPreprodVenture/NWDWebsite/NWDWebsite.csproj"
  "../NWDProdVenture/NWDServer/NWDServer.csproj"
  "../NWDProdVenture/NWDWebsite/NWDWebsite.csproj"
  "../../nwd.ooabab/NWDOoababHub/NWDOoababHub.csproj"
  "../../nwd.ooabab/NWDOoababServer/NWDOoababServer.csproj"
  "../../BBOCommunityWebsite/BBOCommunityWebsite/BBOCommunityWebsite.csproj"
  )

echo "==================================================================="
echo "                            Let's go!"
echo "==================================================================="

dotnet nuget remove source NetWorkedData && true
dotnet nuget add source "https://gitlab.hephaiscode.com/api/v4/projects/281/packages/nuget/index.json" --name NetWorkedData --username jfcontart --password glpat-u9AutH13b4qp_x8XuEcL --store-password-in-clear-text

declare NUGETNAME_LIST=()

SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
cd "${SCRIPT_DIR}/.."

echo " "
echo "==================================================================="
echo "                           Nuget go in "
echo "-------------------------------------------------------------------"
for key in "${!NWDVERSIONDLL_LIST[@]}"; do  
  #echo ${key} = ${NWDVERSIONDLL_LIST[$key]}
  sed -i.bak "s|\".*\"; *//VERSION|\"${VERSION}\"; //VERSION|g" "${NWDVERSIONDLL_LIST[$key]}"
  sed -i.bak "s|false; *//NUGET|true; //NUGET|g" "${NWDVERSIONDLL_LIST[$key]}"
  rm "${NWDVERSIONDLL_LIST[$key]}.bak"
done

for key in "${!NUGET_LIST[@]}"; do  
  CSPROJ="${SCRIPT_DIR}/../${NUGET_LIST[$key]}"
  NUGETNAME=$(basename "${NUGET_LIST[$key]}" .csproj)
  DIR=$(dirname "${SCRIPT_DIR}/../${NUGET_LIST[$key]}")
  echo " "
  echo "==================================================================="
  echo "                           ${key} : ${NUGETNAME} "
  echo "-------------------------------------------------------------------"
  echo ${DIR}
  echo ${NUGETNAME}
  echo ${CSPROJ}
  NUGETNAME_LIST[${#NUGETNAME_LIST[@]}]="${NUGETNAME}"
  sed -i.bak "s|<PackageVersion>.*</PackageVersion>|<PackageVersion>${VERSION}</PackageVersion>|g" "${CSPROJ}"
  sed -i.bak "s|<TargetFramework>net.*</TargetFramework>|<TargetFramework>net${DOTNET}</TargetFramework>|g" "${CSPROJ}"
  sed -i.bak "s|<AssemblyVersion>.*</AssemblyVersion>|<AssemblyVersion>${VERSION}</AssemblyVersion>|g" "${CSPROJ}"
  sed -i.bak "s|<GenerateDocumentationFile>.*</GenerateDocumentationFile>|<GenerateDocumentationFile>${DOCUMENTATION}</GenerateDocumentationFile>|g" "${CSPROJ}"
  sed -i.bak "s|<FileVersion>.*</FileVersion>|<FileVersion>${VERSION}</FileVersion>|g" "${CSPROJ}"
  sed -i.bak "s|<RunAnalyzersDuringBuild>.*</RunAnalyzersDuringBuild>|<RunAnalyzersDuringBuild>false</RunAnalyzersDuringBuild>|g" "${CSPROJ}"
  sed -i.bak "s|<RunAnalyzersDuringLiveAnalysis>.*</RunAnalyzersDuringLiveAnalysis>|<RunAnalyzersDuringLiveAnalysis>false</RunAnalyzersDuringLiveAnalysis>|g" "${CSPROJ}"
  sed -i.bak "s|<RunAnalyzers>.*</RunAnalyzers>|<RunAnalyzers>false</RunAnalyzers>|g" "${CSPROJ}"
    
  for NugetName in "${!NUGETNAME_LIST[@]}"; do  
    #echo "Uncomment package ${NUGETNAME_LIST[$NugetName]}"
    sed -i.bak "s|\<\!\-\- *\<PackageReference *Include\=\"${NUGETNAME_LIST[$NugetName]}\" Version\=\".*\" *\/\> *\-\-\>|\<PackageReference Include\=\"${NUGETNAME_LIST[$NugetName]}\" Version\=\"${VERSION}\" \/\>|g" "${CSPROJ}"
  done
  rm "${CSPROJ}.bak"

  rm ${DIR}/bin/Release/*.nupkg
  #dotnet clean ${CSPROJ} --configuration Release
  #dotnet restore ${CSPROJ} --force
  dotnet restore ${CSPROJ}
  #dotnet build ${CSPROJ} --configuration Release
  dotnet pack ${CSPROJ} --configuration Release
  dotnet nuget push "${DIR}/bin/Release/*.nupkg" --source NetWorkedData    
  #dotnet nuget push "${DIR}/bin/Release/*.nupkg" --source NetWorkedData --api-key "${SCRIPT_DIR}/../NetWorkedData.snk"         
  
  sed -i.bak "s|<RunAnalyzersDuringBuild>.*</RunAnalyzersDuringBuild>|<RunAnalyzersDuringBuild>${ANALYZERS_DURING_BUILD}</RunAnalyzersDuringBuild>|g" "${CSPROJ}"
  sed -i.bak "s|<RunAnalyzersDuringLiveAnalysis>.*</RunAnalyzersDuringLiveAnalysis>|<RunAnalyzersDuringLiveAnalysis>${ANALYZERS_DURING_LIVE_ANALYSIS}</RunAnalyzersDuringLiveAnalysis>|g" "${CSPROJ}"
  sed -i.bak "s|<RunAnalyzers>.*</RunAnalyzers>|<RunAnalyzers>${ANALYZERS}</RunAnalyzers>|g" "${CSPROJ}"
   for NugetName in "${!NUGETNAME_LIST[@]}"; do  
      #echo "Comment package ${NUGETNAME_LIST[$NugetName]}"
      sed -i.bak "s|\<PackageReference *Include\=\"${NUGETNAME_LIST[$NugetName]}\" Version\=\".*\" *\/\>|\<\!\-\- \<PackageReference Include\=\"${NUGETNAME_LIST[$NugetName]}\" Version\=\"${VERSION}\" \/\> \-\-\>|g" "${CSPROJ}"
    done
  rm "${CSPROJ}.bak"
  
done

echo " "
echo "==================================================================="
echo "                           Nuget go out "
echo "-------------------------------------------------------------------"
  
for key in "${!NWDVERSIONDLL_LIST[@]}"; do  
  #echo ${key} = ${NWDVERSIONDLL_LIST[$key]}
  sed -i.bak "s|true; *//NUGET|false; //NUGET|g" "${NWDVERSIONDLL_LIST[$key]}"
  rm "${NWDVERSIONDLL_LIST[$key]}.bak"
done

for key in "${!PREPROD_PROJECT_LIST[@]}"; do  
  CSPROJ="${SCRIPT_DIR}/../${PREPROD_PROJECT_LIST[$key]}"
  echo " "
  echo "==================================================================="
  echo "                           ${key} : ${CSPROJ} "
  echo "-------------------------------------------------------------------"
  for NugetName in "${!NUGETNAME_LIST[@]}"; do  
    #echo ${NUGETNAME_LIST[$NugetName]}
    sed -i.bak "s|<TargetFramework>net.*</TargetFramework>|<TargetFramework>net${DOTNET}</TargetFramework>|g" "${CSPROJ}"
    sed -i.bak "s|Include\=\"${NUGETNAME_LIST[$NugetName]}\" Version\=\".*\"|Include\=\"${NUGETNAME_LIST[$NugetName]}\" Version\=\"${VERSION}\"|g" "${CSPROJ}"
  done
  rm "${CSPROJ}.bak"
  dotnet clean ${CSPROJ} --configuration Debug
  dotnet clean ${CSPROJ} --configuration Release
  dotnet restore ${CSPROJ} --force
  
  cd ${CSPROJ}
  
  UpdateDate=$(date '+%Y-%m-%d %H:%M:%S')
  sed -i.bak "s|<DateTime>.*</DateTime>|<DateTime>${UpdateDate}</DateTime>|g" ../.gitlab-ci.yml
  sed -i.bak "s|GIT_STRATEGY\:.*|GIT_STRATEGY: ${GIT_STRATEGY}|g" ../.gitlab-ci.yml
  rm ../.gitlab-ci.yml.bak
  git status
  git add -u
  git commit -m "[QUICKLY] Quickly.sh commit command"
  git push origin main
  
done

echo "==================================================================="
echo "Finish! Enjoy version ${VERSION} !"
echo "==================================================================="
