set msbuild="C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
set config=Release
set outdir=%CD%\Packages\bin
%msbuild% /p:Configuration=%config% "FLS.SharePoint/FLS.SharePoint.csproj" /t:Package /p:BasePackagePath=%outdir%