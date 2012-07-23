set msbuild="C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
set config=Release
set outdir=%CD%\Packages\
%msbuild% /p:Configuration=%config% "FLS.SharePoint/FLS.SharePoint.csproj" /t:Package /p:BasePackagePath=%outdir%
%msbuild% /p:Configuration=%config% "FLS.SharePoint.SiteStructure/FLS.SharePoint.SiteStructure.csproj" /t:Package /p:BasePackagePath=%outdir%
%msbuild% /p:Configuration=%config% "FLS.SharePoint.System/FLS.SharePoint.System.csproj" /t:Package /p:BasePackagePath=%outdir%