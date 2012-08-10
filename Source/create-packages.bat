set msbuild="C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe"
set config=Release
set outdir=%CD%\Packages\
%msbuild% /p:Configuration=%config% create-packages.build /p:OutDir=%outdir%
del  %outdir%\*.dll
del  %outdir%\*.pdb
