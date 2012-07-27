<?xml version="1.0" encoding="utf-8"?>
<feature xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="54fecaa0-501f-48f5-b86c-77c704e3df0e" description="The Feature creates user profiles in default User Profile Service Application from xml file." featureId="54fecaa0-501f-48f5-b86c-77c704e3df0e" imageUrl="" receiverAssembly="$SharePoint.Project.AssemblyFullName$" receiverClass="$SharePoint.Type.ecba645a-1feb-4414-a8f8-71fab18f146a.FullName$" scope="WebApplication" solutionId="00000000-0000-0000-0000-000000000000" title="FLS.SharePoint.UserProfiles Create user profiles" version="" deploymentPath="$SharePoint.Project.FileNameWithoutExtension$_$SharePoint.Feature.FileNameWithoutExtension$" xmlns="http://schemas.microsoft.com/VisualStudio/2008/SharePointTools/FeatureModel">
  <activationDependencies>
    <referencedFeatureActivationDependency minimumVersion="" itemId="ed31d504-0890-4d0b-b6f6-137d56824c08" projectPath="..\FLS.SharePoint.System\FLS.SharePoint.System.csproj" />
    <referencedFeatureActivationDependency minimumVersion="" itemId="bae7b3c9-a3bd-423d-98eb-bf330f4989a0" projectPath="..\FLS.SharePoint.System\FLS.SharePoint.System.csproj" />
  </activationDependencies>
  <projectItems>
    <projectItemReference itemId="f4a479b1-c4e4-4976-9af1-7d3bc4fa0d95" />
  </projectItems>
</feature>